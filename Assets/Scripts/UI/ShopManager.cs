using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI{
    public class ShopManager : MonoBehaviour{
        [SerializeField] private List<GameObject> characters;
        
        [Header("Text Objects")]
        [SerializeField] private TextMeshProUGUI characterName;
        [SerializeField] private TextMeshProUGUI characterPrice;
        [SerializeField] private TextMeshProUGUI characterDescription;
        
        [Header("Buttons")]
        [SerializeField] private Button buyButton;
        
        private int _activeCharacter;
        

        private void Awake(){
            UpdateCharacter(_activeCharacter);
        }

        public void NextCharacter(){
            if (_activeCharacter + 1 < characters.Count) _activeCharacter++;
            else _activeCharacter = 0;
            UpdateCharacter(_activeCharacter);
        }

        public void PreviousCharacter(){
            if (_activeCharacter - 1 < 0) _activeCharacter = characters.Count - 1;
            else _activeCharacter--;
            UpdateCharacter(_activeCharacter);
        }

        private void UpdateCharacter(int activeCharacter){
            foreach (GameObject character in characters){
                character.SetActive(false);
            }
            characters[activeCharacter].gameObject.SetActive(true);
           
            CharacterDetails characterDetails = characters[activeCharacter].GetComponent<CharacterDetails>();
            characterName.text = characterDetails.GetCharacterName();
            characterPrice.text = characterDetails.GetCost().ToString();
            characterDescription.text = "<b>Shape:</b> " + characterDetails.GetShape() + "\n<b>Speed:</b> " +
                                        characterDetails.GetSpeed() + "/10\n<b>Weight:</b> " + characterDetails.GetWeight() +
                                        "/10\n<b>Acceleration:</b> " + characterDetails.GetAcceleration() +
                                        "/10\n\n<b>Fun fact:</b> " + characterDetails.GetFunFact();
            if (characterDetails.IsBought()) buyButton.interactable = false;
            else buyButton.interactable = true;
        }
    }
}
