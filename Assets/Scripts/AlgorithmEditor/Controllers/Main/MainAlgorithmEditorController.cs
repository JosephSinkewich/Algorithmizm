﻿using Algorithmizm;
using AlgorithmizmModels.Blocks;
using AlgorithmizmModels.Math;
using AlgorithmizmModels.Variables;
using Assets.Scripts.AlgorithmEditor.Controllers.Blocks;
using Assets.Scripts.AlgorithmEditor.Controllers.ContextMenu;
using Assets.Scripts.AlgorithmEditor.Controllers.Panels;
using Assets.Scripts.AlgorithmEditor.Controllers.ResourceProviders;
using Assets.Scripts.AlgorithmEditor.Model;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AlgorithmEditor.Controllers.Main
{
    public class MainAlgorithmEditorController : MonoBehaviour
    {
        //mock
        private List<IVariable> _vars = new List<IVariable>()
        {
            new FloatVariable() { Name = "float1", Value = 7f},
            new FloatVariable() { Name = "float2", Value = 2f},
            new FloatVariable() { Name = "float3", Value = 2.3f},
            new BoolVariable() { Name = "bool1", IsTrue = false},
            new BoolVariable() { Name = "bool2", IsTrue = true},
            new BoolVariable() { Name = "bool3", IsTrue = true},
        };

        [SerializeField] private EditPanel _editPanel;
        [SerializeField] private TreePanel _treePanel;
        [SerializeField] private ContextMenuController _contextMenu;

        [SerializeField] private AlgorithmTreeResourceProvider _resourceProvider;

        private AddSteps _addSteps;
        private AlgorithmBlockUI _addTarget;
        private bool _addInsertInside;
        private BlockType _addType;
        private Dictionary<MenuButton, bool> _addInsertTypeMenuButtons;
        private Dictionary<MenuButton, BlockType> _addTypeMenuButtons;
        private Dictionary<MenuButton, IAlgorithmBlock> _addBlockMenuButtons;
        private IAlgorithmBlock _addBlockData;

        private SetLabelSteps _setLabelSteps;
        private ActiveLabel _setLabelTarget;
        private ValueUI _setLabelValueUi;
        private ActiveLabelType _activeLabelType;
        private Dictionary<MenuButton, ActiveLabelType> _labelTypeMenuButtons;
        private IVariable _activeLabelVariable;
        private Dictionary<MenuButton, IVariable> _labelVariableMenuButtons;

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
            _contextMenu.OnButtonClick.AddListener(ContextMenuItemClickHandler);
        }

        private void RemoveListeners()
        {
            _editPanel.OnToolChanged.RemoveListener(ToolChangedHandler);
            _treePanel.OnBlockClick.RemoveListener(BlockClickHandler);
            _treePanel.OnLabelClick.RemoveListener(LabelClickHandler);
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

        private void LabelClickHandler(ValueUI valueUi, ActiveLabel sender)
        {
            if (_editPanel.CurrentTool == EditTools.Cursor)
            {
                switch (_setLabelSteps)
                {
                    case SetLabelSteps.SetTarget:
                        {
                            _setLabelTarget = sender;
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

        private void ContextMenuItemClickHandler(MenuButton sender)
        {
            switch (_editPanel.CurrentTool)
            {
                case EditTools.Add:
                    {
                        AddOnContextMenuItem(sender);
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
            switch (_addSteps)
            {
                case AddSteps.SetTarget:
                    {
                        _addTarget = sender;

                        ClearContextMenu();
                        SetAddInsertTypeMenuItems();
                        _contextMenu.gameObject.SetActive(true);

                        _addSteps = AddSteps.ChoiseInsertType;
                    }
                    break;
            }
        }

        private void AddOnContextMenuItem(MenuButton sender)
        {
            switch (_addSteps)
            {
                case AddSteps.ChoiseInsertType:
                    {
                        if (!_addInsertTypeMenuButtons.ContainsKey(sender))
                        {
                            return;
                        }

                        _addInsertInside = _addInsertTypeMenuButtons[sender];

                        ClearContextMenu();
                        SetAddTypeMenuItems();

                        _addSteps = AddSteps.ChoiseBlockType;
                    }
                    break;

                case AddSteps.ChoiseBlockType:
                    {
                        if (!_addTypeMenuButtons.ContainsKey(sender))
                        {
                            return;
                        }

                        _addType = _addTypeMenuButtons[sender];

                        ClearContextMenu();
                        SetAddBlocksMenuItems();

                        _addSteps = AddSteps.ChoiseBlock;
                    }
                    break;

                case AddSteps.ChoiseBlock:
                    {
                        if (!_addBlockMenuButtons.ContainsKey(sender))
                        {
                            return;
                        }

                        _contextMenu.gameObject.SetActive(false);

                        _addBlockData = _addBlockMenuButtons[sender];
                        _treePanel.AddBlock(_addTarget, _addBlockData);

                        _editPanel.CurrentTool = EditTools.Cursor;

                        _addSteps = AddSteps.SetBlockData;
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
                            _setLabelValueUi.RebuildAnValue();

                            _setLabelSteps = SetLabelSteps.SetTarget;
                        }

                    }
                    break;

                case SetLabelSteps.SetValue:
                    {
                        _setLabelTarget.Value = _labelVariableMenuButtons[sender];
                        _contextMenu.gameObject.SetActive(false);

                        _setLabelValueUi.RebuildAnValue();

                        _setLabelSteps = SetLabelSteps.SetTarget;
                    }
                    break;
            }
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
        }

        private void SetLabelVariableMenuItems()
        {
            _labelVariableMenuButtons = new Dictionary<MenuButton, IVariable>();
            MenuButton button;

            foreach (IVariable itVar in _vars)
            {
                if (itVar.Type == _setLabelTarget.ValueType)
                {
                    button = _contextMenu.AddButton(itVar.Name);
                    _labelVariableMenuButtons.Add(button, itVar);
                }
            }
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
            _addSteps = AddSteps.SetTarget;
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
