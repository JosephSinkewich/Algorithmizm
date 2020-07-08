using System;
using AlgorithmizmModels.Level;
using AlgorithmizmModels.Primitives;
using System.Collections.Generic;
using UnityEngine;

namespace LevelModule
{
    public class LevelAssistant : MonoBehaviour
    {
        public const float SLOT_SIZE = 1f;

        [SerializeField] protected SimulationResourceProvider _resourceProvider;
        
        [SerializeField] protected Transform _levelField;

        protected readonly Dictionary<Int2, Slot> _slots = new Dictionary<Int2, Slot>();

        public Level LevelDesign { get; protected set; }

        public static Vector3 CoordsToPosition(Int2 coords)
        {
            Vector3 result = new Vector3(SLOT_SIZE / 2f, SLOT_SIZE / 2f);

            result.x += coords.x * SLOT_SIZE;
            result.y += coords.y * SLOT_SIZE;

            return result;
        }

        public static Int2 PositionToCoords(Vector3 position)
        {
            position += new Vector3(SLOT_SIZE / 2, SLOT_SIZE / 2, 0);
            
            int x = (int) Mathf.Round(position.x / SLOT_SIZE);
            int y = (int) Mathf.Round(position.y / SLOT_SIZE);

            return new Int2(x, y);
        }

        public void DestroyCurrentLevel()
        {
            foreach (Slot itSlot in _slots.Values)
            {
                foreach (LevelObjectComponent itComponent in itSlot.LevelObjects)
                {
                    Destroy(itComponent.gameObject);
                }
            }
            
            _slots.Clear();
        }

        public void InstantiateLevel(Level levelDesign)
        {
            DestroyCurrentLevel();
            
            LevelDesign = levelDesign;
            
            foreach (LevelObject itObject in LevelDesign.levelObjects)
            {
                CreateLevelObject(itObject);
            }
        }

        public Slot GetOrCreateSlot(Int2 coords)
        {
            Slot slot;
            
            if (_slots.ContainsKey(coords))
            {
                slot = _slots[coords];
            }
            else
            {
                slot = new Slot
                {
                    Coords = coords.Clone(),
                };
                _slots.Add(coords, slot);
            }

            return slot;
        }

        protected virtual void Start()
        {
            if (LevelDesign == null)
            {
                LevelDesign = new Level
                {
                    levelObjects = new List<LevelObject>()
                };
            }

            InstantiateLevel(LevelDesign);
        }

        private void CreateLevelObject(LevelObject levelObject)
        {
            Slot slot = GetOrCreateSlot(levelObject.coords);

            LevelObjectComponent levelObjectComponent =
                Instantiate(_resourceProvider.LevelObjects[levelObject.name], _levelField);
            levelObjectComponent.Initialize(levelObject);
            
            slot.LevelObjects.Add(levelObjectComponent);
        }
    }
}
