using Algorithmizm;
using AlgorithmizmModels.Blocks;
using Assets.Scripts.AlgorithmEditor.Controllers.Blocks;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.AlgorithmEditor.Controllers.ResourceProviders
{
    [CreateAssetMenu(fileName = "AlgorithmTreeResourcesProvider", menuName = "AlgorithmEditor")]
    public class AlgorithmTreeResourceProvider : SerializedScriptableObject
    {
        [SerializeField] private AlgorithmBlockUI _algorithmBlockPrefab;
        [SerializeField] private ActiveLabel _activeLabelPrefab;
        [SerializeField] private TextMeshProUGUI _autosizebleTextPrefab;
        [SerializeField] private ValueUI _valueUiPrefab;

        [SerializeField] private List<BlockData> _blockDatas;
        [OdinSerialize] private Dictionary<BlockType, Sprite> _blockTypeSprites;

        [SerializeField] private Color _activeLabelNormalColor;
        [SerializeField] private Color _activeLabelErrorColor;

        public AlgorithmBlockUI AlgorithmBlockPrefab => _algorithmBlockPrefab;
        public ActiveLabel ActiveLabelPrefab => _activeLabelPrefab;
        public TextMeshProUGUI AutosizebleTextPrefab => _autosizebleTextPrefab;
        public ValueUI ValueUiPrefab => _valueUiPrefab;

        public List<BlockData> BlockDatas => _blockDatas;
        public Dictionary<BlockType, Sprite> BlockTypeSprites => _blockTypeSprites;

        public Color ActiveLabelNormalColor => _activeLabelNormalColor;
        public Color ActiveLabelErrorColor => _activeLabelErrorColor;
    }
}
