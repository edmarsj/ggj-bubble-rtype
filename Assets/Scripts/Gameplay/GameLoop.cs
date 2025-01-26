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
            //_shared.TotalPoints = 0;
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
            Invoke(nameof(OnPlayerDeathDelayed), _gameOverdelaySeconds);
        }

        private void OnPlayerDeathDelayed()
        {
            SceneManager.LoadScene("Gameplay");
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
            // Unlock next level
            PlayerPrefs.SetInt(_shared.CurrentLevel.NextLevel.name, 1);

#if UNITY_EDITOR
            if (_isDebug)
            {
                //Set
                _shared.SetLevel(_shared.CurrentLevel);
                SceneManager.LoadScene("Gameplay");
                return;
            }
#endif

            SceneManager.LoadScene("LevelSelect");
        }

        private void Back_to_menu()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}