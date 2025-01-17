using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseCharacterController : MonoBehaviour
{
    [SerializeField] private Button nextButton;
    [SerializeField] private List<InputHandler> player;

    void Start(){
        nextButton.interactable = false;
    }
    void FixedUpdate()
    {
        if (EnableNext()){
            print("All players have chosen");
            nextButton.interactable = true;
        }
    }

    private bool EnableNext(){
        if (player == null) return false;
        foreach (InputHandler chooseCharacter in player){
            if (chooseCharacter.GetChooseCharacter().GetIsSelected() == false){
                return false;
            }
        }
        return true;
    }

    public void ResetPlayers(){
        foreach (InputHandler chooseCharacter in player){
            chooseCharacter.ResetInputs();
        } 
    }
}
