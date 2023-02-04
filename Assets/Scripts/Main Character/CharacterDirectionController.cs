using UnityEngine;

namespace Main_Character
{
    public class CharacterDirectionController : MonoBehaviour
    {
        public Direction CurrentDirection { get; private set; } = Direction.Right;
        private Camera _characterCamera;

        private void Awake()
        {
            _characterCamera = Camera.main;
        }

        public void Update()
        {
            var playerPosition = transform.position;
            var inputPosition = (Vector2)_characterCamera.ScreenToWorldPoint(Input.mousePosition);
            var direction = (inputPosition - new Vector2(playerPosition.x, playerPosition.y)).normalized;

            if (direction.x is > -.25F and < .25F && direction.y is > 0F and < 1F)
            {
                CurrentDirection = Direction.Up;
            }
            
            else if (direction.x is > -1F and < -.25F && direction.y is > .25F and < 1F)
            {
                CurrentDirection = Direction.UpLeft;
            }
            
            else if (direction.x is > .25F and < 1F && direction.y is > .25F and < 1F)
            {
                CurrentDirection = Direction.UpRight;
            }
            
            else if (direction.x is > -1F and < 0F && direction.y is > -.25F and < .25F)
            {
                CurrentDirection = Direction.Left;
            }
            
            else if (direction.x is > 0F and < 1F && direction.y is > -.25F and < .25F)
            {
                CurrentDirection = Direction.Right;
            }
            
            else if (direction.x is > -.25F and < .25F && direction.y is > -1F and < 0F)
            {
                CurrentDirection = Direction.Down;
            }
            
            else if (direction.x is > -1F and < -.25F && direction.y is > -1F and < -.25F)
            {
                CurrentDirection = Direction.DownLeft;
            }
            
            else if (direction.x is > .25F and < 1F && direction.y is > -1F and < -.25F)
            {
                CurrentDirection = Direction.DownRight;
            }
        }
    }
}