using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Algorithmizm
{
    public class EditPanel : MonoBehaviour
    {
        [SerializeField] private Toggle _addToggle;
        [SerializeField] private Toggle _moveToggle;
        [SerializeField] private Toggle _deleteToggle;
        [SerializeField] private Toggle _cursorToggle;

        private EditTools _currentTool;

        public EditTools CurrentTool
        {
            get
            {
                return _currentTool;
            }
            set
            {
                switch (value)
                {
                    case EditTools.Add:
                        {
                            _addToggle.isOn = true;
                        }
                        break;
                    case EditTools.Move:
                        {
                            _moveToggle.isOn = true;
                        }
                        break;
                    case EditTools.Delete:
                        {
                            _deleteToggle.isOn = true;
                        }
                        break;
                    case EditTools.Cursor:
                        {
                            _cursorToggle.isOn = true;
                        }
                        break;
                }
            }
        }

        public UnityEvent<EditTools> OnToolChanged { get; set; } = new EditToolsEvent();

        private void Start()
        {
            AddListeners();

            _currentTool = EditTools.Cursor;
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            _addToggle.onValueChanged.AddListener(SetAdd);
            _moveToggle.onValueChanged.AddListener(SetMove);
            _deleteToggle.onValueChanged.AddListener(SetDelete);
            _cursorToggle.onValueChanged.AddListener(SetCursor);
        }

        private void RemoveListeners()
        {
            _addToggle.onValueChanged.RemoveListener(SetAdd);
            _moveToggle.onValueChanged.RemoveListener(SetMove);
            _deleteToggle.onValueChanged.RemoveListener(SetDelete);
            _cursorToggle.onValueChanged.RemoveListener(SetCursor);
        }

        private void SetAdd(bool value)
        {
            if (value)
            {
                _currentTool = EditTools.Add;
                OnToolChanged?.Invoke(_currentTool);
            }
        }

        private void SetMove(bool value)
        {
            if (value)
            {
                _currentTool = EditTools.Move;
                OnToolChanged?.Invoke(_currentTool);
            }
        }


        private void SetDelete(bool value)
        {
            if (value)
            {
                _currentTool = EditTools.Delete;
                OnToolChanged?.Invoke(_currentTool);
            }
        }


        private void SetCursor(bool value)
        {
            if (value)
            {
                _currentTool = EditTools.Cursor;
                OnToolChanged?.Invoke(_currentTool);
            }
        }
    }
}
