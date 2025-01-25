using StarTravellers.Utils;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private Shared _shared;
        [SerializeField] private TMP_Text _txtLevelName;
        [SerializeField] private TMP_Text _txtPoints;
        [SerializeField] private TMP_Text _txtLife;
        [SerializeField] private CanvasGroup _cgBoss;
        [SerializeField] private TMP_Text _txtBossName;
        [SerializeField] private Image _imgBossGauge;
        [SerializeField] private CanvasGroup _cgAlert;

        [Header("Transition")]
        [SerializeField] private Image[] _transitionImages;


        private void Awake()
        {            
            _txtLevelName.text = _shared.CurrentLevel.LevelName;
            _txtPoints.text = _shared.TotalPoints.ToString();
            _cgBoss.alpha = 0f;
            _cgAlert.alpha = 0f;

            foreach (var image in _transitionImages)
            {
                image.fillAmount = 1f;
            }
        }

        private void OnEnable()
        {
            _shared.OnBeginBossBattle.AddListener(OnBeginBossBattle);
            _shared.OnEndBossBattle.AddListener(OnEndBossBattle);
            _shared.FadeInScreen.AddListener(OnFadeInScreen);
            _shared.FadeOutScreen.AddListener(OnFadeOutScreen);
        }

        private void OnDisable()
        {
            _shared.OnBeginBossBattle.RemoveListener(OnBeginBossBattle);
            _shared.OnEndBossBattle.RemoveListener(OnEndBossBattle);
            _shared.FadeInScreen.RemoveListener(OnFadeInScreen);
            _shared.FadeOutScreen.RemoveListener(OnFadeOutScreen);
        }

        private void OnFadeOutScreen()
        {
            StartCoroutine(AnimationHelpers.SmoothLerp(f =>
            {
                foreach (var image in _transitionImages)
                {
                    image.fillAmount = f;
                }
            }, 0f, 1f, 1f));
        }

        private void OnFadeInScreen()
        {
            StartCoroutine(AnimationHelpers.SmoothLerp(f =>
            {
                foreach (var image in _transitionImages)
                {
                    image.fillAmount = f;
                }
            }, 1f, 0f, 1f));
        }

        #region Core

        private void OnBeginBossBattle()
        {
            //Set
            _cgBoss.alpha = 1f;
            _txtBossName.text = _shared.Boss.DisplayName;
            _cgAlert.alpha = 0f;
        }

        private void OnEndBossBattle()
        {
            //Set
            _cgBoss.alpha = 0f;
        }

        #endregion

        private void Update()
        {
            _txtPoints.text = _shared.TotalPoints.ToString();
            if (_shared.Player)
            {
                _txtLife.text = _shared.Player.Life.ToString();
            }

            if (_shared.Boss)
            {
                _imgBossGauge.fillAmount = _shared.Boss.LifePercent;
            }

            if (_shared.LevelStage == GameplayStage.BossPresentation)
            {
                _cgAlert.alpha = Mathf.PingPong(Time.timeSinceLevelLoad * 3f, 1f);
            }

        }
    }
}