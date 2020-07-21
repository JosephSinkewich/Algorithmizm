using AlgorithmizmModels.Primitives;
using LevelModule;
using UnityEngine;

namespace LevelEditor
{
    public class LevelEditorFieldSelector : MonoBehaviour
    {
        private Int2 _prevSlotCoords = new Int2();
        
        private void Update()
        {
            Vector3 cursorWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Int2 slotCoords = LevelAssistant.PositionToCoords(cursorWorldPosition);

            if (slotCoords != _prevSlotCoords)
            {
                transform.position = LevelAssistant.CoordsToPosition(slotCoords);
                
                _prevSlotCoords = slotCoords;
            }
        }
    }
}