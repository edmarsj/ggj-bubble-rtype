using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Haptics
{
    public class HapticsSystem : MonoBehaviour
    {
        public static HapticsSystem Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void OnDisable()
        {
            if (Gamepad.current != null)
            {
                for (var i = 0; i < Gamepad.all.Count; i++)
                {
                    Gamepad.all[i].SetMotorSpeeds(0f, 0f);
                }
            }
        }

        public void LowRumble(float intensity, float duration) => StartCoroutine(Rumble(intensity, 0, duration));
        public void HighRumble(float intensity, float duration) => StartCoroutine(Rumble(0, intensity, duration));

        private IEnumerator Rumble(float lowIntensity, float highIntensity, float duration)
        {
            if (Gamepad.current != null)
            {
                Gamepad.current.SetMotorSpeeds(lowIntensity, highIntensity);
                yield return new WaitForSeconds(duration);
                Gamepad.current.ResetHaptics();
                //for (var i = 0; i < Gamepad.all.Count;i++)
                //{
                //    Gamepad.all[i].SetMotorSpeeds(0f, 0f);
                //}                
            }
        }

        public void StartRumble(float lowIntensity, float highIntensity)
        {
            Gamepad.current.SetMotorSpeeds(lowIntensity, highIntensity);
        }

        public void StopRumble()
        {
            Gamepad.current.SetMotorSpeeds(0f, 0f);
        }
    }
}