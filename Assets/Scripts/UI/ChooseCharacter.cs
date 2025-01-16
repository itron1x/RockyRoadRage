using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI{
    public class ChooseCharacter : MonoBehaviour
    {
        [SerializeField] private List<GameObject> characters;
        
        [Header("Text Objects")]
        [SerializeField] private TextMeshProUGUI characterName;
    
        
        [Header("Buttons")]
        [SerializeField] private Button confirmButton;
        
        private int _activeCharacter;

        // Set initial character
        private void Awake(){
            UpdateCharacter(_activeCharacter);
            
        }

        // update active character to next in list
        public void NextCharacter(){
            if (_activeCharacter + 1 < characters.Count) _activeCharacter++;
            else _activeCharacter = 0;
            UpdateCharacter(_activeCharacter);
        }

        // update active character to previous in list
        public void PreviousCharacter(){
            if (_activeCharacter - 1 < 0) _activeCharacter = characters.Count - 1;
            else _activeCharacter--;
            UpdateCharacter(_activeCharacter);
        }

        public void Confirm(){
        
        }

        // Update UI elements to character
        private void UpdateCharacter(int activeCharacter){
            foreach (GameObject character in characters){
                character.SetActive(false);
            }
            characters[activeCharacter].gameObject.SetActive(true);
           
            CharacterDetails characterDetails = characters[activeCharacter].GetComponent<CharacterDetails>();
            
            characterName.text = characterDetails.GetCharacterName();
            
        }
        
    }
}

