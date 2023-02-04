using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SpecialButton : Button
    {
        [SerializeField] private TextMeshProUGUI Text;
        private readonly Color32 _normalColor = new Color32(56, 113, 3, 255);
        private readonly Color32 _highlightedColor = new Color32(244, 245, 209, 255);

        protected override void Awake()
        {
            base.Awake();

            Text = GetComponentInChildren<TextMeshProUGUI>();
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            
            if (Text == null) return;
            
            switch (state)
            {
                case SelectionState.Normal:
                case SelectionState.Selected:
                case SelectionState.Disabled:
                    Text.color = _normalColor;
                    break;
                case SelectionState.Highlighted:
                case SelectionState.Pressed:
                    Text.color = _highlightedColor;
                    break;
                default:
                    Text.color = _normalColor;
                    break;
            }
        }
    }
}