using UnityEngine;

namespace UI
{
    [System.Serializable]
    public struct EyePosition
    {
        public RectTransform EyeTransform;
        public Vector2 MinMaxPosX;
        public Vector2 MinMaxPosY;
    }
    
    public class EyesLookToInput : MonoBehaviour
    {
        [SerializeField] private bool FlippedX = false;
        [SerializeField] private bool FlippedY = false;
        [SerializeField] private float WidthPercentage = 1f;
        [SerializeField] private float HeightPercentage = 1f;
        [SerializeField] private EyePosition LeftEye;
        [SerializeField] private EyePosition RightEye;
        
        private void LateUpdate()
        {
            var mousePosition = Input.mousePosition;
            
            var leftEyePositionX = mousePosition.x.Map(0, Screen.width * WidthPercentage, LeftEye.MinMaxPosX.x, LeftEye.MinMaxPosX.y);
            var rightEyePositionX = mousePosition.x.Map(0, Screen.width * WidthPercentage, RightEye.MinMaxPosX.x, RightEye.MinMaxPosX.y);
            
            var leftEyePositionY = mousePosition.y.Map(0, Screen.width * HeightPercentage, LeftEye.MinMaxPosY.x, LeftEye.MinMaxPosY.y);
            var rightEyePositionY = mousePosition.y.Map(0, Screen.width * HeightPercentage, RightEye.MinMaxPosY.x, RightEye.MinMaxPosY.y);

            var flippedMultiplierX = FlippedX ? -1 : 1;
            var flippedMultiplierY = FlippedY ? -1 : 1;
            LeftEye.EyeTransform.anchoredPosition = new Vector2(leftEyePositionX * flippedMultiplierX, leftEyePositionY * flippedMultiplierY);
            RightEye.EyeTransform.anchoredPosition = new Vector2(rightEyePositionX * flippedMultiplierX, rightEyePositionY * flippedMultiplierY);
        }
    }
}