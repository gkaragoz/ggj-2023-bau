using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enemy;
using UnityEngine;
using UnityEngine.Rendering;

namespace Main_Character
{
    [Serializable]
    public class WeaponDirectionData
    {
        public string title;
        public Direction direction;
        public int sortOrder;
        public Vector3 localPosition;
        public Vector3 localRotation;
        public bool flipX;
        public bool flipY;
    }
    
    public class CharacterWeaponController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer weaponRenderer;
        [SerializeField] private SortingGroup sortingGroup;
        [SerializeField] private Animator weaponAnimator;
        [SerializeField] private Transform weaponChildSocket;
        [SerializeField] private List<WeaponDirectionData> directionDataset;

        public bool IsSwingAnimationPlaying { get; private set; }
        
        private Tween _rotationAnimation, _moveAnimation;
        
        private void UpdateWeaponChildSocket(Direction direction)
        {
            _rotationAnimation?.Kill();
            _moveAnimation?.Kill();
            
            var data = directionDataset.FirstOrDefault(socketData => socketData.direction == direction);
            sortingGroup.sortingOrder = data.sortOrder;
            weaponRenderer.flipX = data.flipX;
            weaponRenderer.flipY = data.flipY;
            _moveAnimation = weaponChildSocket.DOLocalMove(data.localPosition, .2F);
            _rotationAnimation = weaponChildSocket.DOLocalRotate(data.localRotation, .2F);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out EnemyController enemy))
            {
                if (IsSwingAnimationPlaying)
                {
                    enemy.TakeHit(transform.position);
                }
            }
        }

        public void SwingWeapon()
        {
            weaponAnimator.Play("Swing");
            IsSwingAnimationPlaying = true;
        }

        public void OnCompleteSwingAnimation()
        {
            IsSwingAnimationPlaying = false;
        }

        private void OnEnable()
        {
            CharacterDirectionController.OnChangeDirection += UpdateWeaponChildSocket;
        }

        private void OnDisable()
        {
            CharacterDirectionController.OnChangeDirection -= UpdateWeaponChildSocket;
        }
    }
}