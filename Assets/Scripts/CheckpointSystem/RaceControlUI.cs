using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class RaceControlUI : MonoBehaviour
{
    [SerializeField] private GameObject raceControlInfoTextObject;
    [SerializeField] private GameObject countdownTextObject;
    
    
    private TextMeshProUGUI _countdownText;
    private TextMeshProUGUI _infoText;
    private void Awake()
    {
        _countdownText = countdownTextObject.GetComponent<TextMeshProUGUI>();
        _infoText = raceControlInfoTextObject.GetComponent<TextMeshProUGUI>();
        
        ClearCountdownText();
        DisplayUpdateText("Waiting for players...");
        
        
    }

    public void DisplayUpdateText(string text)
    {
        _infoText.text = text;
        raceControlInfoTextObject.SetActive(true);
    }
    
    public void DisplayUpdateText(string text, int duration)
    {
        _infoText.text = text;
        Invoke(nameof(ClearUpdateText), duration);
        raceControlInfoTextObject.SetActive(true);
    }
    
    public void DisplayCountdownText(string text)
    {
        _countdownText.text = text;
        countdownTextObject.SetActive(true);
    }

    public void DisplayCountdownText(string text, int duration)
    {
        _countdownText.text = text;
        Invoke(nameof(ClearCountdownText), duration);
        countdownTextObject.SetActive(true);
    }
    
    public void ClearCountdownText()
    {
        countdownTextObject.SetActive(false);
        countdownTextObject.GetComponent<TextMeshProUGUI>().text = string.Empty;
    }
    
    public void ClearUpdateText()
    {
        raceControlInfoTextObject.SetActive(false);
        _infoText.text = string.Empty;
    }
   
}
