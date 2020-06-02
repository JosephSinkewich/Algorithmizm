using UnityEngine;
using UnityEngine.Events;
using AlgorithmizmModels.Blocks;

namespace Algorithmizm
{
    public class SetBlockUI : AlgorithmBlockUI
    {
        [SerializeField] private ActiveLabel _variableLabel;

        public SetBlock SetBlock => BlockData as SetBlock;

        public UnityEvent<SetBlockUI, ActiveLabel> OnVariableLabelClick { get; set; } =
            new SetVariableEvent();

        public void RefreshParameter()
        {
            BlockData.Data.parameters[0].type = SetBlock.Variable.Type;
            RefreshAnData();
        }

        private void Start()
        {
            _variableLabel?.OnClick.AddListener(VariableLabelClickHandler);
        }

        private void OnDestroy()
        {
            _variableLabel?.OnClick.RemoveListener(VariableLabelClickHandler);
        }

        private void VariableLabelClickHandler(ActiveLabel label)
        {
            OnVariableLabelClick?.Invoke(this, label);
        }
    }
}
