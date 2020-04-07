using Assets.AlgorithmizmModels.Primitives;
using UnityEngine;

namespace Assets.LevelModule.Components
{
    public class BotComponent : LevelObjectComponent
    {
        [SerializeField] private Side _direction;

        public Side Direction
        {
            get { return _direction; }
            set
            {
                _direction = value;
                ActualizeRotation();
            }
        }

        private void Start()
        {
            ActualizeRotation();
        }

        private void ActualizeRotation()
        {
            float angle = (int)_direction * 90;
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }
}
