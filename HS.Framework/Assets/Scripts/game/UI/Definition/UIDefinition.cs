using System.Collections.Generic;
using framework.UI;

namespace Game
{
    enum UIID
    {
        testPanel = 0,
    }
    

    public static class UIDefinition
    {
        public static Dictionary<int, UIConfig> DicUIConfig = new Dictionary<int, UIConfig>()
        {
            [(int)UIID.testPanel] = new UIConfig()
            {
                ViewType = ViewType.Panel,
                Path = "",
                isLobby = true
            },
        };
    }

}