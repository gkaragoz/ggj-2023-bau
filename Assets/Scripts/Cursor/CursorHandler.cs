using UnityEngine;

namespace Cursor
{
    public class CursorHandler : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField]
        private TrailRenderer trailRenderer;
        [SerializeField] 
        private Texture2D cursorTexture;

        [Header("Settings")]
        [SerializeField] private float distanceFromCamera = 5; 

        private Camera _camera;
        private int _lineRendererPositionIndex;
        private Vector3 _previousFramePosition;
        private Vector2 _cursorOriginPosition;

        private void Awake()
        {
            _previousFramePosition = Input.mousePosition;
        }

        private void Start()
        {
            _camera = Camera.main;

            // set the cursor origin to its centre. (default is upper left corner)
            _cursorOriginPosition = new Vector2(cursorTexture.width / 2f, cursorTexture.height / 2f);

            SetCursor();
            MoveLineRendererToCursor();
        }

        private void Update()
        {
            MoveLineRendererToCursor();
        }

        private void SetCursor()
        {
            //Sets the cursor to the cursor sprite with given offset 
            //and automatic switching to hardware default if necessary
            UnityEngine.Cursor.SetCursor(cursorTexture, _cursorOriginPosition, CursorMode.Auto);
        }

        private void MoveLineRendererToCursor()
        {
            var mouseCurrentPosition = UnityEngine.Input.mousePosition;
            var direction = (mouseCurrentPosition - _previousFramePosition).normalized;
            _previousFramePosition = mouseCurrentPosition;

            if (direction.magnitude == 0)
                return;

            trailRenderer.transform.position = _camera.ScreenToWorldPoint(
                new Vector3(mouseCurrentPosition.x, mouseCurrentPosition.y, distanceFromCamera));
        }
    }
}