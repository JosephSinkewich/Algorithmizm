using AlgorithmizmModels.Blocks;
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
            _contextMenu.OnButtonClick.AddListener(ContextMenuItemClickHandler);
        }

        private void RemoveListeners()
        {
            _editPanel.OnToolChanged.RemoveListener(ToolChangedHandler);
            _treePanel.OnBlockClick.RemoveListener(BlockClickHandler);
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

        private void ContextMenuItemClickHandler(MenuButton sender)
        {
            switch (_editPanel.CurrentTool)
            {
                case EditTools.Add:
                    {
                        AddOnContextMenuItem(sender);
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

                        _addSteps = AddSteps.ChiseInsertType;
                    }
                    break;
            }
        }

        private void AddOnContextMenuItem(MenuButton sender)
        {
            switch (_addSteps)
            {
                case AddSteps.ChiseInsertType:
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
