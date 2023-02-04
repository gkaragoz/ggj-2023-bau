using UnityEngine;

namespace Main_Character
{
    public class CharacterSpriteSymmetry : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer characterRenderer;
        [SerializeField] private Sprite sideDirection;
        [SerializeField] private Sprite upDirection;
        [SerializeField] private Sprite upSideDirection;
        [SerializeField] private Sprite downDirection;
        [SerializeField] private Sprite downSideDirection;

        private void UpdateVisual(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    characterRenderer.sprite = upDirection;
                    characterRenderer.flipX = false;
                    break;
                case Direction.UpRight:
                    characterRenderer.sprite = upSideDirection;
                    characterRenderer.flipX = true;
                    break;
                case Direction.UpLeft:
                    characterRenderer.sprite = upSideDirection;
                    characterRenderer.flipX = false;
                    break;
                case Direction.Left:
                    characterRenderer.sprite = sideDirection;
                    characterRenderer.flipX = true;
                    break;
                case Direction.Right:
                    characterRenderer.sprite = sideDirection;
                    characterRenderer.flipX = false;
                    break;
                case Direction.Down:
                    characterRenderer.sprite = downDirection;
                    characterRenderer.flipX = false;
                    break;
                case Direction.DownRight:
                    characterRenderer.sprite = downSideDirection;
                    characterRenderer.flipX = false;
                    break;
                case Direction.DownLeft:
                    characterRenderer.sprite = downSideDirection;
                    characterRenderer.flipX = true;
                    break;
            }
        }

        private void OnEnable()
        {
            CharacterDirectionController.OnChangeDirection += UpdateVisual;
        }

        private void OnDisable()
        {
            CharacterDirectionController.OnChangeDirection -= UpdateVisual;
        }
    }
}