using UnityEngine;

namespace Game.Sounds
{
    public class Sound_system : MonoBehaviour
    {
        public static void Create_sound(string clip_name, float volume)
        {
            var sfx = PoolingSystem.Instance.Get<SFX>("SFX");
            sfx.Initialize(clip_name, volume);           
        }

        public static void Create_sound(string clip_name, float volume, bool change_pitch)
        {
            var sfx = PoolingSystem.Instance.Get<SFX>("SFX");
            sfx.Initialize(clip_name, volume, change_pitch);
        }       
    }
}

