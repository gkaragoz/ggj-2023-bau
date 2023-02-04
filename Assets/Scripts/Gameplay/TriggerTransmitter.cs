using System;
using UnityEngine;

namespace Gameplay
{
    public class TriggerTransmitter : MonoBehaviour
    {
        public Action<Collider2D> OnTriggerEnter { get; set; }

        private void OnTriggerEnter2D(Collider2D col)
        {
            OnTriggerEnter?.Invoke(col);
        }
    }
}