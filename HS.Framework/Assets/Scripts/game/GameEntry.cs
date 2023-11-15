using System;
using framework.Resource;
using framework.UI;
using game.Config;
using game.UI.Presenter;
using UnityEngine;

namespace game
{
    public class GameEntry : MonoBehaviour
    {
        private void Start()
        {
            ResourceManager.CreateInstance();
            ResourceManager.Instance.Init();
            UIManager.CreateInstance();
            UIManager.Instance.Init();
            var s = HS.ResourceManager.LoadPrefabSync<GameObject>("Assets/Res/AllInOne/11/GameObject");
            HS.UIManager.ShowMainPresenter<Panel_Test1Presenter>((int)GameUIConfig.UIID.TestPanel1, GameUIConfig.DicUIConfigs[GameUIConfig.UIID.TestPanel1]);
        }
    }
    
}