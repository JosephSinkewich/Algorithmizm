using System.Collections.Generic;
using AlgorithmizmModels.Math;
using AlgorithmizmModels.Variables;

namespace Algorithmizm
{
    public class CursorTool
    {
        private MainAlgorithmEditorController _mainController;
        
        private ContextMenuController _contextMenu;
        private VariablesPanel _variablesPanel;
        
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

        public SetLabelSteps SetLabelSteps => _setLabelSteps;
        public ActiveLabel SetLabelTarget => _setLabelTarget;
        public ValueUI SetLabelValueUi => _setLabelValueUi;

        public CursorTool(MainAlgorithmEditorController mainController,
            ContextMenuController contextMenu,
            VariablesPanel variablesPanel)
        {
            _mainController = mainController;
            _contextMenu = contextMenu;
            _variablesPanel = variablesPanel;
        }

        public void CursorOnLabel(ValueUI valueUi, ActiveLabel sender)
        {
            switch (_setLabelSteps)
            {
                case SetLabelSteps.SetTarget:
                {
                    _setLabelTarget = sender;
                    _isSetedLabelVariable = false;
                    _setLabelValueUi = valueUi;

                    _setLabelSteps = SetLabelSteps.ChoiceLabelType;
                    _contextMenu.ClearContextMenu();
                    SetLabelTypeMenuItems();
                    _contextMenu.gameObject.SetActive(true);
                }
                    break;
            }
        }

        public void CursorOnVariableLabel(SetBlockUI setBlock, ActiveLabel sender)
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
                    _contextMenu.ClearContextMenu();
                    SetVariableMenuItems();
                    _contextMenu.gameObject.SetActive(true);
                }
                    break;
            }
        }
        
        public void CursorOnContextMenuItem(MenuButton sender)
        {
            switch (_setLabelSteps)
            {
                case SetLabelSteps.ChoiceLabelType:
                    {
                        _activeLabelType = _labelTypeMenuButtons[sender];
                        _setLabelSteps = SetLabelSteps.SetValue;

                        _contextMenu.ClearContextMenu();
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
                            _mainController.SetValueFromDialog(_setLabelValueUi, _setLabelTarget.Value);

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
    }
}