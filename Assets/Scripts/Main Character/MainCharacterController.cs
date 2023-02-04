using Samples.Basic.Scripts;
using UnityEngine;

namespace Main_Character
{
    public class MainCharacterController : Singleton<MainCharacterController>
    {
        [SerializeField] private Rigidbody2D rigidbody2d;
        [SerializeField] private float speed;
        [SerializeField] private CharacterSpriteSymmetry characterSpriteSymmetry;
        
        private Vector3 _refVel = Vector3.zero;

        private void Update()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");
            var direction = new Vector2(horizontal, vertical);
            Vector2 targetVelocity;
            
            if (Mathf.Abs(horizontal) >= .95F && Mathf.Abs(vertical) >= .95F)
            {
                targetVelocity = direction * speed / 1.5F;
            }

            else
            {
                targetVelocity = direction * speed;
            }

            rigidbody2d.velocity = Vector3.SmoothDamp(rigidbody2d.velocity, targetVelocity, ref _refVel, .05F);
            characterSpriteSymmetry.UpdateVelocity(rigidbody2d.velocity.normalized);
        }
    }
}
