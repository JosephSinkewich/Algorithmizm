using System.Collections.Generic;
using AlgorithmizmModels.Level;
using AlgorithmizmModels.Primitives;
using LevelModule;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LevelEditor
{
    public class LevelEditorDrawController : MonoBehaviour
    {
        [SerializeField] private ToolsController _toolsController;
        [SerializeField] private LevelEditorAssistant _levelEditorAssistant;

        private bool _isMouseDown;

        private void Update()
        {
            bool isSuccessfulClick = PointerRaycast(Input.mousePosition, out Vector3 worldPosition);
            
            if (Input.GetMouseButtonDown(0))
            {
                if (isSuccessfulClick)
                {
                    _isMouseDown = true;
                }
            }

            if (_isMouseDown)
            {
                if (_toolsController.SelectedTool != null)
                {
                    UseTool(_toolsController.SelectedTool, worldPosition);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isMouseDown = false;
            }
        }

        private bool PointerRaycast(Vector2 screenPosition, out Vector3 worldPosition)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            List<RaycastResult> resultsData = new List<RaycastResult>();
            pointerData.position = screenPosition;
            EventSystem.current.RaycastAll(pointerData, resultsData);

            worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            
            if (resultsData.Count > 0)
            {
                if (resultsData[0].gameObject.GetComponent<UIBehaviour>())
                {
                    return false;
                }
            }

            return true;
        }

        private void UseTool(LevelEditorTool tool, Vector3 position)
        {
            Int2 slotCoords = LevelAssistant.PositionToCoords(position);
            Slot slot = _levelEditorAssistant.GetOrCreateSlot(slotCoords);
            List<LevelObject> levelObjects = _levelEditorAssistant.GetSlotObjects(slot);

            Debug.Log(slot.Coords.ToString());
            
            foreach (LevelObject itLevelObject in levelObjects)
            {
                
            }
        }
    }
}