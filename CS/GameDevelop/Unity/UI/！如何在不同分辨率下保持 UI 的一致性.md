## ***\*请简述如何在不同分辨率下保持 UI 的一致性\****

NGUI 很好的解决了这一点，屏幕分辨率的自适应性，原理就是计算出屏幕的宽高比跟原来的预设的屏幕分辨率求出一个对比值，然后修改摄像机的 size。