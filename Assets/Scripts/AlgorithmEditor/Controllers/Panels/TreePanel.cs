using AlgorithmizmModels.Blocks;
using Assets.Scripts.AlgorithmEditor.Controllers.Blocks;
using Assets.Scripts.AlgorithmEditor.Controllers.ResourceProviders;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Assets.Scripts.AlgorithmEditor.Events;

namespace Assets.Scripts.AlgorithmEditor.Controllers.Panels
{
    public class TreePanel : MonoBehaviour
    {
        [SerializeField] private Transform _content;
        [SerializeField] private AlgorithmBlockUI _beginBlock;

        [SerializeField] private AlgorithmTreeResourceProvider _resourceProvider;

        private List<AlgorithmBlockUI> _blocks = new List<AlgorithmBlockUI>();

        public IReadOnlyCollection<AlgorithmBlockUI> Blocks => _blocks;

        public UnityEvent<IReadOnlyCollection<AlgorithmBlockUI>> OnTreeChanged { get; set; } =
            new AlgorithmTreeListEvent();

        public UnityEvent<AlgorithmBlockUI> OnBlockClick { get; set; } =
            new AlgorithmBlockUIEvent();

        public void AddBlock(AlgorithmBlockUI beforeBlock, IAlgorithmBlock newBlockData)
        {
            int index = _blocks.IndexOf(beforeBlock);

            AlgorithmBlockUI newBlock = CreateBlock(newBlockData);
            
            _blocks.Insert(index + 1, newBlock);

            SetContentSiblings();

            RefreshTreeBlocksListeners();
            OnTreeChanged?.Invoke(_blocks);
        }

        private void Start()
        {
            _beginBlock.BlockData = new BeginBlock();
            _blocks.Add(_beginBlock);

            RefreshTreeBlocksListeners();
        }

        private void RefreshTreeBlocksListeners()
        {
            foreach (AlgorithmBlockUI itBlock in _blocks)
            {
                itBlock.OnClick.RemoveListener(BlockClickHandler);
            }
            foreach (AlgorithmBlockUI itBlock in _blocks)
            {
                itBlock.OnClick.AddListener(BlockClickHandler);
            }
        }

        private AlgorithmBlockUI CreateBlock(IAlgorithmBlock blockData)
        {
            AlgorithmBlockUI result = Instantiate(_resourceProvider.AlgorithmBlockPrefab, _content);
            result.BlockData = blockData;
            result.RefreshAnData();

            return result;
        }

        private void SetContentSiblings()
        {
            for (int i = 0; i < _blocks.Count; i++)
            {
                _blocks[i].transform.SetSiblingIndex(i);
            }
        }

        private void BlockClickHandler(AlgorithmBlockUI block)
        {
            OnBlockClick?.Invoke(block);
        }
    }
}
