using UnityEngine;

namespace Player{
    public class LookFollower : MonoBehaviour
    {
    
        public Transform Target;
        public float TargetHeight;
        public Transform Camera;
    
        [Header("Eye Sprites")]
        [SerializeField] private SpriteRenderer currentEyes;
        [SerializeField] private Sprite[] eyeSprites;
    
        void LateUpdate()
        {
            transform.position = Target.position + new Vector3(0, TargetHeight, 0);
            Vector3 direction = transform.position - Camera.position;
            direction = - direction;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
        }

        public void SetTarget(Transform target){
            Target = target;
        }

        public Sprite GetCharacterEye(string characterName){
            switch (characterName){
                case "Pebble Pete":
                    return eyeSprites[0];
                case "Cubic Chris":
                    return eyeSprites[1];
                case "Triangle Tam":
                    return eyeSprites[2];
                case "Smooth Sally":
                    return eyeSprites[3];
                case "Lava Larry":
                    return eyeSprites[4];
            }
            throw new System.Exception("Unknown character");
        }

        public void SetEyes(string characterName){
            currentEyes.sprite = GetCharacterEye(characterName);
        }
    }
}
