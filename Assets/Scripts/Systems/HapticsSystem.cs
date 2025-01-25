using StarTravellers.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Haptics
{
    public class HapticsSystem : MonoBehaviour
    {
        public static HapticsSystem Instance { get; private set; }

        private float _currentLowIntensity = 0f;
        private float _currentHighIntensity = 0f;
        private float _timeNextPulse = 0f;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDisable()
        {
#if UNITY_EDITOR || !PLATFORM_WEBGL
            if (Gamepad.current != null)
            {
                for (var i = 0; i < Gamepad.all.Count; i++)
                {
                    Gamepad.all[i].SetMotorSpeeds(0f, 0f);
                }
            }
#endif
        }

        public void LowRumble(float intensity, float duration) => StartCoroutine(Rumble(intensity, 0, duration));
        public void HighRumble(float intensity, float duration) => StartCoroutine(Rumble(0, intensity, duration));

        private IEnumerator Rumble(float lowIntensity, float highIntensity, float duration)
        {
#if UNITY_EDITOR || !PLATFORM_WEBGL
            if (Gamepad.current != null)
            {
                Gamepad.current.SetMotorSpeeds(lowIntensity, highIntensity);
                yield return new WaitForSeconds(duration);
                Gamepad.current.SetMotorSpeeds(_currentLowIntensity, _currentHighIntensity);
                //for (var i = 0; i < Gamepad.all.Count;i++)
                //{
                //    Gamepad.all[i].SetMotorSpeeds(0f, 0f);
                //}                
            }
#else
            yield return AnimationHelpers.WaitForEndOfFrame;
#endif

        }

        public void StartRumble(float lowIntensity, float highIntensity)
        {
#if UNITY_EDITOR || !PLATFORM_WEBGL
            if (Gamepad.current != null)
            {
                _currentHighIntensity = highIntensity;
                _currentLowIntensity = lowIntensity;

                Gamepad.current.SetMotorSpeeds(_currentLowIntensity, _currentHighIntensity);
            }
#endif
        }

        public void StopRumble()
        {
#if UNITY_EDITOR || !PLATFORM_WEBGL
            if (Gamepad.current != null)
            {
                _currentHighIntensity = 0f;
                _currentLowIntensity = 0f;
                Gamepad.current.SetMotorSpeeds(_currentLowIntensity, _currentHighIntensity);
            }
#endif
        }


        public void Pulse(float intensity)
        {
#if UNITY_EDITOR || !PLATFORM_WEBGL
            if (Gamepad.current != null)
            {
                if (Time.timeSinceLevelLoad > _timeNextPulse)
                {
                    _timeNextPulse = Time.timeSinceLevelLoad + .6f;
                    StartCoroutine(Rumble(intensity, intensity, .2f));
                }
            }
#endif
        }


    }
}