using AlgorithmizmModels.Blocks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Algorithmizm
{
    public class TreePanel : MonoBehaviour
    {
        private const int UPDATE_PERIOD = 15;

        [SerializeField] private Transform _content;
        [SerializeField] private AlgorithmBlockUI _beginBlock;

        [SerializeField] private AlgorithmResourcesProvider _resourceProvider;

        private List<AlgorithmBlockUI> _blocks = new List<AlgorithmBlockUI>();

        private int _fixedUpdateCounter;

        public IReadOnlyCollection<AlgorithmBlockUI> Blocks => _blocks;

        public BeginBlock BeginBlock => _beginBlock.BlockData as BeginBlock;

        public UnityEvent<IReadOnlyCollection<AlgorithmBlockUI>> OnTreeChanged { get; set; } =
            new AlgorithmTreeListEvent();

        public UnityEvent<AlgorithmBlockUI> OnBlockClick { get; set; } =
            new AlgorithmBlockUIEvent();

        public UnityEvent<ValueUI, ActiveLabel> OnLabelClick { get; set; } =
            new ValueUIEvent();

        public UnityEvent<SetBlockUI, ActiveLabel> OnVariableLabelClick { get; set; } =
            new SetVariableEvent();

        public void AddBlock(AlgorithmBlockUI beforeBlock, IAlgorithmBlock newBlockData, bool isInside)
        {
            int index = _blocks.IndexOf(beforeBlock) + 1;

            AlgorithmBlockUI newBlock = CreateBlock(newBlockData);

            if (isInside)
            {
                if (beforeBlock.InnerBlock != null)
                {
                    newBlock.NextBlock = beforeBlock.InnerBlock;
                    beforeBlock.InnerBlock.MainPrevBlock = newBlock;
                }

                beforeBlock.InnerBlock = newBlock;
            }
            else
            {
                if (beforeBlock.NextBlock != null)
                {
                    newBlock.NextBlock = beforeBlock.NextBlock;
                    beforeBlock.NextBlock.MainPrevBlock = newBlock;
                }

                beforeBlock.NextBlock = newBlock;

                index = FindOutsidePosition(beforeBlock);
            }
            
            _blocks.Insert(index, newBlock);

            newBlock.MainPrevBlock = beforeBlock;
            newBlock.IsInsidePrevBlock = isInside;
            newBlock.RefreshAnData();

            SetBlocksOrderAndContentSiblings();

            RefreshTreeBlocksListeners();
            OnTreeChanged?.Invoke(_blocks);
        }

        public void DeleteBlock(AlgorithmBlockUI block)
        {
            AlgorithmBlockUI prevBlock = block.MainPrevBlock;
            AlgorithmBlockUI nextBlock = block.NextBlock;

            if (prevBlock.NextBlock == block)
            {
                prevBlock.NextBlock = nextBlock;
            }
            else if (prevBlock.InnerBlock == block)
            {
                prevBlock.InnerBlock = nextBlock;
            }

            while (block.InnerBlock != null)
            {
                DeleteBlock(block.InnerBlock);
            }

            _blocks.Remove(block);

            Destroy(block.gameObject);

            OnTreeChanged?.Invoke(_blocks);
        }

        public void MoveBlock(AlgorithmBlockUI movedBlock, AlgorithmBlockUI beforeBlock)
        {
            AlgorithmBlockUI prevBlock = movedBlock.MainPrevBlock;
            AlgorithmBlockUI nextBlock = movedBlock.NextBlock;

            if (prevBlock.NextBlock == movedBlock)
            {
                prevBlock.NextBlock = nextBlock;
            }
            else if (prevBlock.InnerBlock == movedBlock)
            {
                prevBlock.InnerBlock = nextBlock;
            }

            if (beforeBlock.InnerBlock != null)
            {
                movedBlock.NextBlock = beforeBlock.InnerBlock;
                beforeBlock.InnerBlock = movedBlock;
            }
            else
            {
                movedBlock.NextBlock = beforeBlock.NextBlock;
                beforeBlock.NextBlock = movedBlock;
            }

            SetBlocksOrderAndContentSiblings();

            OnTreeChanged?.Invoke(_blocks);
        }

        private int FindOutsidePosition(AlgorithmBlockUI block)
        {
            int blockIndex = _blocks.IndexOf(block) + 1;

            while (blockIndex < _blocks.Count)
            {
                if (_blocks[blockIndex].TabulationLevel > block.TabulationLevel)
                {
                    blockIndex++;
                }
                else
                {
                    break;
                }
            }

            return blockIndex;
        }

        private void Start()
        {
            _beginBlock.BlockData = new BeginBlock();
            _blocks.Add(_beginBlock);

            RefreshTreeBlocksListeners();
        }

        private void FixedUpdate()
        {
            _fixedUpdateCounter++;

            if (_fixedUpdateCounter >= UPDATE_PERIOD)
            {
                _fixedUpdateCounter = 0;

                _beginBlock.gameObject.SetActive(false);
                _beginBlock.gameObject.SetActive(true);
            }
        }

        private void RefreshTreeBlocksListeners()
        {
            foreach (AlgorithmBlockUI itBlock in _blocks)
            {
                itBlock.OnClick.RemoveListener(BlockClickHandler);
                itBlock.OnClick.AddListener(BlockClickHandler);

                itBlock.OnLabelClick.RemoveListener(LabelClickHandler);
                itBlock.OnLabelClick.AddListener(LabelClickHandler);

                if (itBlock is SetBlockUI itSetBlock)
                {
                    itSetBlock.OnVariableLabelClick.RemoveListener(VariableLabelClickHandler);
                    itSetBlock.OnVariableLabelClick.AddListener(VariableLabelClickHandler);
                }
            }
        }

        private AlgorithmBlockUI CreateBlock(IAlgorithmBlock blockData)
        {
            AlgorithmBlockUI result = null;
            if (blockData.Type == BlockType.Set)
            {
                result = Instantiate(_resourceProvider.SetBlockPrefab, _content);
            }
            else
            {
                result = Instantiate(_resourceProvider.AlgorithmBlockPrefab, _content);
            }
            
            result.BlockData = blockData;

            return result;
        }

        private void SetBlocksOrderAndContentSiblings()
        {
            _blocks = GetNextBlocksInOrder(_beginBlock);

            for (int i = 0; i < _blocks.Count; i++)
            {
                _blocks[i].transform.SetSiblingIndex(i);
            }
        }

        private List<AlgorithmBlockUI> GetNextBlocksInOrder(AlgorithmBlockUI block)
        {
            List<AlgorithmBlockUI> result = new List<AlgorithmBlockUI>();
            if (block == null) return result;

            result.InsertRange(0, GetNextBlocksInOrder(block.NextBlock));
            result.InsertRange(0, GetNextBlocksInOrder(block.InnerBlock));

            result.Insert(0, block);

            return result;
        }

        private void BlockClickHandler(AlgorithmBlockUI block)
        {
            OnBlockClick?.Invoke(block);
        }

        private void LabelClickHandler(ValueUI valueUi, ActiveLabel label)
        {
            OnLabelClick?.Invoke(valueUi, label);
        }

        private void VariableLabelClickHandler(SetBlockUI setBlock, ActiveLabel label)
        {
            OnVariableLabelClick?.Invoke(setBlock, label);
        }
    }
}
