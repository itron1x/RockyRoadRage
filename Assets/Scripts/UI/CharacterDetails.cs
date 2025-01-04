using UnityEngine;
using UnityEngine.Serialization;

namespace UI{
    public class CharacterDetails : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private string characterName;
        [SerializeField] private int cost;
        
        [Header("Characteristic")]
        [SerializeField] private string shape;
        [SerializeField] private int speed;
        [SerializeField] private int weight;
        [SerializeField] private int acceleration;
        [SerializeField] private string funFact;

        [SerializeField] private bool bought;

        public string GetCharacterName(){
            return characterName;
        }
        
        public int GetCost(){
            return cost;
        }
        
        public string GetShape(){
            return shape;
        }
        
        public string GetFunFact(){
            return funFact;
        }
        
        public int GetSpeed(){
            return speed;
        }
        
        public int GetWeight(){
            return weight;
        }
        
        public int GetAcceleration(){
            return acceleration;
        }

        public void SetBought(bool buy){
            this.bought = buy;
        }
        
        public bool IsBought(){
            return bought;
        }
        
    }
}
