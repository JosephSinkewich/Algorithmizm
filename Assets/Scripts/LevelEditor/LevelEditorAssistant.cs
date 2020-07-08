using System.Collections.Generic;
using AlgorithmizmModels.Level;
using LevelModule;

namespace LevelEditor
{
    public class LevelEditorAssistant : LevelAssistant
    {
        public Level GenerateLevel(string levelName)
        {
            Level result = new Level();
            result.name = levelName;
            result.levelObjects = new List<LevelObject>();

            foreach (Slot itSlot in _slots.Values)
            {
                result.levelObjects.AddRange(GetSlotObjects(itSlot));
            }
            
            return result;
        }

        public List<LevelObject> GetSlotObjects(Slot slot)
        {
            List<LevelObject> result = new List<LevelObject>();
            
            foreach (LevelObjectComponent itComponent in slot.LevelObjects)
            {
                LevelObject levelObject = new LevelObject();
                levelObject.coords = slot.Coords.Clone();
                levelObject.name = itComponent.Name;
            }

            return result;
        }
    }
}