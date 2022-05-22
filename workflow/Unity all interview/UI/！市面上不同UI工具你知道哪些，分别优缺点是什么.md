## 市面上不同UI工具你知道哪些，分别优缺点是什么

IMGUI是Unity自带的古老UI系统。
NGUI是最流行的第三方UI插件。
FairyGUI是跨平台的UI系统。（商业案例：小游戏居多）
UGUI是官方版本。
UI Element是最新版本的UI系统。（Untiy2019版）

为什么选择UGUI：团队最擅长（重点），官方支持，流行趋势，多线程支持（UI重建）。😢



![UGUI和NGUI的使用分布](https://cdn.jsdelivr.net/gh/manhua-man/blog.img/UGUI和NGUI的使用分布.png)

![NGUI与UGUI堆内存主体范围](https://cdn.jsdelivr.net/gh/manhua-man/blog.img/NGUI与UGUI堆内存主体范围.png)







\1) NGUI还保留着图集，需要进行图集的维护。而UGUI没有图集的概念，可以充分利用资源，避免重复资源。

\2) UGUI出现了锚点的概念，更方便屏幕自适应。 

\3) NGUI支持图文混排，UGUI暂未发现支持此功能。 

\4) UGUI没有 UIWrap 来循环 scrollview 内容。

\5) UGUI暂时没有集成Tween组件。 

## UGUI和NGUI的区别？为什么不使用NGUI？

1、uGUI的Canvas 有世界坐标和屏幕坐标，NGUI有2D和3D区别。 

2、uGUI的Image可以使用material。

3、UGUI通过Mask来裁剪，而NGUI通过Panel的Clip

4、NGUI的渲染前后顺序是通过Widget的Depth，而UGUI渲染顺序根据Hierarchy的顺序，越下面渲染在顶层。

 \1) 、UGUI 不需要绑定Colliders，UI可以自动拦截事件。

 \2) 、UGUI的Anchor是相对父对象，没有提供高级选项，个人感觉uGUI的Anchor操作起来比NGUI更方便

7、UGUI没有Atlas一说，使用Sprite Packer。

8、UGUI的Navgation在Scene中能可视化。

9、UGUI的事件需要实现事件系统的接口，但写起来也算简单。

10、NGUI功能更丰富一些

之所以不用NGUI是因为UGUI是Unity官方推出的，慢慢会成为制作UI的主要工具，配套的插件也越来越多，但是具体使用NGUI还是UGUI还要看公司这边，因为这两个我都用过一段时间。