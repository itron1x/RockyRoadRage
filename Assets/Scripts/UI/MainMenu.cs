using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI{
    public class MainMenu : MonoBehaviour
    {
        // Load race scene
        public void StartGame(){
            switch (RaceInfoSystem.GetInstance().ActiveMapIndex){
                case 0:
                    SceneManager.LoadScene("mapTobi");
                    break;
                case 1:
                    SceneManager.LoadScene("map0");
                    break;
                case 2:
                    SceneManager.LoadScene("map2");
                    break;
                case 3:
                    SceneManager.LoadScene("map1");
                    break;
            }
            SceneManager.UnloadSceneAsync("MainMenu");
        }
        
        // Quit game
        public void QuitGame(){
            SaveSystem.Save();
            Application.Quit();
        }

        public void SelectDifficulty(float speed){
            RaceInfoSystem.GetInstance().SetRaceSpeed(speed);
        }

        public void ChangeResolution(int resolution){
            print("Changed Resolution");
            switch (resolution){
                case 0:
                    Screen.SetResolution(1280, 720, true);
                    break;
                case 1:
                    Screen.SetResolution(1920, 1080, true);
                    break;
                case 2:
                    Screen.SetResolution(2560, 1440, true);
                    break;
                case 3:
                    Screen.SetResolution(3840, 2160, true);
                    break;
                
            }
        }

        public void SetVolume(float volume){
            print("Volume set to: " + volume * 100);
            AudioListener.volume = volume;
        }
    }
}
