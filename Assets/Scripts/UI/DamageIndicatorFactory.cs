using Samples.Basic.Scripts;
using UnityEngine;

namespace UI
{
    public class DamageIndicatorFactory : Singleton<DamageIndicatorFactory>
    {
        [SerializeField] private DamageIndicator DamageIndicatorPrefab;
        
        public RectTransform ParentCanvas;

        private DamageIndicator CreateCanvas()
        {
            return Instantiate(DamageIndicatorPrefab);
        }

        public void Create(int damage, Transform targetTransform)
        {
            var damageIndicator = CreateCanvas();
            damageIndicator.transform.SetParent(ParentCanvas);
            damageIndicator.Animate(damage.ToString(), targetTransform, ParentCanvas);
        }
    }
}