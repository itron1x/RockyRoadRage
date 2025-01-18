using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class NavigationButtonScripts : MonoBehaviour
    {
        public RaceControlManager raceControlManager;
        
        public void ResumeGame()
        {
            raceControlManager.resumeRace();
        }

        public void OnBackToMainMenu()
        {
            Debug.Log("Loading Main Menu");
            RaceInfoSystem.BackToMainMenu();
        }
    }
    
}
