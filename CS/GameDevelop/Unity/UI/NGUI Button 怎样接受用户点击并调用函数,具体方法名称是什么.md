## ***\*NGUI Button 怎样接受用户点击并调用函数,具体方法名称是什么\****

1、主要是在UICamera脚本中用射线判断点击的物体并通过SendMessage调用OnClick() OnPress()等函数，可以说NGUI的按钮是通过发消息这个方式调用的。具体方法名称是OnClick()

2、void Awake () {  

​    //获取需要监听的按钮对象

​    GameObject button = GameObject.Find("UI Root/Button3");

​    //设置这个按钮的监听，指向本类的ButtonClick方法中。

​    UIEventListener.Get(button).onClick = OnButton3Click;

  } 

  private void OnButton3Click(GameObject button) {

​    Debug.Log("我是按钮3被点击了");

}