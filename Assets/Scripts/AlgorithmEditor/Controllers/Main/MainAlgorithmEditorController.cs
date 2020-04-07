﻿using AlgorithmizmModels.Blocks;
using Assets.Scripts.AlgorithmEditor.Controllers.Blocks;
using Assets.Scripts.AlgorithmEditor.Controllers.ContextMenu;
using Assets.Scripts.AlgorithmEditor.Controllers.Panels;
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

                        _addBlockData = _addBlockMenuButtons[sender];

                        _contextMenu.gameObject.SetActive(false);

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
            MenuButton button;

            button = _contextMenu.AddButton("ActionBlock1");
            _addBlockMenuButtons.Add(button, null);
            button = _contextMenu.AddButton("ActionBlock2");
            _addBlockMenuButtons.Add(button, null);
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