using System;
using game.Config;
using UnityEngine;

namespace game
{
    public class GameEntry : MonoBehaviour
    {
        private void Start()
        {
            HS.ResourceManager.CreateInstance();
            HS.UIManager.CreateInstance();

            HS.UIManager.ShowMainPresenter<>(GameUIConfig.UIID.TestPanel, GameUIConfig.DicUIConfigs[GameUIConfig.UIID.TestPanel]);
        }
    }
    
}