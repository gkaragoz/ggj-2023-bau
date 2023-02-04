using System;
using Main_Character;
using UnityEngine;

namespace Gameplay
{
    public class AnimationEventTransmitter : MonoBehaviour
    {
        [SerializeField] private CharacterWeaponController CharacterWeaponController;

        private void OnCompleteSwingAnimation()
        {
            CharacterWeaponController.OnCompleteSwingAnimation();
        }
    }
}