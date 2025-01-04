using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characters;
    public int selectedCharacter = 0;

    public void NextCharacter ()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter + 1) % characters.Length;
        characters[selectedCharacter].SetActive(true);
    }

    public void PreviousCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if (selectedCharacter < 0)
        {
            selectedCharacter += characters.Length;

        }
        characters[selectedCharacter].SetActive(true);
    }

    public void SaveSelectedCharacter()
    {
        // Speichert den Index des ausgewählten Charakters
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
        PlayerPrefs.Save();
    }
    private void Start()
    {
        // Automatischer Aufruf von SaveSelectedCharacter() beim Start
        SaveSelectedCharacter();
    }
}
