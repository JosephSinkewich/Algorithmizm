using UnityEngine;

namespace LevelEditor
{
    [RequireComponent(typeof(Camera))]
    public class CameraZoomController : MonoBehaviour
    {
        [SerializeField] private float _minZoom;
        [SerializeField] private float _maxZoom;
        [SerializeField] private float _deltaZoomScale;

        private Camera _camera;

        private void Start()
        {
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            float orthographicSize = _camera.orthographicSize - Input.mouseScrollDelta.y * _deltaZoomScale;

            _camera.orthographicSize = Mathf.Clamp(orthographicSize, _minZoom, _maxZoom);
        }
    }
}