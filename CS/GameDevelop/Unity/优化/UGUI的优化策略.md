### 优化Unity UI系统的首要任务是找到性能问题的准确原因。



#### 开发过程中四个常见的问题

过多的GPU片段着色器使用率（如屏幕填充率过高）
过多的CPU时间开销在重建一个画布上
过多的CPU时间开销在生成顶点上（通常是文本）
过多的画布重建次数

针对这四个问题来分组介绍优化策略
网格重建优化策略
屏幕填充率优化策略
合批优化策略
字体优化策略
滚动视图优化策划
其它优化策略



#### 网格重建优化策略（Mesh）


使用尽可能少的UI元素：在制作UI时，一定要仔细查检UI层级，删除不必要的UI元素，这样可以减少深度排序的时间以及Rebuild的时间。

减少Rebuild的频率：将动态UI元素（频繁改变例如顶点、alpha、坐标和大小等的元素）与静态UI元素分离出来，放到特定的Canvas中。

谨慎使用UI元素的active操作：因为它们会触发耗时较高的rebuild。

谨慎使用Canvas的Pixel Perfect选项：该选项的开启会导致UI元素在发生位移时，其长宽会被进行微调（为了对齐像素），从而造成layout Rebuild。（比如ScrollRect滚动时，会使得Canvas.SendWillRenderCanvas消耗较高）

Animator最佳用法： Animator每帧都会改变元素，即使动画中的数值没有变化，因为Animator没有空指令检查。对于仅响应事件时才变化的元素，可以自行编写代码或使用第三方补间插件。

谨慎用Tiled类型的Image

