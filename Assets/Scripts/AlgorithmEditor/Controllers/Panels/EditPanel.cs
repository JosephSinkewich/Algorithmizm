using Assets.Scripts.AlgorithmEditor.Model;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Assets.Scripts.AlgorithmEditor.Events;

namespace Assets.Scripts.AlgorithmEditor.Controllers.Panels
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
                _currentTool = value;
                OnToolChanged?.Invoke(_currentTool);
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
                CurrentTool = EditTools.Add;
            }
        }

        private void SetMove(bool value)
        {
            if (value)
            {
                CurrentTool = EditTools.Move;
            }
        }


        private void SetDelete(bool value)
        {
            if (value)
            {
                CurrentTool = EditTools.Delete;
            }
        }


        private void SetCursor(bool value)
        {
            if (value)
            {
                CurrentTool = EditTools.Cursor;
            }
        }
    }
}
