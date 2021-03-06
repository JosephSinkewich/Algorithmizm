﻿using AlgorithmizmModels.Level;
using AlgorithmizmModels.Primitives;
using UnityEngine;

namespace LevelModule
{
    public abstract class LevelObjectComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;

        [SerializeField] private bool _isObstacle;
        
        private LevelObject _levelObject;

        public string Name
        {
            get { return _levelObject.name; }
            set { _levelObject.name = value; }
        }

        public Int2 Coords
        {
            get { return _levelObject.coords; }
            set { _levelObject.coords = value; }
        }

        public GameObject Prefab => _prefab;
        public bool IsObstacle => _isObstacle;

        public void Initialize(LevelObject levelObject)
        {
            _levelObject = levelObject;
            transform.position = LevelAssistant.CoordsToPosition(_levelObject.coords);
        }
    }
}
