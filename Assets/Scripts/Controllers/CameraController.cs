using UnityEngine;

namespace StrategyGameDemo.Controllers
{
	public class CameraController : MonoBehaviour
	{
        private Camera cameraToDrag;

        private bool isDragging = false;
        private Vector3 initialCameraPos;
        private Vector2 initialMousePos;
        private float scalingFactor;

        void Awake()
        {
            cameraToDrag = GetComponent<Camera>();
        }

        public void StartDragging(Vector2 mousePosition)
        {
            if (cameraToDrag == null) return;

            isDragging = true;
            initialMousePos = mousePosition;
            initialCameraPos = cameraToDrag.transform.position;

            if (cameraToDrag.orthographic)
            {
                scalingFactor = 2f * cameraToDrag.orthographicSize / Screen.height;
            }
        }

        public void StopDragging()
        {
            isDragging = false;
        }

        public void UpdateDrag(Vector2 currentMousePosition)
        {
            if (!isDragging || cameraToDrag == null) return;

            Vector2 delta = currentMousePosition - initialMousePos;

            if (cameraToDrag.orthographic)
            {
                float worldDeltaX = delta.x * scalingFactor;
                float worldDeltaY = delta.y * scalingFactor;
                Vector3 worldDelta = new Vector3(worldDeltaX, worldDeltaY, 0);
                cameraToDrag.transform.position = initialCameraPos - worldDelta;
            }
        }
	}
}