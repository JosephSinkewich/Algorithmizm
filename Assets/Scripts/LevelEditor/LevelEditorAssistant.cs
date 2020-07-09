﻿using System.Collections.Generic;
using AlgorithmizmModels.Level;
using AlgorithmizmModels.Primitives;
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
        
        public bool TryCreateLevelObject(Int2 coords, string objectName)
        {
            Slot slot = GetOrCreateSlot(coords);
            List<LevelObject> levelObjects = GetSlotObjects(slot);

            bool hasSlotObject = false;
            foreach (LevelObject itLevelObject in levelObjects)
            {
                if (itLevelObject.name == objectName)
                {
                    hasSlotObject = true;
                    break;
                }
            }

            bool success = false;
            
            if (!hasSlotObject)
            {
                LevelObject levelObject = new LevelObject()
                {
                    name = objectName,
                    coords = coords
                };
                
                CreateLevelObject(levelObject);
                
                success = true;
            }

            return success;
        }

        public void ClearAtPoint(Int2 coords)
        {
            Slot slot = GetOrCreateSlot(coords);

            foreach (LevelObjectComponent itObjectComponent in slot.LevelObjects)
            {
                Destroy(itObjectComponent.gameObject);
            }
            
            slot.LevelObjects.Clear();
        }

        private List<LevelObject> GetSlotObjects(Slot slot)
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