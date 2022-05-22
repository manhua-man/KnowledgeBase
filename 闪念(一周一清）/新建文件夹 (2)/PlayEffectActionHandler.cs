/*
* 作者：Code Make Me bald
* 联系方式：1476802919@qq.com
* 文档: 暂无
* 创建时间: 2021年3月16日
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XMLib.AM;

namespace AliveCell
{
    /// <summary>
    /// PlayEffectConfig
    /// </summary>
    [ActionConfig(typeof(PlayEffect))]
    [Serializable]
    public class PlayEffectConfig : HoldFrames
    {
        public String effectName;
        public String parentName;
        public Single lifeTime;
        public Vector3 localPosition;
        public Vector3 localRotation;
        public bool updateTransform;
    }

    /// <summary>
    /// PlayEffect
    /// </summary>
    public class PlayEffect : IActionHandler
    {
        public GameObject effec;

        public void Enter(ActionNode node)
        {
            PlayEffectConfig config = (PlayEffectConfig)node.config;
            //IActionMachine machine = node.actionMachine;
            ActionMachineController controller = (ActionMachineController)node.actionMachine.controller;
            //node.data = new Data();

            effec = (GameObject)MonoBehaviour.Instantiate(Resources.Load(config.effectName));
            Transform parent = controller.gameObject.transform.Find(config.parentName);
            effec.transform.parent = parent;
            effec.transform.localPosition = config.localPosition;
            effec.transform.localRotation = Quaternion.Euler(config.localRotation);
                
            if(!config.updateTransform) effec.transform.SetParent(null);    //特效坐标不跟随人物
            
            UnityEngine.Object.Destroy(effec, config.lifeTime);
        }

        public void Exit(ActionNode node)
        {
            //PlayEffectConfig config = (PlayEffectConfig)node.config;
            //IActionMachine machine = node.actionMachine;
            //IActionController controller = (IActionController)node.actionMachine.controller;
            //Data data = (Data)node.data;
            
        }

        public void Update(ActionNode node, Single deltaTime)
        {
            //PlayEffectConfig config = (PlayEffectConfig)node.config;
            //IActionMachine machine = node.actionMachine;
            //IActionController controller = (IActionController)node.actionMachine.controller;
            //Data data = (Data)node.data;
        }
    }
}