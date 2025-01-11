using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace UI{
    public class ShopManager : MonoBehaviour{
        [SerializeField] private List<GameObject> characters;
        
        [Header("Text Objects")]
        [SerializeField] private TextMeshProUGUI characterName;
        [SerializeField] private TextMeshProUGUI characterPrice;
        [SerializeField] private TextMeshProUGUI characterDescription;
        [SerializeField] private TextMeshProUGUI globalCoins;
        [SerializeField] private Canvas error;
        [SerializeField] private Canvas information;
        
        [Header("Buttons")]
        [SerializeField] private Button buyButton;
        
        private int _activeCharacter;
        private int _coins;

        // Set initial character
        private void Awake(){
            UpdateCharacter(_activeCharacter);
            _coins = RaceInfoSystem.GetInstance().GetGlobalCoins();
            globalCoins.text = _coins.ToString();
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

        public void BuyCharacter(){
            CharacterDetails characterDetails = characters[_activeCharacter].GetComponent<CharacterDetails>();
            RaceInfoSystem raceInfoSystem = RaceInfoSystem.GetInstance();

            // Ckeck if enough coins are available
            if (raceInfoSystem.GetGlobalCoins() < characterDetails.GetCost()){
                error.gameObject.SetActive(true);
                print("Not enough money");
                return;
            }
            
            // Buy character
            raceInfoSystem.BuyCharacter(characterDetails.GetCharacterName(), characterDetails.GetCost());
            buyButton.interactable = false;
            information.gameObject.SetActive(true);
            
            // Update coins
            globalCoins.text = raceInfoSystem.GetGlobalCoins().ToString();
            SaveSystem.Save();
        }

        // Update UI elements to character
        private void UpdateCharacter(int activeCharacter){
            foreach (GameObject character in characters){
                character.SetActive(false);
            }
            characters[activeCharacter].gameObject.SetActive(true);
           
            CharacterDetails characterDetails = characters[activeCharacter].GetComponent<CharacterDetails>();
            RaceInfoSystem raceInfoSystem = RaceInfoSystem.GetInstance();
            
            characterName.text = characterDetails.GetCharacterName();
            characterPrice.text = characterDetails.GetCost().ToString();

            if (!raceInfoSystem.IsBought(characterDetails.GetCharacterName()) && raceInfoSystem.GetGlobalCoins() < characterDetails.GetCost()){
                characterPrice.color = Color.red;
            }
            else characterPrice.color = Color.white;
            
            characterDescription.text = "<b>Shape:</b> " + characterDetails.GetShape() + "\n<b>Speed:</b> " +
                                        characterDetails.GetSpeed() + "/10\n<b>Weight:</b> " + characterDetails.GetWeight() +
                                        "/10\n<b>Acceleration:</b> " + characterDetails.GetAcceleration() +
                                        "/10\n\n<b>Fun fact:</b> " + characterDetails.GetFunFact();
            if (raceInfoSystem.IsBought(characterDetails.GetCharacterName())) buyButton.interactable = false;
            else buyButton.interactable = true;
        }

        public List<GameObject> GetAvailableCharacters()
        {
            return characters;
        }
        
        public void Save(){
            SaveSystem.Save();
        }
    }
}
