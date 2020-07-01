using Algorithmizm;
using AlgorithmizmModels.Level;
using AlgorithmizmModels.Primitives;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LevelModule
{
    public class LevelAssistant : MonoBehaviour
    {
        public const float SLOT_SIZE = 1f;

        public static readonly UnityEvent onStartTurn = new VoidEvent();

        [SerializeField] private SimulationResourceProvider _resourceProvider;
        
        [SerializeField] private BotComponent _botPrefab;
        [SerializeField] private Interpreatator _interpreatator;
        
        [SerializeField] private Transform _levelField;
        [SerializeField] private Transform _cameraTransform;

        private readonly Dictionary<Int2, Slot> _slots = new Dictionary<Int2, Slot>();

        public Level LevelDesign { get; private set; }

        public static Vector3 CoordsToPosition(Int2 coords)
        {
            Vector3 result = new Vector3(SLOT_SIZE / 2f, SLOT_SIZE / 2f);

            result.x += coords.x * SLOT_SIZE;
            result.y += coords.y * SLOT_SIZE;

            return result;
        }

        public bool IsPassable(Int2 position)
        {
            if (_slots.ContainsKey(position))
            {
                foreach (LevelObjectComponent itObject in _slots[position].LevelObjects)
                {
                    if (itObject.IsObstacle)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void Start()
        {
            Level levelDesign = new Level();
            levelDesign.levelObjects = new List<LevelObject>
            {
                new LevelObject { coords = new Int2(0, 1), name = "rock" },
                new LevelObject { coords = new Int2(0, 2), name = "rock" },
                new LevelObject { coords = new Int2(0, 3), name = "rock" },
                new LevelObject { coords = new Int2(0, 4), name = "rock" },
                new LevelObject { coords = new Int2(0, 5), name = "rock" },
                new LevelObject { coords = new Int2(0, 6), name = "rock" },
                new LevelObject { coords = new Int2(0, 7), name = "rock" },
                
                new LevelObject { coords = new Int2(1, 0), name = "wall" },
                new LevelObject { coords = new Int2(2, 0), name = "wall" },
                new LevelObject { coords = new Int2(3, 0), name = "wall" },
                new LevelObject { coords = new Int2(4, 0), name = "wall" },
                new LevelObject { coords = new Int2(5, 0), name = "wall" },
                
                new LevelObject { coords = new Int2(1, 1), name = "wall" },
                new LevelObject { coords = new Int2(2, 2), name = "wall" },
                new LevelObject { coords = new Int2(3, 3), name = "wall" },
                new LevelObject { coords = new Int2(4, 4), name = "wall" }
            };

            InstantiateLevel(levelDesign);
            InitializeBot();
        }

        private void InitializeBot()
        {
            BotComponent bot =  Instantiate(_botPrefab, _levelField);
            bot.Initialize(new LevelObject {coords = new Int2(), name = "bot"});
            bot.Initialize(this);
            
            _cameraTransform.SetParent(bot.transform);
            
            _interpreatator.Initialize(bot);
        }

        private void InstantiateLevel(Level levelDesign)
        {
            LevelDesign = levelDesign;
            
            _slots.Clear();
            
            foreach (LevelObject itObject in LevelDesign.levelObjects)
            {
                CreateLevelObject(itObject);
            }
        }

        private void CreateLevelObject(LevelObject levelObject)
        {
            Slot slot = GetOrCreateSlot(levelObject.coords);

            LevelObjectComponent levelObjectComponent =
                Instantiate(_resourceProvider.LevelObjects[levelObject.name], _levelField);
            levelObjectComponent.Initialize(levelObject);
            
            slot.LevelObjects.Add(levelObjectComponent);
        }

        private Slot GetOrCreateSlot(Int2 coords)
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
    }
}
