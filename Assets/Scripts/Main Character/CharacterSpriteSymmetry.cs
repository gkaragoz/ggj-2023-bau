using UnityEngine;

namespace Main_Character
{
    public class CharacterSpriteSymmetry : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private SpriteRenderer character;
        [SerializeField] private Sprite horizontalRenderer;
        [SerializeField] private Sprite upRenderer;
        [SerializeField] private Sprite backRenderer;
        
        public void Update()
        {
            var playerPosition = transform.position;
            var inputPosition = (Vector2)playerCamera.ScreenToWorldPoint(Input.mousePosition);
            var direction = (inputPosition - new Vector2(playerPosition.x, playerPosition.y)).normalized;

            if (direction.y is > -.5F and < .5F && direction.x is >= -1F and < 0F)
            {
                character.sprite = horizontalRenderer;
                character.flipX = true;
            }
            
            else if (direction.y is > -.5F and < .5F && direction.x is > 0 and <= 1F)
            {
                character.sprite = horizontalRenderer;
                character.flipX = false;
            }
            
            else if (direction.x is > -.5F and < 0F && direction.y is > 0 and <= 1F)
            {
                character.sprite = upRenderer;
                character.flipX = false;
            }
            
            else if (direction.x is > 0F and < 0.5F && direction.y is > 0 and <= 1F)
            {
                character.sprite = upRenderer;
                character.flipX = true;
            }
            
            else if (direction.x is > -.5F and < .5F && direction.y is >= -1F and < 0F)
            {
                character.sprite = backRenderer;
                character.flipX = false;
            }
        }
    }
}