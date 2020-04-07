using AlgorithmizmModels.Blocks;
using Assets.Scripts.AlgorithmEditor.Controllers.Blocks;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AlgorithmEditor.Controllers.ResourceProviders
{
    [CreateAssetMenu(fileName = "AlgorithmTreeResourcesProvider", menuName = "AlgorithmEditor")]
    public class AlgorithmTreeResourceProvider : ScriptableObject
    {
        [SerializeField] private AlgorithmBlockUI _algorithmBlockPrefab;
        [SerializeField] private List<BlockData> _blockDatas;

        public AlgorithmBlockUI AlgorithmBlockPrefab => _algorithmBlockPrefab;
        public List<BlockData> BlockDatas => _blockDatas;
    }
}
