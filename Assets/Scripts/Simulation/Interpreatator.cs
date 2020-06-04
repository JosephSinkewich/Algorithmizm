using AlgorithmizmModels.Blocks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LevelModule
{
    public class Interpreatator : MonoBehaviour
    {
        [SerializeField] private BotComponent _bot;

        private IAlgorithmBlock _currentBlock;

        private Dictionary<string, Action> _procedures = new Dictionary<string, Action>();

        public Algorithm Algorithm { get; set; }
        public bool IsDone { get; set; }

        public void StartAlgorithm()
        {
            IsDone = false;
            _currentBlock = Algorithm?.BeginBlock;
            ExecuteNext();
        }

        public void ExecuteNext()
        {
            _currentBlock = _currentBlock?.Next;
            ExecuteBlock(_currentBlock);
        }

        private void Start()
        {
            InitProcedures();
        }

        private void InitProcedures()
        {
            _procedures = new Dictionary<string, Action>();

            _procedures.Add("MoveForward", MoveForwardProcedure);
        }

        private void ExecuteBlock(IAlgorithmBlock block)
        {
            if (block == null)
            {
                Debug.Log("IsDone");
                IsDone = true;
                return;
            }

            if (block.Type == BlockType.Action)
            {
                if (_procedures.ContainsKey(block.Data.name))
                {
                    _procedures[block.Data.name].Invoke();
                }

                return;
            }


        }

        #region Procedures

        private void MoveForwardProcedure()
        {
            Debug.Log(">>>>");
        }

        #endregion
    }
}
