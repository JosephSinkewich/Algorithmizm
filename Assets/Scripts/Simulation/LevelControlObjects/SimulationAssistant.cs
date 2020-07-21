using Algorithmizm;
using AlgorithmizmModels.Level;
using AlgorithmizmModels.Primitives;
using LevelManagement;
using UnityEngine;
using UnityEngine.Events;

namespace LevelModule
{
    public class SimulationAssistant : LevelAssistant
    {
        public static readonly UnityEvent onStartTurn = new VoidEvent();

        [SerializeField] private BotComponent _botPrefab;
        [SerializeField] private Interpreatator _interpreatator;
        [SerializeField] private Transform _cameraTransform;

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
        
        protected override void Start()
        {
            LevelDesign = LevelSaveLoader.LoadLevel("levelLox");

            base.Start();
            
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
    }
}