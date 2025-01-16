using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Button decreaseButton;
        [SerializeField] private Button increaseButton;
        private int _numberofPlayer = 1;
        public List<GameObject> _canvases;

        private void Awake()
        {
            if (Gamepad.all.Count > 1 && Gamepad.all.Count < 9)
            {
                _numberofPlayer = Gamepad.all.Count;
            }
            else if (Gamepad.all.Count > 9)
            {
                _numberofPlayer = 8;
            }
            text.text = _numberofPlayer.ToString();
        }

        public void AddNumber()
        {
            if (_numberofPlayer < 8)
            {
                _numberofPlayer++;
            }
            text.text = _numberofPlayer.ToString();
            if (_numberofPlayer == 8)
            {
                increaseButton.interactable = false;
            }
            
            increaseButton.interactable = true;
        }

        public void RemoveNumber()
        {
            if (_numberofPlayer > 1)
            {
                _numberofPlayer--;
            }
            text.text = _numberofPlayer.ToString();
        
            if (_numberofPlayer == 1)
            {
                decreaseButton.interactable = false;
            }
            decreaseButton.interactable = true;
        }

        public void switchCanvas()
        {
            text.text = _numberofPlayer.ToString();
            switch (_numberofPlayer)
            {
                case (1):
                    _canvases[0].SetActive(true);
                    break;
                case  (2):
                    _canvases[1].SetActive(true);
                    break;
                case  (3):
                    _canvases[2].SetActive(true);
                    break;
                case  (4):
                    _canvases[3].SetActive(true);
                    break;
                case  (5):
                    _canvases[4].SetActive(true);
                    break;
                case  (6):
                    _canvases[5].SetActive(true);
                    break;
                case  (7):
                    _canvases[6].SetActive(true);
                    break;
                case  (8):
                    _canvases[7].SetActive(true);
                    break;
                case  (9):
                    _canvases[8].SetActive(true);
                    break;
            }
        }

        public int GetNumberOfPlayer()
        {
            return _numberofPlayer;
        }

    }
}
