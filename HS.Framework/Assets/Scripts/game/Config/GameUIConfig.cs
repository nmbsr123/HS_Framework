using System.Collections.Generic;
using framework.UI;

namespace game.Config
{
    public class GameUIConfig
    {
        public enum UIID
        {
            TestPanel1 = 1,
        }
        
        public static Dictionary<UIID, UIConfig> DicUIConfigs = new Dictionary<UIID, UIConfig>()
        {
            [UIID.TestPanel1] = new UIConfig()
            {
                uiID = (int)UIID.TestPanel1,
                viewType = ViewType.Panel,
                path = "Assets/Res/OneInOne/UI/Prefab/Panel_Test",
                isLobby = true
            }
        };
    }
}