using System;
using System.Collections.Generic;
using framework.Resource;
using framework.UI;
using game.Config;
using game.UI.Presenter;
using UnityEngine;

namespace game
{
    public class GameEntry : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
            GameRuner.CreateInstance();
            ResourceManager.CreateInstance();
            ResourceManager.Instance.Init();
            GameRuner.Instance.RegisterUpdate(AssetBundleManager.Instance);
            UIManager.CreateInstance();
            UIManager.Instance.Init();
        }

        private void Start()
        {
            var s = HS.ResourceManager.LoadPrefabSync<GameObject>("Assets/Res/AllInOne/11/GameObject");
            HS.UIManager.ShowMainPresenter<Panel_Test1Presenter>((int)GameUIConfig.UIID.TestPanel1, GameUIConfig.DicUIConfigs[GameUIConfig.UIID.TestPanel1], null, true);
        }

        private void Update()
        {
            GameRuner.Instance.Update(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            GameRuner.Instance.FixedUpdate();
        }

        private void LateUpdate()
        {
            GameRuner.Instance.LateUpdate();
        }
    }
    
}