## ***\*NGUI的自适应性是？如果此时屏幕比例变化，屏幕出现黑边怎么办？（注：改变NGUI和UGUI的Fixed size with Screen不可行）\****

NGUI根目录的UIRoot组件自带了根据高度自适应分辨率的功能。

Scaling Style属性可选择三种不同的缩放策略。

PixelPerfect 完美像素：直接显示设定好的像素。当屏幕高度低于minimum Height时按比例缩小，当屏幕高度大于maximum Height时按比例扩大。

FixedSize 按比例缩放：在设定好的基础上，直接按比例缩放。

FixedSizeOnMobiles 合体版，android和ios为FixedSize方式，其它按照PixelPerfect方式。



 