Drawcall：CPU对底层图形绘制接口的调用命令GPU执行渲染操作，渲染流程采用流水线实现，CPU和GPU并行工作，它们之间通过命令缓冲区连接，CPU向其中发送渲染命令，GPU接收并执行对应的渲染命令。

DrawCall,对底层图形程序（比如：OpenGL ES)接口的调用

## ***\*如何减少Drawcall？\****

1.使用Draw Call Batching，静态批处理。

2.通过把纹理打包成图集来尽量减少材质的使用。