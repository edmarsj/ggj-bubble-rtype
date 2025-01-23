using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace StarTravellers.Utils
{
    public static class AnimationHelpers
    {
        public static WaitForEndOfFrame WaitForEndOfFrame = new();
        public static WaitForFixedUpdate WaitForFixedUpdate = new();
        public static WaitForSeconds WaitFor1Seconds = new(1f);
        public static IEnumerator SmoothLookAt(Transform transform, Transform target, float duration)
        {
            var speed = 1f / duration;
            var lookPos = target.position - transform.position;
            lookPos.y = 0;

            var targetRotation = Quaternion.LookRotation(lookPos);
            var initialRotation = transform.rotation;
            var step = 0f;

            while (step < 1f)
            {
                step += Time.deltaTime * speed;
                transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, step);
                yield return WaitForEndOfFrame;
            }
        }

        public static IEnumerator SmoothCanvasGroupTransition(CanvasGroup canvasGroup, float fromAlpha, float toAlpha, float duration)
        {
            var step = 0f;
            var speed = 1f / duration;

            while (step < 1f)
            {
                step += Time.deltaTime * speed;
                canvasGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, step);
                yield return WaitForEndOfFrame;
            }
        }

        public static IEnumerator SmoothImageFillTransition(Image image, float from, float to, float duration)
        {
            var step = 0f;
            var speed = 1f / duration;

            while (step < 1f)
            {
                step += Time.deltaTime * speed;
                image.fillAmount = Mathf.Lerp(from, to, step);
                yield return WaitForEndOfFrame;
            }
        }

        public static IEnumerator SmoothColorLerp(Action<Color> action, Color fromColor, Color toColor, float duration)
        {
            var step = 0f;
            var speed = 1f / duration;

            while (step < 1f)
            {
                step += Time.deltaTime * speed;
                action(Color.Lerp(fromColor, toColor, step));
                yield return WaitForEndOfFrame;
            }
        }

        public static IEnumerator SmoothLerp(Action<float> action, float from, float to, float duration)
        {
            var step = 0f;
            var speed = 1f / duration;

            while (step < 1f)
            {
                step += Time.deltaTime * speed;
                action(Mathf.Lerp(from, to, step));
                yield return WaitForEndOfFrame;
            }
        }

        public static IEnumerator EvalAnimationCurve(Action<float> action, AnimationCurve animationCurve, float duration)
        {
            var step = 0f;
            var speed = 1f / duration;

            while (step < 1f)
            {
                step += Time.deltaTime * speed;
                
                action(animationCurve.Evaluate(step));
                yield return WaitForEndOfFrame;
            }
        }

        public static IEnumerator SmoothPositionTranslation(Transform transform, Vector3 from, Vector3 to, float duration)
        {
            var step = 0f;
            var speed = 1f / duration;

            while (step < 1f)
            {
                step += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(from, to, step);
                yield return WaitForFixedUpdate;
            }
        }

        public static IEnumerator AnimationSequence(Func<IEnumerator>[] sequence)
        {
            foreach (var item in sequence)
            {
                yield return item();
            }
        }

        public static IEnumerator Delay(float time)
        {
            yield return new WaitForSeconds(time);
        }       
    }
}