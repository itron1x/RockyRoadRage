using UnityEngine;

namespace Player{
    public class PrefabReferences : MonoBehaviour{
        [SerializeField] private GameObject character;
        [SerializeField] private GameObject overlays;
        [SerializeField] private GameObject eyes;

        public GameObject GetCharacter(){
            return character;
        }

        public GameObject GetOverlays(){
            return overlays;
        }

        public GameObject GetEye(){
            return eyes;
        }

    
    }
}
