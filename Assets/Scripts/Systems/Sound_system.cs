using UnityEngine;

namespace Game.Sounds
{
    public class Sound_system : MonoBehaviour
    {
        public static void Create_sound(string clip_name, float volume)
        {
            //Vars
            GameObject new_obj = new GameObject("New_sound/" + clip_name);

            //Set
            new_obj.AddComponent<AudioSource>();

            new_obj.GetComponent<AudioSource>().clip = Resources.Load("Sounds/" + clip_name) as AudioClip;
            new_obj.GetComponent<AudioSource>().volume = volume;

            new_obj.GetComponent <AudioSource>().Play();
        }

        public static void Create_sound(string clip_name, float volume, bool change_pitch)
        {
            //Vars
            GameObject new_obj = new GameObject("New_sound/" + clip_name);

            //Set
            new_obj.AddComponent<AudioSource>();

            new_obj.GetComponent<AudioSource>().clip = Resources.Load("Sounds/" + clip_name) as AudioClip;
            new_obj.GetComponent<AudioSource>().volume = volume;

            if (change_pitch )
            {
                new_obj.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
            }

            new_obj.GetComponent<AudioSource>().Play();
        }
    }
}

