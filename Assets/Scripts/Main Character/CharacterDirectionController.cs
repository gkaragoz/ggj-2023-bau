using System;
using Samples.Basic.Scripts;
using UnityEngine;

namespace Main_Character
{
    public enum Direction
    {
        Up = 0,
        UpRight,
        UpLeft,
        Left,
        Right,
        Down,
        DownRight,
        DownLeft
    }

    public class CharacterDirectionController : Singleton<CharacterDirectionController>
    {
        public Direction CurrentDirection { get; private set; } = Direction.Right;
        public static Action<Direction> OnChangeDirection;

        private Camera _characterCamera;
        
        private void Awake()
        {
            _characterCamera = Camera.main;
        }

        private void ChangeDirection(Direction direction)
        {
            if(direction == CurrentDirection) return;
            CurrentDirection = direction;
            OnChangeDirection?.Invoke(CurrentDirection);
        }

        public void Update()
        {
            var playerPosition = transform.position;
            var inputPosition = (Vector2)_characterCamera.ScreenToWorldPoint(Input.mousePosition);
            var direction = (inputPosition - new Vector2(playerPosition.x, playerPosition.y)).normalized;

            if (direction.x is > -.25F and < .25F && direction.y is > 0F and < 1F)
            {
                ChangeDirection(Direction.Up);
            }
            
            else if (direction.x is > -1F and < -.25F && direction.y is > .25F and < 1F)
            {
                ChangeDirection(Direction.UpLeft);
            }
            
            else if (direction.x is > .25F and < 1F && direction.y is > .25F and < 1F)
            {
                ChangeDirection(Direction.UpRight);
            }
            
            else if (direction.x is >= -1F and < 0F && direction.y is > -.25F and < .25F)
            {
                ChangeDirection(Direction.Left);
            }
            
            else if (direction.x is > 0F and <= 1F && direction.y is > -.25F and < .25F)
            {
                ChangeDirection(Direction.Right);
            }
            
            else if (direction.x is > -.25F and < .25F && direction.y is > -1F and < 0F)
            {
                ChangeDirection(Direction.Down);
            }
            
            else if (direction.x is > -1F and < -.25F && direction.y is > -1F and < -.25F)
            {
                ChangeDirection(Direction.DownLeft);
            }
            
            else if (direction.x is > .25F and < 1F && direction.y is > -1F and < -.25F)
            {
                ChangeDirection(Direction.DownRight);
            }
            
            // Debug.LogError($"Vector2D: {direction}, Direction: {CurrentDirection}");
        }
    }
}