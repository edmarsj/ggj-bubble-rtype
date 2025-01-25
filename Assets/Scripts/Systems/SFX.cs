using Game.Pools;
using System.Collections;
using UnityEngine;

namespace Game.Sounds
{
    public class SFX : PoolableObject
    {
        private AudioSource _audioSource;        

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();           
        }

        public void Initialize(string clipName, float volume, bool changePitch = false)
        {
            _audioSource.clip = Resources.Load<AudioClip>("Sounds/" + clipName);

            //if (changePitch)
            //{
            //    _audioSource.pitch = Random.Range(0.9f, 1.1f);
            //}

            _audioSource.Play();
            Invoke(nameof(ReturnToCache), _audioSource.clip.length);         
        }            
    }
}