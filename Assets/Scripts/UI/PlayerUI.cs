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
        }

        public int GetNumberOfPlayer()
        {
            return _numberofPlayer;
        }
    

    }
}
