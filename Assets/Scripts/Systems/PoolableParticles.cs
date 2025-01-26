using UnityEngine;

namespace Game.Pools
{
    public class PoolableParticles : PoolableObject
    {
        [SerializeField] private ParticleSystem _particleSystem;

        public void Update()
        {
            if (_particleSystem)
            {
                if (!_particleSystem.IsAlive())
                {
                    gameObject.SetActive(false);
                    ReturnToCache();
                }
            }
        }
    }
}