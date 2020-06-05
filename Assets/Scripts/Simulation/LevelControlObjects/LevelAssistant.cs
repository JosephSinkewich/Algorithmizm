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

        public static UnityEvent onStartTurn = new VoidEvent();

        private Dictionary<Int2, Slot> _slots = new Dictionary<Int2, Slot>();

        public Level LevelDesign { get; set; }

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
    }
}
