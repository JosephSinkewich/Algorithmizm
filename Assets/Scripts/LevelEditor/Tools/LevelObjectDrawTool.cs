using AlgorithmizmModels.Primitives;
using UnityEngine;

namespace LevelEditor
{
    public class LevelObjectDrawTool : BaseLevelEditorTool
    {
        [SerializeField] private string _objectName;
        [SerializeField] private LevelEditorAssistant _levelEditorAssistant;

        protected override void OnMouseButtonPress(MouseButtonEventData eventData)
        {
            if (!IsSelected) return;
            
            if (eventData.buttonIndex == 0)
            {
                DrawObject(eventData.coords);
            }
            else if (eventData.buttonIndex == 1)
            {
                EraseObject(eventData.coords);
            }
        }

        private void DrawObject(Int2 slotCoords)
        {
            _levelEditorAssistant.TryCreateLevelObject(slotCoords, _objectName);
        }

        private void EraseObject(Int2 slotCoords)
        {
            _levelEditorAssistant.RemoveLevelObject(slotCoords, _objectName);
        }
    }
}