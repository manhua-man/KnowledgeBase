gameobject应该加一个hider和Shower

延迟显示隐藏

```
private void SetUpGamePlayer(GamePlayer gamePlayer){

this.gamePlayer=gamePlayer;
//找到游戏控制器，并设置到流程的Timeline中
Animator animator=gamePlayer.GetComponent<Animator>();
pdSelf.SetAinmatorBinding(animator,"GamePlayer")
pdSelf.Shower(0.5f)
}
```

