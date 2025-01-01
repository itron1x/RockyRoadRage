using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class RaceControlUI : MonoBehaviour
{
    [SerializeField] private GameObject raceControlInfoText;
    [SerializeField] private GameObject countdownText;
    
    private void Awake()
    {
        ClearCountdownText(); 
        DisplayUpdateText("Waiting for players...");
    }

    public void DisplayUpdateText(string text)
    {
        raceControlInfoText.GetComponent<TextMeshProUGUI>().text = text;
        raceControlInfoText.SetActive(true);
    }
    
    public void DisplayUpdateText(string text, int duration)
    {
        raceControlInfoText.GetComponent<TextMeshProUGUI>().text = text;
        Invoke(nameof(ClearUpdateText), duration);
        raceControlInfoText.SetActive(true);
    }
    
    public void DisplayCountdownText(string text)
    {
        countdownText.GetComponent<TextMeshProUGUI>().text = text;
        countdownText.SetActive(true);
    }

    public void DisplayCountdownText(string text, int duration)
    {
        countdownText.GetComponent<TextMeshProUGUI>().text = text;
        Invoke(nameof(ClearCountdownText), duration);
        countdownText.SetActive(true);
    }
    
    public void ClearCountdownText()
    {
        countdownText.SetActive(false);
        countdownText.GetComponent<TextMeshProUGUI>().text = string.Empty;
    }
    
    public void ClearUpdateText()
    {
        raceControlInfoText.SetActive(false);
        raceControlInfoText.GetComponent<TextMeshPro>().text = string.Empty;
    }
   
}
