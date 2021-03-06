﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Algorithmizm
{
    public class ContextMenuController : MonoBehaviour
    {
        [SerializeField] private MenuButton _menuButtonPrefab;

        private List<MenuButton> _buttons = new List<MenuButton>();
        public IReadOnlyCollection<MenuButton> Buttons => _buttons;

        public UnityEvent<MenuButton> OnButtonClick { get; set; } =
            new MenuButtonEvent();

        public MenuButton AddButton(string buttonText)
        {
            MenuButton result = Instantiate(_menuButtonPrefab, transform)
                .GetComponent<MenuButton>();
            result.Text = buttonText;

            _buttons.Add(result);

            RefreshButtonsListeners();

            return result;
        }
        
        public void ClearContextMenu()
        {
            MenuButton[] menuItems = GetComponentsInChildren<MenuButton>();
            foreach (MenuButton item in menuItems)
            {
                Destroy(item.gameObject);
            }
        }

        private void OnDestroy()
        {
            foreach (MenuButton itButton in _buttons)
            {
                itButton.OnClick.RemoveListener(MenuButtonClickHandler);
            }
        }

        private void RefreshButtonsListeners()
        {
            foreach (MenuButton itButton in _buttons)
            {
                itButton.OnClick.RemoveListener(MenuButtonClickHandler);
            }
            foreach (MenuButton itButton in _buttons)
            {
                itButton.OnClick.AddListener(MenuButtonClickHandler);
            }
        }

        private void MenuButtonClickHandler(MenuButton button)
        {
            OnButtonClick?.Invoke(button);
        }
    }
}
