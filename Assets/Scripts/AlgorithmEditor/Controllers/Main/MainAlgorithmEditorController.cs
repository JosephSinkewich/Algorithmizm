using AlgorithmizmModels.Blocks;
using AlgorithmizmModels.Math;
using AlgorithmizmModels.Variables;
using Assets.Scripts.AlgorithmEditor.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Algorithmizm
{
    public class MainAlgorithmEditorController : MonoBehaviour
    {
        [SerializeField] private Transform _canvasTransform;

        [SerializeField] private EditPanel _editPanel;
        [SerializeField] private TreePanel _treePanel;
        [SerializeField] private VariablesPanel _variablesPanel;
        [SerializeField] private ContextMenuController _contextMenu;

        [SerializeField] private AlgorithmTreeResourceProvider _resourceProvider;

        private AddBlockSteps _addBlockStep;
        private AlgorithmBlockUI _addTarget;
        private bool _isAddBlock;
        private bool _addInsertInside;
        private BlockType _addType;
        private Dictionary<MenuButton, bool> _addInsertTypeMenuButtons;
        private Dictionary<MenuButton, BlockType> _addTypeMenuButtons;
        private Dictionary<MenuButton, IAlgorithmBlock> _addBlockMenuButtons;
        private IAlgorithmBlock _addBlockData;

        private AddVariableSteps _addVariableStep;
        private ValueType _addVariableType;
        private Dictionary<MenuButton, ValueType> _addVariableTypeMenuButtons;

        private SetLabelSteps _setLabelSteps;
        private ActiveLabel _setLabelTarget;
        private bool _isSetedLabelVariable;
        private SetBlockUI _setVariableBlock;
        private ValueUI _setLabelValueUi;
        private ActiveLabelType _activeLabelType;
        private Dictionary<MenuButton, ActiveLabelType> _labelTypeMenuButtons;
        private IVariable _activeLabelVariable;
        private Dictionary<MenuButton, IVariable> _labelVariableMenuButtons;
        private Operations _numberOperation;
        private Dictionary<MenuButton, Operations> _numberOperationMenuButtons;
        private LogicOperations _logicOperation;
        private Dictionary<MenuButton, LogicOperations> _logicOperationMenuButtons;
        private Relations _relation; 
        private Dictionary<MenuButton, Relations> _relationMenuButtons;

        private string _dialogValue;

        private void Start()
        {
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
        }

        private void ToolChangedHandler(EditTools tool)
        {
            switch (tool)
            {
                case EditTools.Add:
                    {
                        AddInit();
                    }
                    break;
                case EditTools.Move:
                    {
                        MoveInit();
                    }
                    break;
                case EditTools.Delete:
                    {
                        DeleteInit();
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
                        AddOnBlock(sender);
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
                        AddOnVariablesPanel();
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
                        AddOnVariablesPanel();
                    }
                    break;
            }
        }

        private void LabelClickHandler(ValueUI valueUi, ActiveLabel sender)
        {
            if (_editPanel.CurrentTool == EditTools.Cursor)
            {
                switch (_setLabelSteps)
                {
                    case SetLabelSteps.SetTarget:
                        {
                            _setLabelTarget = sender;
                            _isSetedLabelVariable = false;
                            _setLabelValueUi = valueUi;

                            _setLabelSteps = SetLabelSteps.ChoiseLabelType;
                            ClearContextMenu();
                            SetLabelTypeMenuItems();
                            _contextMenu.gameObject.SetActive(true);
                        }
                        break;
                }
            }
        }

        private void VariableLabelClickHandler(SetBlockUI setBlock, ActiveLabel sender)
        {
            if (_editPanel.CurrentTool == EditTools.Cursor)
            {
                switch (_setLabelSteps)
                {
                    case SetLabelSteps.SetTarget:
                        {
                            _setLabelTarget = sender;
                            _isSetedLabelVariable = true;
                            _setVariableBlock = setBlock;
                            _activeLabelType = ActiveLabelType.Variable;

                            _setLabelSteps = SetLabelSteps.SetValue;
                            ClearContextMenu();
                            SetVariableMenuItems();
                            _contextMenu.gameObject.SetActive(true);
                        }
                        break;
                }
            }
        }

        private void ContextMenuItemClickHandler(MenuButton sender)
        {
            switch (_editPanel.CurrentTool)
            {
                case EditTools.Add:
                    {
                        if (_isAddBlock)
                        {
                            AddBlockOnContextMenuItem(sender);
                        }
                        else
                        {
                            AddVariableOnContextMenuItem(sender);
                        }
                    }
                    break;
                case EditTools.Cursor:
                    {
                        CursorOnContextMenuItem(sender);
                    }
                    break;
            }
        }

        private void AddOnBlock(AlgorithmBlockUI sender)
        {
            switch (_addBlockStep)
            {
                case AddBlockSteps.SetTarget:
                    {
                        _addTarget = sender;
                        _isAddBlock = true;
                        
                        ClearContextMenu();

                        if (_addTarget.BlockData.Type == BlockType.If
                            || _addTarget.BlockData.Type == BlockType.While)
                        {
                            SetAddInsertTypeMenuItems();
                            _contextMenu.gameObject.SetActive(true);

                            _addBlockStep = AddBlockSteps.ChoiseInsertType;
                        }
                        else
                        {
                            _addInsertInside = false;
                            
                            SetAddTypeMenuItems();
                            _contextMenu.gameObject.SetActive(true);

                            _addBlockStep = AddBlockSteps.ChoiseBlockType;
                        }
                    }
                    break;
            }
        }

        private void AddOnVariablesPanel()
        {
            switch (_addBlockStep)
            {
                case AddBlockSteps.SetTarget:
                    {
                        _isAddBlock = false;

                        ClearContextMenu();
                        SetAddVariableTypeMenuItems();
                        _contextMenu.gameObject.SetActive(true);

                        _addVariableStep = AddVariableSteps.SetType;
                    }
                    break;
            }
        }

        private void AddBlockOnContextMenuItem(MenuButton sender)
        {
            switch (_addBlockStep)
            {
                case AddBlockSteps.ChoiseInsertType:
                    {
                        if (!_addInsertTypeMenuButtons.ContainsKey(sender))
                        {
                            return;
                        }

                        _addInsertInside = _addInsertTypeMenuButtons[sender];

                        ClearContextMenu();
                        SetAddTypeMenuItems();

                        _addBlockStep = AddBlockSteps.ChoiseBlockType;
                    }
                    break;

                case AddBlockSteps.ChoiseBlockType:
                    {
                        if (!_addTypeMenuButtons.ContainsKey(sender))
                        {
                            return;
                        }

                        _addType = _addTypeMenuButtons[sender];

                        ClearContextMenu();
                        SetAddBlocksMenuItems();

                        _addBlockStep = AddBlockSteps.ChoiseBlock;
                    }
                    break;

                case AddBlockSteps.ChoiseBlock:
                    {
                        if (!_addBlockMenuButtons.ContainsKey(sender))
                        {
                            return;
                        }

                        _contextMenu.gameObject.SetActive(false);

                        _addBlockData = _addBlockMenuButtons[sender];

                        _treePanel.AddBlock(_addTarget, _addBlockData, _addInsertInside);

                        _editPanel.CurrentTool = EditTools.Cursor;

                        _addBlockStep = AddBlockSteps.SetBlockData;
                    }
                    break;
            }
        }

        private void AddVariableOnContextMenuItem(MenuButton sender)
        {
            switch (_addVariableStep)
            {
                case AddVariableSteps.SetType:
                    {
                        if (!_addVariableTypeMenuButtons.ContainsKey(sender))
                        {
                            return;
                        }

                        _addVariableType = _addVariableTypeMenuButtons[sender];
                        _contextMenu.gameObject.SetActive(false);

                        IVariable newVariable = null;

                        if (_addVariableType == ValueType.Bool)
                        {
                            newVariable = new BoolVariable();
                        }
                        else
                        {
                            newVariable = new FloatVariable();
                        }

                        _addVariableStep = AddVariableSteps.SetName;
                        SetVariableNameFromDialog(newVariable);
                    }
                    break;
            }
        }

        private void CursorOnContextMenuItem(MenuButton sender)
        {
            switch (_setLabelSteps)
            {
                case SetLabelSteps.ChoiseLabelType:
                    {
                        _activeLabelType = _labelTypeMenuButtons[sender];
                        _setLabelSteps = SetLabelSteps.SetValue;

                        ClearContextMenu();
                        if (_activeLabelType == ActiveLabelType.Variable)
                        {
                            SetLabelVariableMenuItems();
                        }
                        else if (_activeLabelType == ActiveLabelType.Expression)
                        {
                            if (_setLabelTarget.ValueType == ValueType.Bool)
                            {
                                _setLabelTarget.Value = new LogicExpression();
                            }
                            else
                            {
                                _setLabelTarget.Value = new Expression();
                            }

                            if (_setLabelTarget.Value.Parent == null)
                            {
                                _setLabelValueUi.Value = _setLabelTarget.Value;
                            }
                            _setLabelValueUi.RebuildAnValue();
                            
                            _contextMenu.gameObject.SetActive(false);
                            _setLabelSteps = SetLabelSteps.SetTarget;
                        }
                        else if (_activeLabelType == ActiveLabelType.Constant)
                        {
                            SetValueFromDialog(_setLabelValueUi, _setLabelTarget.Value);

                            _contextMenu.gameObject.SetActive(false);
                            _setLabelSteps = SetLabelSteps.SetTarget;
                        }
                        else if (_activeLabelType == ActiveLabelType.Operation)
                        {
                            if (_setLabelTarget.LabelType == ActiveLabelType.Condition)
                            {
                                SetRelationMenuItems();
                            }
                            else
                            {
                                if (_setLabelTarget.ValueType == ValueType.Bool)
                                {
                                    SetLogicOperationMenuItems();
                                }
                                else
                                {
                                    SetNumberOperationMenuItems();
                                }
                            }

                            _setLabelSteps = SetLabelSteps.SetValue;
                        }
                        else if (_activeLabelType == ActiveLabelType.Condition)
                        {
                            _setLabelTarget.Value = new Condition();
                            
                            if (_setLabelTarget.Value.Parent == null)
                            {
                                _setLabelValueUi.Value = _setLabelTarget.Value;
                            }
                            _setLabelValueUi.RebuildAnValue();

                            _contextMenu.gameObject.SetActive(false);
                            _setLabelSteps = SetLabelSteps.SetTarget;
                        }
                    }
                    break;

                case SetLabelSteps.SetValue:
                    {
                        if (_activeLabelType == ActiveLabelType.Operation)
                        {

                            if (_setLabelTarget.LabelType == ActiveLabelType.Condition)
                            {
                                Condition condition = _setLabelTarget.Value as Condition;
                                condition.Relation = _relationMenuButtons[sender];
                            }
                            else
                            {
                                if (_setLabelTarget.ValueType == ValueType.Bool)
                                {
                                    LogicExpression logicExpression = _setLabelTarget.Value as LogicExpression;
                                    logicExpression.Operation = _logicOperationMenuButtons[sender];
                                }
                                else
                                {
                                    Expression numberExpression = _setLabelTarget.Value as Expression;
                                    numberExpression.Operation = _numberOperationMenuButtons[sender];
                                }
                            }

                            _setLabelValueUi.RebuildAnValue();
                        }
                        else if (_activeLabelType == ActiveLabelType.Variable)
                        {
                            if (_isSetedLabelVariable)
                            {
                                _setLabelTarget.Value = _labelVariableMenuButtons[sender];
                                _setVariableBlock.SetBlock.Variable = _labelVariableMenuButtons[sender];

                                _setVariableBlock.RefreshParameter();
                            }
                            else
                            {
                                _setLabelTarget.Value = _labelVariableMenuButtons[sender];

                                if (_setLabelTarget.Value.Parent == null)
                                {
                                    _setLabelValueUi.Value = _setLabelTarget.Value;
                                }
                                _setLabelValueUi.RebuildAnValue();
                            }
                        }

                        _contextMenu.gameObject.SetActive(false);
                        _setLabelSteps = SetLabelSteps.SetTarget;
                    }
                    break;
            }
        }

        private void SetValueFromDialog(ValueUI valueUI, IValue value)
        {
            SetValueDialog setDialog = Instantiate(_resourceProvider.SetValueDialog, _canvasTransform);

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
                                _setLabelTarget.Value = constant;

                                if (_setLabelTarget.Value.Parent == null)
                                {
                                    _setLabelValueUi.Value = _setLabelTarget.Value;
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
                                _setLabelTarget.Value = constant;

                                if (_setLabelTarget.Value.Parent == null)
                                {
                                    _setLabelValueUi.Value = _setLabelTarget.Value;
                                }
                            }
                        }
                    }

                    valueUI.RebuildAnValue();
                });
        }

        private void SetVariableNameFromDialog(IVariable variable)
        {
            SetValueDialog setDialog = Instantiate(_resourceProvider.SetValueDialog, _canvasTransform);

            setDialog.Init("Variable Name:", "VariableName");

            setDialog.OnOk.AddListener(
                (valueString) => 
                {
                    variable.Name = valueString;
                    _variablesPanel.AddVariable(variable);
                });
        }

        private void SetAddInsertTypeMenuItems()
        {
            _addInsertTypeMenuButtons = new Dictionary<MenuButton, bool>();
            MenuButton button;

            button = _contextMenu.AddButton("Inside");
            _addInsertTypeMenuButtons.Add(button, true);

            button = _contextMenu.AddButton("Outside");
            _addInsertTypeMenuButtons.Add(button, false);
        }

        private void SetAddTypeMenuItems()
        {
            _addTypeMenuButtons = new Dictionary<MenuButton, BlockType>();
            MenuButton button;

            button = _contextMenu.AddButton("Action");
            _addTypeMenuButtons.Add(button, BlockType.Action);

            button = _contextMenu.AddButton("If");
            _addTypeMenuButtons.Add(button, BlockType.If);

            button = _contextMenu.AddButton("While");
            _addTypeMenuButtons.Add(button, BlockType.While);

            button = _contextMenu.AddButton("Set");
            _addTypeMenuButtons.Add(button, BlockType.Set);
        }

        private void SetAddBlocksMenuItems()
        {
            _addBlockMenuButtons = new Dictionary<MenuButton, IAlgorithmBlock>();

            foreach (BlockData itData in _resourceProvider.BlockDatas)
            {
                if (itData.type != _addType)
                {
                    continue;
                }

                IAlgorithmBlock blockData = CreateBlockData(itData);

                MenuButton button = _contextMenu.AddButton(itData.name);
                _addBlockMenuButtons.Add(button, blockData);
            }
        }

        private void SetAddVariableTypeMenuItems()
        {
            _addVariableTypeMenuButtons = new Dictionary<MenuButton, ValueType>();
            MenuButton button;

            button = _contextMenu.AddButton("Boolean");
            _addVariableTypeMenuButtons.Add(button, ValueType.Bool);

            button = _contextMenu.AddButton("Number");
            _addVariableTypeMenuButtons.Add(button, ValueType.Number);
        }

        private void SetLabelTypeMenuItems()
        {
            _labelTypeMenuButtons = new Dictionary<MenuButton, ActiveLabelType>();
            MenuButton button;

            button = _contextMenu.AddButton("Constant");
            _labelTypeMenuButtons.Add(button, ActiveLabelType.Constant);

            button = _contextMenu.AddButton("Variable");
            _labelTypeMenuButtons.Add(button, ActiveLabelType.Variable);

            button = _contextMenu.AddButton("Expression");
            _labelTypeMenuButtons.Add(button, ActiveLabelType.Expression);

            if (_setLabelTarget.ValueType == ValueType.Bool)
            {
                button = _contextMenu.AddButton("Condition");
                _labelTypeMenuButtons.Add(button, ActiveLabelType.Condition);
            }

            if (_setLabelTarget.Value is IExpression || _setLabelTarget.Value is Condition)
            {
                button = _contextMenu.AddButton("Operation");
                _labelTypeMenuButtons.Add(button, ActiveLabelType.Operation);
            }
        }

        private void SetLabelVariableMenuItems()
        {
            _labelVariableMenuButtons = new Dictionary<MenuButton, IVariable>();
            MenuButton button;

            foreach (IVariable itVar in _variablesPanel.Variables)
            {
                if (itVar.Type == _setLabelTarget.ValueType)
                {
                    button = _contextMenu.AddButton(itVar.Name);
                    _labelVariableMenuButtons.Add(button, itVar);
                }
            }
        }

        private void SetVariableMenuItems()
        {
            _labelVariableMenuButtons = new Dictionary<MenuButton, IVariable>();
            MenuButton button;

            foreach (IVariable itVar in _variablesPanel.Variables)
            {
                button = _contextMenu.AddButton(itVar.Name);
                _labelVariableMenuButtons.Add(button, itVar);
            }
        }

        private void SetNumberOperationMenuItems()
        {
            _numberOperationMenuButtons = new Dictionary<MenuButton, Operations>();
            MenuButton button;

            button = _contextMenu.AddButton("+");
            _numberOperationMenuButtons.Add(button, Operations.Add);

            button = _contextMenu.AddButton("-");
            _numberOperationMenuButtons.Add(button, Operations.Substract);

            button = _contextMenu.AddButton("*");
            _numberOperationMenuButtons.Add(button, Operations.Multiple);

            button = _contextMenu.AddButton("/");
            _numberOperationMenuButtons.Add(button, Operations.Divide);
        }

        private void SetLogicOperationMenuItems()
        {
            _logicOperationMenuButtons = new Dictionary<MenuButton, LogicOperations>();
            MenuButton button;

            button = _contextMenu.AddButton("&&");
            _logicOperationMenuButtons.Add(button, LogicOperations.And);

            button = _contextMenu.AddButton("||");
            _logicOperationMenuButtons.Add(button, LogicOperations.Or);
        }

        private void SetRelationMenuItems()
        {
            _relationMenuButtons = new Dictionary<MenuButton, Relations>();
            MenuButton button;

            button = _contextMenu.AddButton("==");
            _relationMenuButtons.Add(button, Relations.Equal);

            button = _contextMenu.AddButton("<");
            _relationMenuButtons.Add(button, Relations.Less);

            button = _contextMenu.AddButton("<=");
            _relationMenuButtons.Add(button, Relations.LessEqual);

            button = _contextMenu.AddButton(">");
            _relationMenuButtons.Add(button, Relations.More);

            button = _contextMenu.AddButton(">=");
            _relationMenuButtons.Add(button, Relations.MoreEqual);

            button = _contextMenu.AddButton("!=");
            _relationMenuButtons.Add(button, Relations.NotEqual);
        }

        private IAlgorithmBlock CreateBlockData(BlockData data)
        {
            IAlgorithmBlock result = null;

            switch (data.type)
            {
                case BlockType.Begin:
                    {
                        BeginBlock block = new BeginBlock();
                        block.Data = data;
                        result = block;
                    }
                    break;

                case BlockType.Action:
                    {
                        ActionBlock block = new ActionBlock();
                        block.Data = data;
                        block.Name = data.name;
                        result = block;
                    }
                    break;

                case BlockType.If:
                    {
                        IfBlock block = new IfBlock();
                        block.Data = data;
                        result = block;
                    }
                    break;

                case BlockType.While:
                    {
                        WhileBlock block = new WhileBlock();
                        block.Data = data;
                        result = block;
                    }
                    break;

                case BlockType.Set:
                    {
                        SetBlock block = new SetBlock();
                        block.Data = data;
                        result = block;
                    }
                    break;
            }

            return result;
        }

        private void AddInit()
        {
            _addBlockStep = AddBlockSteps.SetTarget;
            _addVariableStep = AddVariableSteps.SetType;
        }

        private void MoveInit()
        {

        }

        private void DeleteInit()
        {

        }

        private void ClearContextMenu()
        {
            MenuButton[] menuItems = _contextMenu.GetComponentsInChildren<MenuButton>();
            foreach (MenuButton item in menuItems)
            {
                Destroy(item.gameObject);
            }
        }
    }
}
