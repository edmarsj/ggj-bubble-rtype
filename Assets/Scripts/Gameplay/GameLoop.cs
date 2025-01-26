using StarTravellers.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Gameplay
{
    public class GameLoop : PausableBehaviour
    {
        [SerializeField] private Level _firstLevel;
        [SerializeField] private bool _isDebug;
        [SerializeField] private float _gameOverdelaySeconds = 2f;



        private void Awake()
        {
            //_shared.CurrentLevel = _firstLevel;            
        }


        private void OnEnable()
        {
            //Set
            _shared.Player_touch_worm_hole.AddListener(Change_level);
            _shared.OnPlayerDie.AddListener(OnPlayerDeath);
            _shared.CurrentPointsOnLevel = 0;
        }


        private void OnDisable()
        {
            _shared.Player_touch_worm_hole.RemoveListener(Change_level);
            _shared.OnPlayerDie.RemoveListener(OnPlayerDeath);
        }

        private void OnPlayerDeath()
        {
            Invoke(nameof(OnPlayerDeathDelayed), _gameOverdelaySeconds);
        }

        private void OnPlayerDeathDelayed()
        {
            TransitionController.Instance.TransitionToScene("Gameplay");
            return;
        }

        private void Start()
        {
            _shared.LevelStage = GameplayStage.Spawners;
            _shared.BossDefeated = false;


            var subsceneName = $"Level{_shared.CurrentLevel.name}";

#if UNITY_EDITOR
            if (_isDebug)
            {
                for (var i = 0; i < SceneManager.loadedSceneCount; i++)
                {
                    var scene = SceneManager.GetSceneAt(i);
                    if (scene.name.StartsWith("Level"))
                    {
                        // this avoids trying to load the default first level instead of the level being edited 
                        subsceneName = scene.name;
                        _shared.SetLevel(GameObject.FindFirstObjectByType<LevelController>().Level);
                    }
                }
            }
#endif

            if (!SceneManager.GetSceneByName(subsceneName).isLoaded)
            {
                SceneManager.LoadScene(subsceneName, LoadSceneMode.Additive);
            }

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
            _shared.ConfirmScore();
            // Unlock next level
            PlayerPrefs.SetInt(_shared.CurrentLevel.NextLevel.name, 1);

#if UNITY_EDITOR
            if (_isDebug)
            {
                //Set
                _shared.SetLevel(_shared.CurrentLevel);
                TransitionController.Instance.TransitionToScene("Gameplay",false);
               
                return;
            }
#endif
            TransitionController.Instance.TransitionToScene("LevelSelect", false);         
        }

        private void Back_to_menu()
        {
            TransitionController.Instance.TransitionToScene("LevelSelect");
        }
    }
}