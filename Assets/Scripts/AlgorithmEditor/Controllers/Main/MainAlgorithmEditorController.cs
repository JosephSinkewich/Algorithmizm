using AlgorithmizmModels.Blocks;
using AlgorithmizmModels.Math;
using AlgorithmizmModels.Variables;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Algorithmizm
{
    public class MainAlgorithmEditorController : MonoBehaviour
    {
        [SerializeField] private Transform _dialogLayer;

        [SerializeField] private EditPanel _editPanel;
        [SerializeField] private TreePanel _treePanel;
        [SerializeField] private VariablesPanel _variablesPanel;

        [SerializeField] private ContextMenuController _contextMenu;

        [SerializeField] private Button _doneButton;

        [SerializeField] private AlgorithmResourcesProvider _resourceProvider;

        private AddTool _addTool;
        private CursorTool _cursorTool;
        private DeleteTool _deleteTool;

        public Algorithm Algorithm { get; set; }

        public UnityEvent OnAlgorithmDone { get; set; } = 
            new VoidEvent();
        
        public void SetVariableNameFromDialog(IVariable variable)
        {
            SetValueDialog setDialog = Instantiate(_resourceProvider.SetValueDialog, _dialogLayer);

            setDialog.Init("Variable Name:", "VariableName");

            setDialog.OnOk.AddListener(
                (valueString) =>
                {
                    variable.Name = valueString;
                    _variablesPanel.AddVariable(variable);
                });
        }

        public void SetValueFromDialog(ValueUI valueUI, IValue value)
        {
            SetValueDialog setDialog = Instantiate(_resourceProvider.SetValueDialog, _dialogLayer);

            string inputValue = "0";

            bool isNumber = true;
            INumber number = value as INumber;
            if (number != null)
            {
                inputValue = number.Value.ToString();
            }

            IBoolean boolean = value as IBoolean;
            if (boolean != null)
            {
                inputValue = boolean.IsTrue ? "1" : "0";
                isNumber = false;
            }

            setDialog.Init("Set Constant", inputValue);

            setDialog.OnOk.AddListener(
                (valueString) =>
                {
                    if (isNumber)
                    {
                        if (float.TryParse(valueString, out float floatValue))
                        {
                            FloatConstant constant = number as FloatConstant;

                            if (constant != null)
                            {
                                number.Value = floatValue;
                            }
                            else
                            {
                                constant = new FloatConstant();
                                constant.Value = floatValue;
                                _cursorTool.SetLabelTarget.Value = constant;

                                if (_cursorTool.SetLabelTarget.Value.Parent == null)
                                {
                                    _cursorTool.SetLabelValueUi.Value = _cursorTool.SetLabelTarget.Value;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (float.TryParse(valueString, out float floatValue))
                        {
                            BoolConstant constant = boolean as BoolConstant;

                            if (constant != null)
                            {
                                boolean.IsTrue = floatValue != 0;
                            }
                            else
                            {
                                constant = new BoolConstant();
                                constant.IsTrue = floatValue != 0;
                                _cursorTool.SetLabelTarget.Value = constant;

                                if (_cursorTool.SetLabelTarget.Value.Parent == null)
                                {
                                    _cursorTool.SetLabelValueUi.Value = _cursorTool.SetLabelTarget.Value;
                                }
                            }
                        }
                    }

                    valueUI.RebuildAnValue();
                });
        }

        private void Start()
        {
            InitTools();
            
            AddListeners();
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            _editPanel.OnToolChanged.AddListener(ToolChangedHandler);

            _treePanel.OnBlockClick.AddListener(BlockClickHandler);
            _treePanel.OnLabelClick.AddListener(LabelClickHandler);
            _treePanel.OnVariableLabelClick.AddListener(VariableLabelClickHandler);

            _variablesPanel.OnVariableClick.AddListener(VariableClickHandler);
            _variablesPanel.OnPanelClick.AddListener(VariablesPanelClickHandler);

            _contextMenu.OnButtonClick.AddListener(ContextMenuItemClickHandler);

            _doneButton.onClick.AddListener(DoneButtonClickHandler);
        }

        private void RemoveListeners()
        {
            _editPanel.OnToolChanged.RemoveListener(ToolChangedHandler);

            _treePanel.OnBlockClick.RemoveListener(BlockClickHandler);
            _treePanel.OnLabelClick.RemoveListener(LabelClickHandler);
            _treePanel.OnVariableLabelClick.RemoveListener(VariableLabelClickHandler);

            _variablesPanel.OnVariableClick.RemoveListener(VariableClickHandler);
            _variablesPanel.OnPanelClick.RemoveListener(VariablesPanelClickHandler);

            _contextMenu.OnButtonClick.RemoveListener(ContextMenuItemClickHandler);

            _doneButton.onClick.RemoveListener(DoneButtonClickHandler);
        }

        private void InitTools()
        {
            _addTool = new AddTool(this, _contextMenu, _editPanel, _treePanel, _resourceProvider);
            _cursorTool = new CursorTool(this, _contextMenu, _variablesPanel);
            _deleteTool = new DeleteTool(_editPanel, _treePanel);
        }

        private void ToolChangedHandler(EditTools tool)
        {
            switch (tool)
            {
                case EditTools.Add:
                    {
                        _addTool.AddInit();
                    }
                    break;
                case EditTools.Move:
                    {
                        MoveInit();
                    }
                    break;
                case EditTools.Delete:
                    {
                    }
                    break;
                case EditTools.Cursor:
                    break;
            }
        }

        private void BlockClickHandler(AlgorithmBlockUI sender)
        {
            switch (_editPanel.CurrentTool)
            {
                case EditTools.Add:
                    {
                        _addTool.AddOnBlock(sender);
                    }
                    break;
                case EditTools.Delete:
                    {
                        _deleteTool.DeleteOnBlock(sender);
                    }
                    break;
            }
        }

        private void VariableClickHandler(VariableUI sender)
        {
            switch (_editPanel.CurrentTool)
            {
                case EditTools.Add:
                    {
                        _addTool.AddOnVariablesPanel();
                    }
                    break;
            }
        }

        private void VariablesPanelClickHandler()
        {
            switch (_editPanel.CurrentTool)
            {
                case EditTools.Add:
                    {
                        _addTool.AddOnVariablesPanel();
                    }
                    break;
            }
        }

        private void LabelClickHandler(ValueUI valueUi, ActiveLabel sender)
        {
            if (_editPanel.CurrentTool == EditTools.Cursor)
            {
                _cursorTool.CursorOnLabel(valueUi, sender);
            }
        }

        private void VariableLabelClickHandler(SetBlockUI setBlock, ActiveLabel sender)
        {
            if (_editPanel.CurrentTool == EditTools.Cursor)
            {
                _cursorTool.CursorOnVariableLabel(setBlock, sender);
            }
        }

        private void ContextMenuItemClickHandler(MenuButton sender)
        {
            switch (_editPanel.CurrentTool)
            {
                case EditTools.Add:
                    {
                        if (_addTool.IsAddBlock)
                        {
                            _addTool.AddBlockOnContextMenuItem(sender);
                        }
                        else
                        {
                            _addTool.AddVariableOnContextMenuItem(sender);
                        }
                    }
                    break;
                case EditTools.Cursor:
                    {
                        _cursorTool.CursorOnContextMenuItem(sender);
                    }
                    break;
            }
        }

        private void MoveInit()
        {

        }

        private void DoneButtonClickHandler()
        {
            InitAlgorithmBlocks();

            Algorithm = new Algorithm();
            Algorithm.BeginBlock = _treePanel.BeginBlock;
            Algorithm.Variables = _variablesPanel.Variables;

            OnAlgorithmDone?.Invoke();
        }

        private void InitAlgorithmBlocks()
        {
            foreach (AlgorithmBlockUI itBlock in _treePanel.Blocks)
            {
                AlgorithmBlockUI nextBlock = itBlock.NextBlock;
                if (nextBlock == null)
                {
                    nextBlock = FindNextForBlock(itBlock);
                }

                switch (itBlock.BlockData)
                {
                    case BeginBlock beginBlock:
                        {
                            beginBlock.Next = nextBlock?.BlockData;
                        }
                        break;
                    case ActionBlock actionBlock:
                        {
                            actionBlock.Next = nextBlock?.BlockData;
                        }
                        break;
                    case SetBlock setBlock:
                        {
                            setBlock.Next = nextBlock?.BlockData;

                            setBlock.Value = itBlock.ValueUis[0].Value;
                        }
                        break;
                    case IfBlock ifBlock:
                        {
                            ifBlock.ElseBlock = nextBlock?.BlockData;

                            ifBlock.ThenBlock = itBlock.InnerBlock?.BlockData;
                            ifBlock.Condition = itBlock.ValueUis[0].Value as IBoolean;
                        }
                        break;
                    case WhileBlock whileBlock:
                        {
                            whileBlock.OuterBlock = nextBlock?.BlockData;

                            whileBlock.InnerBlock = itBlock.InnerBlock?.BlockData;
                            whileBlock.Condition = itBlock.ValueUis[0].Value as IBoolean;
                        }
                        break;
                }
            }
        }

        private AlgorithmBlockUI FindNextForBlock(AlgorithmBlockUI block)
        {
            AlgorithmBlockUI currentBlock = block.MainPrevBlock;

            while (currentBlock != null)
            {
                if (currentBlock.TabulationLevel < block.TabulationLevel)
                {
                    if (currentBlock.BlockData.Type == BlockType.While)
                    {
                        break;
                    }
                    else if (currentBlock.BlockData.Type == BlockType.If)
                    {
                        AlgorithmBlockUI prevBlock = currentBlock;
                        currentBlock = currentBlock.NextBlock;

                        if (currentBlock == null)
                        {
                            currentBlock = FindNextForBlock(prevBlock);
                        }

                        break;
                    }
                }

                currentBlock = currentBlock.MainPrevBlock;
            }

            return currentBlock;
        }
    }
}
