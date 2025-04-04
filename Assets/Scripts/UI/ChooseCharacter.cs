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
        [SerializeField] private TMP_InputField input;
        
        [Header("Buttons")]
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button previousButton;
        
        private int _isBoughtCharacter;
        private List<GameObject> boughtCharacters = new List<GameObject>(); // new List 
        private RaceInfoSystem _raceInfoSystem;
        private List<int> _characterIDs = new List<int>();
        
        private bool _isSelected;
        
        // Set initial character
        private void Awake()
        {

            boughtCharacters = new List<GameObject>();
            _raceInfoSystem = RaceInfoSystem.GetInstance();

            UpdateBoughtCharacters();

            UpdateArrows();
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
             // _raceInfoSystem.AddCharacter(_isBoughtCharacter);
             _raceInfoSystem.AddCharacter(_characterIDs[_isBoughtCharacter]);
             _raceInfoSystem.AddName(playerName.text);
             _isSelected = true;
             nextButton.interactable = false;
             previousButton.interactable = false;
        }

        // Update UI elements to character
        private void UpdateCharacter(int activeCharacter)
        {
            print(_isBoughtCharacter);
            if (boughtCharacters.Count == 1) return;
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

        public void ResetCharacter(){
            confirmButton.interactable = true;
            nextButton.interactable = true;
            previousButton.interactable = true;
        }

        public void UpdateBoughtCharacters(){
            if(boughtCharacters != null) boughtCharacters.Clear();;
            
            foreach (GameObject character in characters){
                CharacterDetails checkCharacter = character.GetComponent<CharacterDetails>();
                bool isBought = RaceInfoSystem.GetInstance().IsBought(checkCharacter.GetCharacterName());

                if (isBought){
                    boughtCharacters.Add(character);
                    _characterIDs.Add(characters.IndexOf(character));
                }
                // call after check IsBought
                UpdateCharacter(0);
                _isSelected = false;
            }
        }

        public void UpdateArrows(){
            if (boughtCharacters?.Count == 1){
                nextButton.interactable = false;
                previousButton.interactable = false;
            } ;
        }

        public void RemoveSpaces(){
            input.text = input.text.Replace(" ", "");
        }
    }
}

