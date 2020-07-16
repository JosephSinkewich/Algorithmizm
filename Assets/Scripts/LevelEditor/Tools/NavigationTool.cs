using UnityEngine;

namespace LevelEditor
{
    public class NavigationTool : BaseLevelEditorTool
    {
        private Vector3 _navigationStartPoint;
        private Vector3 _cameraStartPoint;
        private Vector3 _cameraOffset;
        
        protected override void OnMouseButtonDown(MouseButtonEventData eventData)
        {
            if (IsSelected || eventData.buttonIndex == 2)
            {
                _navigationStartPoint = eventData.position;
                _cameraStartPoint = Camera.main.transform.position;
                _cameraOffset = Vector3.zero;
            }
        }

        protected override void OnMouseButtonPress(MouseButtonEventData eventData)
        {
            if (IsSelected || eventData.buttonIndex == 2)
            {
                DragCamera(eventData.position);
            }
        }

        private void DragCamera(Vector3 currentPosition)
        {
            _cameraOffset = _navigationStartPoint - (currentPosition - _cameraOffset);
            Camera.main.transform.position = _cameraStartPoint + _cameraOffset;
        }
    }
}