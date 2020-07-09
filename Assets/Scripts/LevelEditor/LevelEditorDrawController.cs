using System.Collections.Generic;
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

        private bool[] _mouseDowns = new bool[3];

        private void Update()
        {
            bool isSuccessfulClick = PointerRaycast(Input.mousePosition, out Vector3 worldPosition);
            Int2 slotCoords = LevelAssistant.PositionToCoords(worldPosition);

            for (var i = 0; i < 3; i++)
            {
                if (!Input.GetMouseButtonDown(i)) continue;
                
                if (isSuccessfulClick)
                {
                    _mouseDowns[i] = true;
                }
            }

            if (_mouseDowns[0])
            {
                if (_toolsController.SelectedTool != null)
                {
                    UseTool(_toolsController.SelectedTool, slotCoords);
                }
            }
            
            if (_mouseDowns[1])
            {
                Erase(slotCoords);
            }

            for (var i = 0; i < 3; i++)
            {
                if (!Input.GetMouseButtonUp(i)) continue;
                
                _mouseDowns[i] = false;
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

        private void UseTool(LevelEditorTool tool, Int2 slotCoords)
        {
            _levelEditorAssistant.TryCreateLevelObject(slotCoords, tool.ObjectName);
        }
        
        private void Erase(Int2 slotCoords)
        {
            _levelEditorAssistant.ClearAtPoint(slotCoords);
        }
    }
}