using System;
using System.Collections.Generic;
using AlgorithmizmModels.Primitives;
using LevelModule;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LevelEditor
{
    public class LevelEditorDrawController : MonoBehaviour
    {
        private readonly bool[] _mouseButtonsDowns = new bool[3];
        
        public event Action<MouseButtonEventData> OnMouseButtonDown;
        public event Action<MouseButtonEventData> OnMouseButtonPress;
        public event Action<MouseButtonEventData> OnMouseButtonUp;
        
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

                    InvokeMouseButtonDown(i, worldPosition, slotCoords);
                }
            }

            for (var i = 0; i < 3; i++)
            {
                if (_mouseButtonsDowns[i])
                {
                    InvokeMouseButtonPress(i, worldPosition, slotCoords);
                }
            }
            
            for (var i = 0; i < 3; i++)
            {
                if (!Input.GetMouseButtonUp(i)) continue;

                _mouseButtonsDowns[i] = false;
                InvokeMouseButtonUp(i, worldPosition, slotCoords);
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

        private void InvokeMouseButtonDown(int index, Vector3 wlorldPosition, Int2 slotCoords)
        {
            OnMouseButtonDown?.Invoke(new MouseButtonEventData
            {
                buttonIndex = index,
                position = wlorldPosition,
                coords = slotCoords
            });
        }

        private void InvokeMouseButtonPress(int index, Vector3 wlorldPosition, Int2 slotCoords)
        {
            OnMouseButtonPress?.Invoke(new MouseButtonEventData
            {
                buttonIndex = index,
                position = wlorldPosition,
                coords = slotCoords
            });
        }

        private void InvokeMouseButtonUp(int index, Vector3 wlorldPosition, Int2 slotCoords)
        {
            OnMouseButtonUp?.Invoke(new MouseButtonEventData
            {
                buttonIndex = index,
                position = wlorldPosition,
                coords = slotCoords
            });
        }
    }
}