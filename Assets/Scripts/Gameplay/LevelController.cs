using Game.Haptics;
using StarTravellers.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace Game.Gameplay
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private Shared _shared;
        [SerializeField] private Renderer _bgRenderer;
        [SerializeField] private Transform _mainContent;
        [SerializeField] private Transform _bossSpawnPosition;
        [SerializeField] private GameObject _wormhole;
        [SerializeField] private Transform _wormholeEntryPoint;

        private Material _bgMaterial;
        private void Start()
        {
            _bgMaterial = _bgRenderer.material;
            _shared.BossSpawnPosition = _bossSpawnPosition;
            _shared.OnBeginBossBattle.AddListener(OnBeginBossBattle);
            _shared.OnEndBossBattle.AddListener(OnEndBossBattle);
            _wormhole.SetActive(false);
        }

        private void OnBeginBossBattle()
        {
            HapticsSystem.Instance.StopRumble();
        }

        private void OnEndBossBattle()
        {
            _shared.LevelStage = GameplayStage.Wormhole;
            _wormhole.SetActive(true);

            StartCoroutine(AnimationHelpers.AnimationSequence(new Func<IEnumerator>[]
           {
               () => AnimationHelpers.Action(() => HapticsSystem.Instance.StartRumble(.1f,.1f)),
               () => AnimationHelpers.SmoothPositionTranslation(_shared.Player.transform,_shared.Player.transform.position,_wormholeEntryPoint.position,1f),
               () => AnimationHelpers.Action(() => HapticsSystem.Instance.StartRumble(.5f,.5f)),
               () => AnimationHelpers.Delay(.5f),
               () => AnimationHelpers.SmoothPositionTranslation(_shared.Player.transform,_shared.Player.transform.position,_wormhole.transform.position,.5f),
               () => AnimationHelpers.Action(() => HapticsSystem.Instance.StopRumble()),
               () => AnimationHelpers.Action(() => _shared.FadeOutScreen.Invoke()),
               () => AnimationHelpers.Delay(1f),
               () => AnimationHelpers.Action(() => _shared.Player_touch_worm_hole?.Invoke())
           }));
        }

        private void Update()
        {
            _bgMaterial.SetFloat("_Time_Scale", .2f * _shared.CurrentLevel.LevelScrollSpeed);
            _mainContent.Translate(Vector2.left * _shared.CurrentLevel.LevelScrollSpeed * Time.deltaTime);
        }

        public void StartBossBattle()
        {
            _shared.LevelStage = GameplayStage.SpawnBoss;
            HapticsSystem.Instance.StartRumble(.3f,.3f);
        }
    }
}