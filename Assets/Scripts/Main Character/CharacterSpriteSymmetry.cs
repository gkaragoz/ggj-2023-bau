using UnityEngine;

namespace Main_Character
{
    public class CharacterSpriteSymmetry : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        public void UpdateVelocity(Vector3 velocity)
        {
            spriteRenderer.flipX = velocity.x < 0;
        }
    }
}