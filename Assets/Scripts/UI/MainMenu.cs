using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI{
    public class MainMenu : MonoBehaviour
    {
        // Load race scene
        public void StartGame(){
            SceneManager.LoadScene("SampleRaceScene");
        }
        
        // Quit game
        public void QuitGame(){
            SaveSystem.Save();
            Application.Quit();
        }
    }
}
