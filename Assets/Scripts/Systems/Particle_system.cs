using UnityEngine;

namespace Game.Particles
{
    public class Particle_system : MonoBehaviour
    {
        public static void Create_particle(string particle_name, Vector2 pos)
        {
            //Set
            GameObject new_particle = Instantiate(Resources.Load("Prefabs/Particles/" + particle_name), pos, Quaternion.identity) as GameObject;
        }
    }
}