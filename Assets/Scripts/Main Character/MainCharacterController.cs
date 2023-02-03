using UnityEngine;

namespace Main_Character
{
    public class MainCharacterController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rigidbody2d;
        [SerializeField] private float speed;
        [SerializeField] private CharacterSpriteSymmetry characterSpriteSymmetry; 
        
        private void Update()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");
            
            rigidbody2d.velocity = new Vector2(horizontal, vertical) * speed;
            
            characterSpriteSymmetry.UpdateVelocity(rigidbody2d.velocity.normalized);
        }
    }
}
