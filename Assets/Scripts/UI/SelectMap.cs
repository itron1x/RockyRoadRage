using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI{
    public class SelectMap : MonoBehaviour{
        [Header("Maps")] 
        [SerializeField] private List<Sprite> maps;
        [SerializeField] private Image selectedMap;
        
        [Header("Buttons")]
        [SerializeField] private Button nextButton;
        [SerializeField] private Button previousButton;

        private int _currentMap = 0;

        void Awake(){
            if (maps.Count <= 1){
                nextButton.interactable = false; 
                previousButton.interactable = false;
            }
        }
        public void NextMap(){
            if (_currentMap == maps.Count - 1){
                _currentMap = 0;
            }
            else _currentMap++;
            
            RaceInfoSystem.GetInstance().ActiveMapIndex = _currentMap;
            selectedMap.sprite = maps[_currentMap];
        }

        public void PreviousMap(){
            if (_currentMap == 0){
                _currentMap = maps.Count - 1;
            } 
            else _currentMap--;
            
            RaceInfoSystem.GetInstance().ActiveMapIndex = _currentMap;
            selectedMap.sprite = maps[_currentMap];
        }
    }
}
