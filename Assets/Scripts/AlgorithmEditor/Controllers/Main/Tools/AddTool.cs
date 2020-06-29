using System.Collections.Generic;
using AlgorithmizmModels.Blocks;
using AlgorithmizmModels.Math;
using AlgorithmizmModels.Variables;

namespace Algorithmizm
{
    public class AddTool
    {
        private MainAlgorithmEditorController _mainController;

        private ContextMenuController _contextMenu;
        private EditPanel _editPanel;
        private TreePanel _treePanel;
        
        private AlgorithmResourcesProvider _resourceProvider;
        
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

        public bool IsAddBlock => _isAddBlock;

        public AddTool(MainAlgorithmEditorController mainController,
            ContextMenuController contextMenu,
            EditPanel editPanel,
            TreePanel treePanel,
            AlgorithmResourcesProvider resourceProvider)
        {
            _mainController = mainController;
            _contextMenu = contextMenu;
            _editPanel = editPanel;
            _treePanel = treePanel;
            _resourceProvider = resourceProvider;
        }
        
        public void AddInit()
        {
            _addBlockStep = AddBlockSteps.SetTarget;
            _addVariableStep = AddVariableSteps.SetType;
        }
        
        public void AddOnBlock(AlgorithmBlockUI sender)
        {
            switch (_addBlockStep)
            {
                case AddBlockSteps.SetTarget:
                {
                    _addTarget = sender;
                    _isAddBlock = true;

                    _contextMenu.ClearContextMenu();

                    if (_addTarget.BlockData.Type == BlockType.If
                        || _addTarget.BlockData.Type == BlockType.While)
                    {
                        SetAddInsertTypeMenuItems();
                        _contextMenu.gameObject.SetActive(true);

                        _addBlockStep = AddBlockSteps.ChoiceInsertType;
                    }
                    else
                    {
                        _addInsertInside = false;

                        SetAddTypeMenuItems();
                        _contextMenu.gameObject.SetActive(true);

                        _addBlockStep = AddBlockSteps.ChoiceBlockType;
                    }
                }
                    break;
            }
        }
        
        public void AddOnVariablesPanel()
        {
            switch (_addBlockStep)
            {
                case AddBlockSteps.SetTarget:
                    {
                        _isAddBlock = false;

                        _contextMenu.ClearContextMenu();
                        SetAddVariableTypeMenuItems();
                        _contextMenu.gameObject.SetActive(true);

                        _addVariableStep = AddVariableSteps.SetType;
                    }
                    break;
            }
        }

        public void AddBlockOnContextMenuItem(MenuButton sender)
        {
            switch (_addBlockStep)
            {
                case AddBlockSteps.ChoiceInsertType:
                    {
                        if (!_addInsertTypeMenuButtons.ContainsKey(sender))
                        {
                            return;
                        }

                        _addInsertInside = _addInsertTypeMenuButtons[sender];

                        _contextMenu.ClearContextMenu();
                        SetAddTypeMenuItems();

                        _addBlockStep = AddBlockSteps.ChoiceBlockType;
                    }
                    break;

                case AddBlockSteps.ChoiceBlockType:
                    {
                        if (!_addTypeMenuButtons.ContainsKey(sender))
                        {
                            return;
                        }

                        _addType = _addTypeMenuButtons[sender];

                        _contextMenu.ClearContextMenu();
                        SetAddBlocksMenuItems();

                        _addBlockStep = AddBlockSteps.ChoiceBlock;
                    }
                    break;

                case AddBlockSteps.ChoiceBlock:
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

        public void AddVariableOnContextMenuItem(MenuButton sender)
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
                        _mainController.SetVariableNameFromDialog(newVariable);
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

        private void SetAddVariableTypeMenuItems()
        {
            _addVariableTypeMenuButtons = new Dictionary<MenuButton, ValueType>();
            MenuButton button;

            button = _contextMenu.AddButton("Boolean");
            _addVariableTypeMenuButtons.Add(button, ValueType.Bool);

            button = _contextMenu.AddButton("Number");
            _addVariableTypeMenuButtons.Add(button, ValueType.Number);
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
    }
}