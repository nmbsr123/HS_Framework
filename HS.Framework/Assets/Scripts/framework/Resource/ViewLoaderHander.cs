using UnityEngine;

namespace framework.Resource
{
    public class ViewLoaderHander : LoaderHandler
    {
        public GameObject gameObject => asset ? asset as GameObject : null;
    }
}