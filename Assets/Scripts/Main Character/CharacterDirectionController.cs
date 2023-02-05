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
        public Vector2 CurrentDirectionVector { get; private set; }
        
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
            if(MainCharacterController.Instance.CurrentState == CharacterState.Death) return;
            var playerPosition = transform.position;
            var inputPosition = (Vector2)_characterCamera.ScreenToWorldPoint(Input.mousePosition);
            CurrentDirectionVector = (inputPosition - new Vector2(playerPosition.x, playerPosition.y)).normalized;
            
            if (CurrentDirectionVector.x is > -.25F and < .25F && CurrentDirectionVector.y is > 0F and < 1F)
            {
                ChangeDirection(Direction.Up);
            }
            
            else if (CurrentDirectionVector.x is > -1F and < -.25F && CurrentDirectionVector.y is > .25F and < 1F)
            {
                ChangeDirection(Direction.UpLeft);
            }
            
            else if (CurrentDirectionVector.x is > .25F and < 1F && CurrentDirectionVector.y is > .25F and < 1F)
            {
                ChangeDirection(Direction.UpRight);
            }
            
            else if (CurrentDirectionVector.x is >= -1F and < 0F && CurrentDirectionVector.y is > -.25F and < .25F)
            {
                ChangeDirection(Direction.Left);
            }
            
            else if (CurrentDirectionVector.x is > 0F and <= 1F && CurrentDirectionVector.y is > -.25F and < .25F)
            {
                ChangeDirection(Direction.Right);
            }
            
            else if (CurrentDirectionVector.x is > -.25F and < .25F && CurrentDirectionVector.y is > -1F and < 0F)
            {
                ChangeDirection(Direction.Down);
            }
            
            else if (CurrentDirectionVector.x is > -1F and < -.25F && CurrentDirectionVector.y is > -1F and < -.25F)
            {
                ChangeDirection(Direction.DownLeft);
            }
            
            else if (CurrentDirectionVector.x is > .25F and < 1F && CurrentDirectionVector.y is > -1F and < -.25F)
            {
                ChangeDirection(Direction.DownRight);
            }
            
            // Debug.LogError($"Vector2D: {direction}, Direction: {CurrentDirection}");
        }
    }
}