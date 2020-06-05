using AlgorithmizmModels.Primitives;
using System.Collections;
using UnityEngine;

namespace LevelModule
{
    public class BotComponent : LevelObjectComponent
    {
        [SerializeField] private LevelAssistant _levelAssistant;
        [SerializeField] private Side _direction;

        [SerializeField] private float _actionDuation;
        [SerializeField] private int _framesCount;
        [SerializeField] private float _actionDelay;

        private Coroutine _currentCoroutine;

        private float FramePeriod => _actionDuation / _framesCount;

        public Side Direction
        {
            get { return _direction; }
            set
            {
                _direction = value;
                ActualizeTransform();
            }
        }

        #region Procedures

        public void MoveForward()
        {
            Int2 coordOffset = _direction.ToInt2();
            Int2 newCoords = Coords + coordOffset;

            if (_levelAssistant.IsPassable(newCoords))
            {
                RunActionCoroutine(Move(Coords, newCoords));
                Coords = newCoords;
            }
        }

        public void Rotate(int rotation)
        {
            Side newDirection = _direction.Rotate(rotation);

            RunActionCoroutine(ProcessRotate(rotation));

            _direction = newDirection;
        }

        #endregion

        private void RunActionCoroutine(IEnumerator enumerator)
        {
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
                ActualizeTransform();
            }
            _currentCoroutine = StartCoroutine(enumerator);
        }

        private void Start()
        {
            ActualizeTransform();
        }

        private void ActualizeTransform()
        {
            transform.position = LevelAssistant.CoordsToPosition(Coords);
            transform.eulerAngles = new Vector3(0, 0, _direction.ToAngle());
        }

        private IEnumerator Move(Int2 from, Int2 to)
        {
            Vector3 fromPosition = LevelAssistant.CoordsToPosition(from);
            Vector3 toPosition = LevelAssistant.CoordsToPosition(to);
            
            float step = LevelAssistant.SLOT_SIZE / _framesCount;
            Vector3 stepVector = (toPosition - fromPosition).normalized * step;
            float pathLength = 0;

            while (pathLength < LevelAssistant.SLOT_SIZE)
            {
                transform.position += stepVector;
                pathLength += step;

                yield return new WaitForSeconds(FramePeriod);
            }

            transform.position = toPosition;
        }

        private IEnumerator ProcessRotate(int rotation)
        {
            float fromAngle = _direction.ToAngle();
            float rotateAngle = rotation * 90;
            float toAngle = fromAngle + rotateAngle;
            
            float step = rotateAngle / _framesCount;
            float rotated = 0;
            float rotationLength = Mathf.Abs(rotateAngle);
            float absStep = Mathf.Abs(step);

            float currentAngle = fromAngle;

            while (rotated < rotationLength)
            {
                currentAngle += step;
                transform.eulerAngles = new Vector3(0, 0, currentAngle);

                rotated += absStep;

                yield return new WaitForSeconds(FramePeriod);
            }

            transform.eulerAngles = new Vector3(0, 0, toAngle);
        }
    }
}
