using UnityEngine;

namespace Game.Pools
{
    public class PoolableObject : MonoBehaviour
    {
        public string CacheKey { get; private set; }

        public void SetCacheKey(string prefabName)
        {
            CacheKey = prefabName;
        }

        public void ReturnToCache()
        {
            PoolingSystem.Instance.PutBackInPool(CacheKey, gameObject);
        }


    }
}
