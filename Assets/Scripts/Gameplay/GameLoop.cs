using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Game.Gameplay
{
    public class GameLoop : PausableBehaviour
    {
        [SerializeField] private Level _firstLevel;
        [SerializeField] private bool _isDebug;
        private static bool _newGame = true;


        private void Awake()
        {
            if (_newGame)
            {
                _shared.CurrentLevel = _firstLevel;
                _shared.TotalPoints = 0;
                _newGame = false;
            }

            
        }

        private void OnEnable()
        {
            //Set
            _shared.Player_touch_worm_hole.AddListener(Change_level);
            _shared.OnPlayerDie.AddListener(OnPlayerDeath);
        }

        private void OnDisable()
        {
            _shared.Player_touch_worm_hole.RemoveListener(Change_level);
            _shared.OnPlayerDie.RemoveListener(OnPlayerDeath);
        }

        private void OnPlayerDeath()
        {
#if UNITY_EDITOR
            if (_isDebug)
            {
                SceneManager.LoadScene("Gameplay");
                return;
            }
#endif
            SceneManager.LoadScene("GameOver");
        }

        private void Start()
        {
            _shared.LevelStage = GameplayStage.Spawners;
            _shared.CurrentPointsOnLevel = 0;
            _shared.BossDefeated = false;


            var subsceneName = $"Level{_shared.CurrentLevel.name}";

#if UNITY_EDITOR
            for (var i = 0; i < SceneManager.loadedSceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name.StartsWith("Level"))
                {
                    // this avoids trying to load the default first level instead of the level being edited 
                    subsceneName = scene.name;
                }
            }
#endif

            if (!SceneManager.GetSceneByName(subsceneName).isLoaded)
            {
                SceneManager.LoadScene(subsceneName, LoadSceneMode.Additive);
            }


            _shared.FadeInScreen.Invoke();
        }

        protected override void DoUpdate()
        {
            switch (_shared.LevelStage)
            {
                case GameplayStage.Spawners:
                    break;
                case GameplayStage.SpawnBoss:
                    var boss = Instantiate(_shared.CurrentLevel.BossPrefab);
                    boss.transform.position = _shared.BossSpawnPosition.position;
                    _shared.Boss = boss;
                    _shared.LevelStage = GameplayStage.BossPresentation;
                    break;
                case GameplayStage.BossFight:
                    break;
                default:
                    break;
            }

            if (Input.GetButtonDown("Cancel")) { Back_to_menu(); }
        }

        private void Change_level()
        {
            //Set
            _shared.CurrentLevel = _shared.CurrentLevel.NextLevel;
            SceneManager.LoadScene("Gameplay");
        }

        private void Back_to_menu()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}