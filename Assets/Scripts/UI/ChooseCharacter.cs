using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI{
    public class ChooseCharacter : MonoBehaviour
    {
        [SerializeField] private List<GameObject> characters;
        [FormerlySerializedAs("characterName")]
        [Header("Text Objects")]
        [SerializeField] private TextMeshProUGUI playerName;
        
        [Header("Buttons")]
        [SerializeField] private Button confirmButton;
        
        private int _isBoughtCharacter;
        private List<GameObject> boughtCharacters; // new List 
        private RaceInfoSystem _raceInfoSystem;
        
        private bool _isSelected;
        
        // Set initial character
        private void Awake()
        {

            boughtCharacters = new List<GameObject>();
            _raceInfoSystem = RaceInfoSystem.GetInstance();

            foreach (GameObject character in characters)
            {
                CharacterDetails checkCharacter = character.GetComponent<CharacterDetails>();
                bool isBought = _raceInfoSystem.IsBought(checkCharacter.GetCharacterName());

                if (isBought)
                {
                    boughtCharacters.Add(character);
                }
                // call after check IsBought
                UpdateCharacter(0);
                _isSelected = false;
            }
        }

        // update active character to next in list
        public void NextCharacter(){
            if (_isBoughtCharacter + 1 < boughtCharacters.Count) _isBoughtCharacter++; // just go throught boughtCharacters
            else _isBoughtCharacter = 0;
            UpdateCharacter(_isBoughtCharacter);
        }

        // update active character to previous in list
        public void PreviousCharacter(){
            if (_isBoughtCharacter - 1 < 0) _isBoughtCharacter = boughtCharacters.Count - 1;
            else _isBoughtCharacter--;
            UpdateCharacter(_isBoughtCharacter);
        }

        public void Confirm(){
             _raceInfoSystem.AddCharacter(_isBoughtCharacter);
             _raceInfoSystem.AddName(playerName.text);
             _isSelected = true;
        }

        // Update UI elements to character
        private void UpdateCharacter(int activeCharacter)
        {
            print(_isBoughtCharacter);
            foreach (GameObject character in boughtCharacters)
            {
                character.SetActive(false);
            }

            boughtCharacters[activeCharacter].gameObject.SetActive(true);
        }

        public bool GetIsSelected(){
            return _isSelected;
        }

        public void SetPlayerName(string newName){
            playerName.text = newName;
        }
    }
}

