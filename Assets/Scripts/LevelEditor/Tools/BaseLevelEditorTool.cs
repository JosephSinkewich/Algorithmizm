using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public abstract class BaseLevelEditorTool : MonoBehaviour
    {
        [SerializeField] private Toggle _toggle;

        [SerializeField] private ToolsController _toolsController;
        [SerializeField] private LevelEditorDrawController _drawController;

        public Toggle Toggle => _toggle;
        
        protected ToolsController ToolsController => _toolsController;

        protected bool IsSelected => ToolsController.SelectedTool == this;

        private void Start()
        {
            _drawController.OnMouseButtonDown += OnMouseButtonDown;
            _drawController.OnMouseButtonPress += OnMouseButtonPress;
            _drawController.OnMouseButtonUp += OnMouseButtonUp;
        }

        private void OnDestroy()
        {
            _drawController.OnMouseButtonDown -= OnMouseButtonDown;
            _drawController.OnMouseButtonPress -= OnMouseButtonPress;
            _drawController.OnMouseButtonUp -= OnMouseButtonUp;
        }

        protected virtual void OnMouseButtonDown(MouseButtonEventData eventData)
        {
        }

        protected virtual void OnMouseButtonPress(MouseButtonEventData eventData)
        {
        }
        protected virtual void OnMouseButtonUp(MouseButtonEventData eventData)
        {
        }
    }
}