using YooAsset;

namespace ET
{
    public static class InitAssetHelper
    {
        public static AssetOperationHandle LoadSync<T>(string path) where T : UnityEngine.Object
        {
            AssetOperationHandle handle = YooAssets.LoadAssetSync<T>(path);
            return handle;
        }
    }
}