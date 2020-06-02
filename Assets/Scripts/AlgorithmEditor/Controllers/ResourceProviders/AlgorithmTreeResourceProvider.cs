using AlgorithmizmModels.Blocks;
using AlgorithmizmModels.Math;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Algorithmizm
{
    [CreateAssetMenu(fileName = "AlgorithmTreeResourcesProvider", menuName = "AlgorithmEditor")]
    public class AlgorithmTreeResourceProvider : SerializedScriptableObject
    {
        [SerializeField] private AlgorithmBlockUI _algorithmBlockPrefab;
        [SerializeField] private SetBlockUI _setBlockPrefab;
        [SerializeField] private ActiveLabel _activeLabelPrefab;
        [SerializeField] private TextMeshProUGUI _autosizebleTextPrefab;
        [SerializeField] private ValueUI _valueUiPrefab;

        [SerializeField] private VariableUI _variableUIPrefab;

        [SerializeField] private MessageDialog _messageDialog;
        [SerializeField] private SetValueDialog _setValueDialog;

        [SerializeField] private List<BlockData> _blockDatas;
        [OdinSerialize] private Dictionary<BlockType, Sprite> _blockTypeSprites;

        [OdinSerialize] private Dictionary<ValueType, Sprite> _valueTypeSprites;

        [SerializeField] private Color _activeLabelNormalColor;
        [SerializeField] private Color _activeLabelErrorColor;

        public AlgorithmBlockUI AlgorithmBlockPrefab => _algorithmBlockPrefab;
        public SetBlockUI SetBlockPrefab => _setBlockPrefab;
        public ActiveLabel ActiveLabelPrefab => _activeLabelPrefab;
        public TextMeshProUGUI AutosizebleTextPrefab => _autosizebleTextPrefab;
        public ValueUI ValueUiPrefab => _valueUiPrefab;

        public VariableUI VariableUIPrefab => _variableUIPrefab;

        public MessageDialog MessageDialog => _messageDialog;
        public SetValueDialog SetValueDialog => _setValueDialog;

        public List<BlockData> BlockDatas => _blockDatas;
        public Dictionary<BlockType, Sprite> BlockTypeSprites => _blockTypeSprites;

        public Dictionary<ValueType, Sprite> ValueTypeSprites => _valueTypeSprites;

        public Color ActiveLabelNormalColor => _activeLabelNormalColor;
        public Color ActiveLabelErrorColor => _activeLabelErrorColor;
    }
}
