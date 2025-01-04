using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI{
    public class MainMenu : MonoBehaviour
    {
        public void StartGame(){
            SceneManager.LoadScene("SampleRaceScene");
        }
        public void QuitGame(){
            Application.Quit();
        }
    }
}
