using System.Collections.Generic;
using AlgorithmizmModels.Primitives;
using LevelModule;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LevelEditor
{
    public class LevelEditorDrawController : MonoBehaviour
    {
        private const string NAVIGATION_TOOL_NAME = "_navigation";
        private const string ERASER_TOOL_NAME = "_eraser";
        
        [SerializeField] private ToolsController _toolsController;
        [SerializeField] private LevelEditorAssistant _levelEditorAssistant;

        private readonly bool[] _mouseButtonsDowns = new bool[3];

        private Vector3 _navigationStartPoint;
        private Vector3 _cameraStartPoint;
        private Vector3 _cameraOffset;
        
        private void Update()
        {
            bool isSuccessfulClick = PointerRaycast(Input.mousePosition, out Vector3 worldPosition);
            Int2 slotCoords = LevelAssistant.PositionToCoords(worldPosition);

            for (var i = 0; i < 3; i++)
            {
                if (!Input.GetMouseButtonDown(i)) continue;

                if (isSuccessfulClick)
                {
                    _mouseButtonsDowns[i] = true;

                    if (_toolsController.SelectedTool != null 
                        && _toolsController.SelectedTool.ObjectName == NAVIGATION_TOOL_NAME
                        || i == 2)
                    {
                        _navigationStartPoint = worldPosition;
                        _cameraStartPoint = Camera.main.transform.position;
                        _cameraOffset = Vector3.zero;
                    }
                }
            }

            if (_toolsController.SelectedTool != null)
            {
                if (_mouseButtonsDowns[0])
                {
                    if (_toolsController.SelectedTool.ObjectName == NAVIGATION_TOOL_NAME)
                    {
                        UseNavigation(worldPosition);
                    }
                    else if (_toolsController.SelectedTool.ObjectName == ERASER_TOOL_NAME)
                    {
                        ClearSlot(slotCoords);
                    }
                    else
                    {
                        UseTool(_toolsController.SelectedTool, slotCoords);
                    }
                }

                if (_mouseButtonsDowns[1])
                {
                    EraseObject(_toolsController.SelectedTool, slotCoords);
                }
                
                if (_mouseButtonsDowns[2])
                {
                    UseNavigation(worldPosition);
                }
            }
            
            for (var i = 0; i < 3; i++)
            {
                if (!Input.GetMouseButtonUp(i)) continue;

                _mouseButtonsDowns[i] = false;
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

        private void EraseObject(LevelEditorTool tool, Int2 slotCoords)
        {
            _levelEditorAssistant.RemoveLevelObject(slotCoords, tool.ObjectName);
        }

        private void UseNavigation(Vector3 currentPosition)
        {
            _cameraOffset = _navigationStartPoint - (currentPosition - _cameraOffset);
            Camera.main.transform.position = _cameraStartPoint + _cameraOffset;
        }

        private void ClearSlot(Int2 slotCoords)
        {
            _levelEditorAssistant.ClearAtPoint(slotCoords);
        }
    }
}