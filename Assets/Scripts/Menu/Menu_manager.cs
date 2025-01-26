using StarTravellers.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Menu
{
    public class Menu_manager : MonoBehaviour
    {
        [SerializeField] private Shared _shared;
        

        #region Main functions

        public void Play_button()
        {
            //Call                     
            Load_scene("LevelSelect");
        }

        public void Settings_button()
        {
            Load_scene("Settings");
        }

        public void Credits_button()
        {
            //Call
            Load_scene("Credits");
        }

        public void Quit_button()
        {
            Application.Quit();
        }

        #endregion

        #region Core

        private void Load_scene(string sceneName)
        {
            TransitionController.Instance.TransitionToScene(sceneName);
        }


        #endregion
    }
}
