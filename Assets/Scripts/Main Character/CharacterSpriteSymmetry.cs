using UnityEngine;

namespace Main_Character
{
    public class CharacterSpriteSymmetry : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer character;
        [SerializeField] private Sprite horizontalRenderer;
        [SerializeField] private Sprite upRenderer;
        [SerializeField] private Sprite backRenderer;
        
        public void UpdateVelocity(Vector3 velocity)
        {
            if (velocity.x > 0F)
            {
                character.sprite = horizontalRenderer;
                character.flipX = false;
            }
            
            else if (velocity.x < 0F)
            {
                character.sprite = horizontalRenderer;
                character.flipX = true;
            }
            
            else if (velocity.y > 0F)
            {
                character.sprite = upRenderer;
            }
            
            else if (velocity.y < 0F)
            {
                character.sprite = backRenderer;
            }
        }
    }
}