![谨慎用Tiled类型的Image](https://cdn.jsdelivr.net/gh/manhua-man/blog.img/谨慎用Tiled类型的Image.png)

#### 屏幕填充率优化策略（OverDraw）


禁用不可见的面板：比如当打开一个系统时如果完全挡住了另外一个系统，则可以将被遮挡住的系统面板禁用。（龙与少女优化方案：通过修改Canvas对象的Layer隐藏面板。）

不要使用空的Image做按键响应：在Unity中Raycast使用Graphic作为基本元素来检测touch。如果使用空的image也会产生不必要的overdraw。可以实现一个只在逻辑上响应Raycast但是不参与绘制的组件即可。

Polygon Mode Sprites：如果图片边缘有大片留白就会产生很多无用填充。Unity和Texture Packer目前都支持了Polygon Mode，也就是说将原来的矩形Sprite用更加紧致的Polygon来描述。

Image Fill Center：在Image Type选项为Sliced的情况下，不需要Fill Center的时候去掉勾选。

![OverDraw和多边形精灵示意图](https://cdn.jsdelivr.net/gh/manhua-man/blog.img/OverDraw和多边形精灵示意图.png)



#### 合批优化策略（DrawCall）


相同层级原则：父节点下所有子节点，尽量保持相同的层次结构。相同层级下的UI元素可以Batch。

Mask组件：Mask组件使用了模版缓存，Mask中的UI元素无法与外界UI元素合批，Mask组件还会额外增加2个DrawCall.

隐藏的Image：Image组件中sprite为空，都是占用drawcall渲染的，并且还会打断前后元素的合批。

Screen Space-Camera模式：一个Canvas中的任何一个UI元素只要在屏幕中，则这个Canvas中的其他UI元素即使在屏幕外DrawCall仍不会减少。

Hierarchy穿插重叠问题：如下图红点和Icon在不同图集中，如果红点稍微大一点，遮挡了旁边的Icon，就不能合批，须要调整Icon和红点的节点关系，4个Icons放在一个节点下，4个红点放在一个借点下。

![合批优化策略示意图](https://cdn.jsdelivr.net/gh/manhua-man/blog.img/合批优化策略示意图.png)



#### 字体优化策略（Font）


字体图集的重建机制：当一个新文字出现的时候，会被添加到字体图集，如果图集已经没有空余的地方，那么图集会被重建。图集会以相同的尺寸重建，打包当前激活的所有UI text组件中要显示的文字，如果发现图集尺寸不够用的时候，图集会重新扩充尺寸。

后备字体机制：对于字体库里没有的文字，会被放进后备字体图集里，后背字体图集会常驻内存里，不会被销毁。后备字体取自于系统自带的系统字库Arial.ttf，在发布的游戏安装包里该字库是不存在的。我们在一些Unity开发的游戏里，偶尔会发现一些生僻字的字形和其它常见文字的字形不统一。

Text的网格重建：Text组件被重新启用的时候，会重建Text的网格。如果含有大量的文字，会造成严重的CPU开销。

提前生成动态字体：准备游戏非常常用的文字集合，通过Font.RequestCharactersInTexture接口提前放入字体图集里。注意使用Font.textureRebuilt 委托，在字体图集被重新重建的时候，把我们提前准备的文字集合再次添加进去。

使用美术数字：游戏的分数，可以使用美术数字（精灵图片）来代替Text组件。

谨慎使用Text的Best Fit选项：虽然这个选项可以动态的调整字体大小以适应UI布局而不会超框，但其代价是很高的，Unity会为用到的该元素所用到的所有字号生成图元保存在图集里，不但增加额外的生成时间，还会使得字体对应的图集变大。

减少长文本Text的变动，慎用UI/Effect：描边和阴影效果都会增大四倍的顶点数



#### 滚动视图优化策略（ScrollView）

有两种方法填充滚动视图
用所有需要出现在滚动视图的元素填充滚动视图
用池处理这些元素，根据需要重新放置它们的位置

RectMask2D组件：俩种方法可以通过给滚动视图添加一个RectMask2D组件来提高性能。该组件确保在滚动视图窗口外面的滚动视图元素不会出现在可画的元素列表中，省去了该元素的batch。

一种简单的缓存池策略：在UI中布局中，使用带有Layout Element组件的对象占位（ Slot ）。给可见UI元素实例一个池，来填充滚动视图看可见区域，Slot作为父物体来定位。

基于位置的缓存池策略：通过移动布局里UI元素的RectTransforms坐标值，来排序显示位置。通常写一个自定义的滚动视图类或者写一个自定义布局组的组件。



#### 其它优化策略

禁用无用的Raycast：UGUI的touch处理消耗也可能会成为性能热点。因为UGUI在默认情况下会对所有可见的Graphic组件调用raycast。对于不需要接收touch事件的grahic，一定要禁用raycast。（龙与少女为策划提供了检视的辅助脚本）


![禁用无用的Raycast](https://cdn.jsdelivr.net/gh/manhua-man/blog.img/禁用无用的Raycast.png)


OverrideSorting：子Canvas中的OverrideSorting属性将会造成Graphic Raycast测试停止遍历Transform层级。

UI对象的坐标Z值：Z值不为零的时候会影响对象渲染顺序并不能合批。（例如：龙与少女里的阵型界面都是修改Spine的SortingOrder来实现位置排序）

网格开销巨大：如果出现了WaitingForJob或PutGeometryJobFence，则说明合并网格开销巨大（子线程网格合并)

高级技巧：对于处于选中播放动画的需求，并且所处canvas下内容比较多的情况下，可以单独把选中对象放到预先建好的动态canvas里，取消选中时再放回去。

CanvasGroup的使用
在窗口的GameObject上添加一个CanvasGroup，通过控制它的Alpha值来淡入或淡出整个窗口。
在窗口的GameObject上添加一个CanvasGroup，通过设置它Interactable值来控制底层所有控件的交互开关。







## ***\*说说你对UI优化思路\****

使用图集、减少UI之间的覆盖、使用canvas对不同层级的UI进行分层、销毁不使用的UI界面。

 

***\*UI在不同canves层级下有什么好处\****

Canvas的CPU方面消耗主要是Rebatch和Rebuild。

Rebatch发生在C++层面，是指Canvas分析UI节点生成最优批次（渲染指令）的过程，节点数量过多会导致算法（贪心策略）耗时较长，关于Batching的过程可以参考：[Guess Into UGUI](https://zhuanlan.zhihu.com/p/28897082)

Rebuild发生在C#层面，是指UGUI库中Layout组件调整RectTransform尺寸、Graphic组件更新Mesh和Material，以及Mask执行Cull的过程，耗时和发生变化的节点（动态节点）数量基本呈线性相关。

Canvas下UI节点数量较多时，如果节点都是静态不怎么变动的，那么问题不大，Rebatch之后结果会进行缓存复用直到下一次节点变化；如果某些节点经常变动，会引起Canvas的Rebatch和Rebuild，CPU耗时会相应增加。

Unity5.2之后Rebacth的过程进行了优化，会根据设备CPU核心数量多线程执行，但是如果每隔几帧就要Rebatch/Rebuild一次，那么耗时还是比较可观的，例如UI元素含有Animator动画，频繁缩放、修改Sprite等等。这时候就建议将Canvas进行拆分，进行动静分离等策略，详细可以参考Unity官方优化UGUI的文档：[Optimizing Unity UI](https://learn.unity.com/tutorial/optimizing-unity-ui)

 

 

## ***\*UGUI渲染原理层级顺序\*******\*，\*******\*以此怎么优化，加深研究\****

1，当有多个canvas并且渲染模式都为Overlay。

这种情况下，渲染顺序是由canvas组件下的Sort Order决定的，值越大的越后渲染。

2，当有多个canvas并且渲染模式都为Camera。 

这种情况下，渲染顺序首先由Rendener Camera的Depth值决定，值越大越后渲染。

如果Depth值相同，那么由canvas组件下的Sortint Layer顺序决定，顺序越后则越后渲染。

如果Depth值和Layer值都相同的情况下，渲染顺序由Order in Layer决定，值越大越后渲染。 （注意：若Rendener Camera都是同一个摄像机，则不考虑Depth情况）

3，当有多个canvas并且渲染模式都为World。 

这种情况下，渲染顺序由canvas组件下的Sortint Layer顺序决定，顺序越后则越后渲染。

如果Layer值都相同的情况下，渲染顺序由Order in Layer决定，值越大越后渲染。

特别注意，当Layer和Order值都相同的情况下，此时渲染顺序由canvas距离Render Camera的距离决定，距离越近越后渲染。（ 只有World模式有这种情况，因其他模式下canvas都是位置固定不可移动的。并且以上情况只考虑canvas都出现在Render Camera摄像范围的情况下。）

4，当有多个canvas并且渲染模式都存在的情况下。

这种情况下，首先Overlay模式的canvas永远是最后渲染，并且同为Overlay模式的canvas在Sort Order的值越大时越后渲染。

其次，Camera和Overlay同时存在的情况下有两种情况：若使用的不同的相机，则由摄像机的Depth决定，值越大越后渲染。若使用的是相同的相机，则是由canvas距离摄像机的距离决定的，距离越近的越后渲染。