﻿using AlgorithmizmModels.Blocks;
using Assets.Scripts.AlgorithmEditor.Controllers.Blocks;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AlgorithmEditor.Controllers.ResourceProviders
{
    [CreateAssetMenu(fileName = "AlgorithmTreeResourcesProvider", menuName = "AlgorithmEditor")]
    public class AlgorithmTreeResourceProvider : SerializedScriptableObject
    {
        [SerializeField] private AlgorithmBlockUI _algorithmBlockPrefab;
        [SerializeField] private List<BlockData> _blockDatas;
        [OdinSerialize] private Dictionary<BlockType, Sprite> _blockTypeSprites;

        public AlgorithmBlockUI AlgorithmBlockPrefab => _algorithmBlockPrefab;
        public List<BlockData> BlockDatas => _blockDatas;
        public Dictionary<BlockType, Sprite> BlockTypeSprites => _blockTypeSprites;
    }
}
