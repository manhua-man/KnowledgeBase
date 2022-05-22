#### 重要概念

Canvas Batch：Canvas下的UI元素最终都会被Batch到同一个Mesh中，而在Batch前，会根据这些UI元素的材质（通常就是Atlas）以及渲染顺序进行重排，在不改变渲染结果的前提下，尽可能将相同材质的UI元素合并在同一个SubMesh中，从而把DrawCall降到最低。Batch的结果会被缓存复用，直到这个Canvas被标记为dirty。

Unity官方的重要提示：当给定Canvas上的任何可绘制UI元素发生更改时，Canvas必须重新执行合批过程。此过程重新分析Canvas上的每个可绘制UI元素，不管它是否被修改。注意，“更改”是指影响UI元素外观的任何变动，包括修改sprite renderer的sprite、transform的position和scale、文本网格的text等。
Canvas嵌套：Canvas可以嵌套使用，一个子Canvas下dirty的子物体不会触发父Canvas的rebuild。

#### 网格重建因素


UI顶点属性变化会引发网格更新
修改Image、Text的color属性，会改变UIVertex.color
修改RectTransform的Size、Anchors、Pivot等，会改变UIVertex.position
注意：在UGUI中颜色的变化是通过修改顶点色实现的，避免生成了新的DrawCall
注意：UIVertex.position记录的是本地空间下的坐标

![UIvertex](https://cdn.jsdelivr.net/gh/manhua-man/blog.img/UIvertex.png)

#### Rebuild流程图

![Rebuild流程图](https://cdn.jsdelivr.net/gh/manhua-man/blog.img/Rebuild流程图.png)

##### 流程图说明：


该过程由CanvasUpdateRegistry监听Canvas的WillRenderCanvases（上图中1）而执行,主要是对当前标记为dirty的layout和graphic执行rebuild。

在rebuild layout之前会对Layout rebuild queue中的元素依据它们在heiarchy中的层次进行排序（上图中的2），排列的结果是越靠近根的节点越会被优先处理。

rebuild layout（上图中的3）,主要是执行ILayoutElement和ILayoutController接口中的方法来计算位置，Rect的大小等布局信息。

rebulid graphic（上图中的4）,主要是调用UpdateGeometry重建网格的顶点数据（上图中5）以及调用UpdateMeterial更新CanvasRender的材质信息（上图中6）。

Question：为什么在rebuild layout的时候，要优先处理根节点的元素？



#### 合批原理： UGUI的合批规则是进行重叠检测，然后分层合并。


第一步计算每个UI元素的层级号：如果有一个UI元素，它所占的矩形范围内，如果没有任何UI在它的底下，那么它的层级号就是0（最底下）；如果有一个UI在其底下且该UI可以和它Batch，那它的层级号与底下的UI层级一样；如果有一个UI在其底下但是无法与它Batch，那它的层级号为底下的UI的层级+1；如果有多个UI都在其下面，那么按前两种方式遍历计算所有的层级号，其中最大的那个作为自己的层级号。

第二步合并相同层级中可以Batch的元素作为一个批次，并对批次进行排序 ：有了层级号之后，Unity会将每一层的所有元素进行一个排序（按照材质、纹理等信息），合并掉可以Batch的元素成为一个批次。经过以上排序，就可以得到一个有序的批次序列了。这时Unity会再做一个优化，即如果相邻间的两个批次正好可以Batch的话就会进行Batch。合批的Batch数据，最后会分别放在CanvasMesh的SubMesh里。

![合批的UI图](https://cdn.jsdelivr.net/gh/manhua-man/blog.img/合批的UI图.JPG)

