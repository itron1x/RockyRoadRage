using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreenMenu : MonoBehaviour
{

    public GameObject[] characters;

    /*
    private void Start()
    {
        // Lädt den gespeicherten Charakter
        int selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0); // Standard ist 0
        foreach (GameObject character in characters)
        {
            character.SetActive(false);
        }
        characters[selectedCharacter].SetActive(true);
    }
    */

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void ShopScene()
    {
        SceneManager.LoadScene("ShopMenu");

    }

    public void QuitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }

}
