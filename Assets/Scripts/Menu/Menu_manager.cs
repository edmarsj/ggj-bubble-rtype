using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Menu
{
    public class Menu_manager : MonoBehaviour
    {
        #region Main functions

        public void Play_button()
        {
            //Call           
            Load_scene("LevelSelect");
        }

        public void Settings_button()
        {

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
            //Call
            SceneManager.LoadScene(sceneName);
        }

        #endregion
    }
}
