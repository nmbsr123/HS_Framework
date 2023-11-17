using framework;
using framework.Resource;
using framework.UI;

namespace game
{
    public class HS
    {
        public static ResourceManager ResourceManager => ResourceManager.Instance;
        public static UIManager UIManager => UIManager.Instance;

        public static EventDispatch GameCommonEvent = new EventDispatch();
    }
}