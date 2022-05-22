锚点的作用是使控件到这四个点的距离不变，但若屏幕逐渐缩小时，控件自身会跟着缩小。通过这个可以做屏幕适配。

**锚点是什么，是怎么实现的，怎么计算距离**😢

[^]: 



RectTransform 组件：锚点，轴心，坐标值，旋转值，缩放值。


![锚点](https://cdn.jsdelivr.net/gh/manhua-man/blog.img/锚点.png)
	

Blueprint模式：未采用旋转或缩放的矩形框。
RawEditor模式：在编辑pivot时，pivot会始终保持在UI内的相对位置，UI跟随pivot的位置移动

轴心：轴心是UI元素旋转和缩放的基准点。下图中心位置的蓝色小圈为轴心点

![轴心](https://cdn.jsdelivr.net/gh/manhua-man/blog.img/轴心.png)



锚点：UI元素矩形框的位置和大小，以锚点在父物体的矩形框内的位置作为自身偏移和拉伸的参考。
锚点在编辑视图里是四个小三角形（锚点控制柄），这四个小三角组成里了一个锚框。
锚框的范围不能超过父类的矩形框，锚框的四个控制柄可以聚合到同一个位置。

为了便于理解界面里的数值，我们给锚点定义了俩种形态：锚点形态和锚框形态

锚点形态：物体的大小不会随着父物体的大小变化而变化，但是位置会根据轴心到锚点的距离一致的原则发生对应的变化

![锚点形态下的Rect组件](https://cdn.jsdelivr.net/gh/manhua-man/blog.img/锚点形态下的Rect组件.png)

锚框形态：物体的大小会随着父物体的大小变化而变化。

![锚框形态下的Rect组件](https://cdn.jsdelivr.net/gh/manhua-man/blog.img/锚框形态下的Rect组件.png)

锚框形态下界面里数值的意义：下图中，在黑框大小和位置变化的时候，会保证红框的左下角到锚框的左下角距离不变，同时红框的右上角到锚框的右上角距离不变。

![锚框形态下界面里数值的意义](https://cdn.jsdelivr.net/gh/manhua-man/blog.img/锚框形态下界面里数值的意义.JPG)

![RectTransform组件重要属性](https://cdn.jsdelivr.net/gh/manhua-man/blog.img/RectTransform组件重要属性.png)

轴心点就是UI元素旋转和缩放的基准点。

![UI元素旋转和缩放的基准点](https://cdn.jsdelivr.net/gh/manhua-man/blog.img/UI元素旋转和缩放的基准点.png)

锚点在代码里是由2个位置信息组成（对应界面里的Anchors Min 和 Anchors Max），这俩个位置决定里锚框的范围。计算出来

![锚点在代码里的2个位置信息](https://cdn.jsdelivr.net/gh/manhua-man/blog.img/锚点在代码里的2个位置信息.png)的UI元素的最终矩形框，该矩形框也是图片填充的范围。（只读属性)

![计算出来的UI元素的最终矩形框](https://cdn.jsdelivr.net/gh/manhua-man/blog.img/计算出来的UI元素的最终矩形框.png)

rect的x,y为轴心点到矩形框的左下角向量值![rect的x,y为轴心点到矩形框的左下角向量值]

![rect的x,y为轴心点到矩形框的左下角向量值](https://cdn.jsdelivr.net/gh/manhua-man/blog.img/rect的x,y为轴心点到矩形框的左下角向量值.png)

如下图，最终得到的都是一个指向UI元素的vector2向量。在锚框形态下我们可以在代码里面动态的去调整UI元素相对锚框边界的距离。

![offsetMax](https://cdn.jsdelivr.net/gh/manhua-man/blog.img/offsetMax.png)

![在锚框形态下我们可以在代码里面动态的去调整UI元素相对锚框边界的距离](https://cdn.jsdelivr.net/gh/manhua-man/blog.img/在锚框形态下我们可以在代码里面动态的去调整UI元素相对锚框边界的距离.JPG)

![anchoredPosition](https://cdn.jsdelivr.net/gh/manhua-man/blog.img/anchoredPosition.png)

通俗讲，在锚点形态下，该值就是表示锚点到UI轴心的矢量值。
通俗讲，在锚框形态下，该值就是表示锚框里某一个点到UI轴心的矢量值![anchoredPosition的表现](https://cdn.jsdelivr.net/gh/manhua-man/blog.img/anchoredPosition的表现.png)
