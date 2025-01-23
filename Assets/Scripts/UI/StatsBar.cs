using StarTravellers.Utils;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{
    [SerializeField] private bool _enableChangeEffect;
    [SerializeField] private float _changeEffectDuration = .3f;
    [SerializeField] private float _changeEffectDelay = 2f;

    private Image _background;
    private Image _foreground;

    private void Start()
    {
        _background = transform.Find("bgChange")
                               .GetComponent<Image>();
        _foreground = transform.Find("foreground")
                               .GetComponent<Image>();
    }

    public void SetValuePercent(float value)
    {
        if (_foreground.fillAmount == value)
        {
            return;
        }

        _foreground.fillAmount = value;

        if (_enableChangeEffect)
        {
            StopAllCoroutines();
            StartCoroutine(AnimationHelpers.AnimationSequence(new Func<IEnumerator>[]
            {
                () => AnimationHelpers.Delay(_changeEffectDelay),
                () => AnimationHelpers.SmoothImageFillTransition(_background, _background.fillAmount, value, _changeEffectDuration)
            }));
        }
        else
        {
            _background.fillAmount = value;
        }
    }


}
