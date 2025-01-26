using Game.Pools;
using UnityEngine;

namespace Game.Particles
{
    public class Particle_system : MonoBehaviour
    {
        public static void Create_particle(string particle_name, Vector2 pos)
        {
            if (PoolingSystem.Instance)
            {
                var newParticle = PoolingSystem.Instance.Get<PoolableParticles>(particle_name);
                newParticle.transform.position = pos;
                newParticle.transform.rotation = Quaternion.identity;
            }            
        }
    }
}