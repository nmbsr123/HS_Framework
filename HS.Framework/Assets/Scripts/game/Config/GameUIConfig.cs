using System.Collections.Generic;
using framework.UI;

namespace game.Config
{
    public class GameUIConfig
    {
        public enum UIID
        {
            TestPanel = 1,
        }
        
        public static Dictionary<UIID, UIConfig> DicUIConfigs = new Dictionary<UIID, UIConfig>()
        {
            [UIID.TestPanel] = new UIConfig()
            {
                
            }
        };
    }
}