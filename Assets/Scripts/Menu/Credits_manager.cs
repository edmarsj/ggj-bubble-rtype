using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Menu
{
    public class Credits_manager : MonoBehaviour
    {
        [Header("Components")]
        public CanvasGroup Credits_group;
        public GameObject TCredits;
        public Image IHold_button;

        [SerializeField] private float _button_hold_time;
        private bool _returningToMenu = false;
        private void Start()
        {
            //Set
            Credits_group.alpha = 0f;
            TCredits.SetActive(true);
            IHold_button.fillAmount = 0f;            
        }

        #region Core

        private void Update()
        {
            if (_returningToMenu)
            {
                return;
            }

            //Call
            Inputs();
        }

        private void Inputs()
        {
            

            if (Input.GetButtonDown("Fire1"))
            {
                //Set
                _button_hold_time = 0;
                IHold_button.fillAmount = _button_hold_time;
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                //Set
                _button_hold_time = 0;
                IHold_button.fillAmount = _button_hold_time;
            }

            if (Input.GetButton("Fire1"))
            {
                //Set
                _button_hold_time += Time.deltaTime;
                IHold_button.fillAmount = (_button_hold_time / 2f);

                if (_button_hold_time >= 2f)
                {
                    _returningToMenu = true;
                    //Call
                    Change_scene("Menu");
                }
            }
        }

        #endregion

        #region Main functions

        private void Show_buttons()
        {
            //Set
            Credits_group.alpha = 1.0f;
            TCredits.SetActive(false);
        }

        private void Change_scene(string scene)
        {
            TransitionController.Instance.TransitionToScene(scene);            
        }

        #endregion
    }
}
