using AlgorithmizmModels.Primitives;
using UnityEngine;

namespace LevelEditor
{
    public class EraserTool : BaseLevelEditorTool
    {
        [SerializeField] private LevelEditorAssistant _levelEditorAssistant;

        protected override void OnMouseButtonPress(MouseButtonEventData eventData)
        {
            if (IsSelected && eventData.buttonIndex == 0)
            {
                ClearSlot(eventData.coords);
            }
        }

        private void ClearSlot(Int2 slotCoords)
        {
            _levelEditorAssistant.ClearAtPoint(slotCoords);
        }
    }
}