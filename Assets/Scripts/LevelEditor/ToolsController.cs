using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class ToolsController : MonoBehaviour
    {
        [SerializeField] private ToggleGroup _toggleGroup;
        
        private BaseLevelEditorTool[] _tools;
        
        public BaseLevelEditorTool SelectedTool { get; private set; }

        private void Start()
        {
            _tools = GetComponentsInChildren<BaseLevelEditorTool>();
            
            foreach (BaseLevelEditorTool itTool in _tools)
            {
                InitTool(itTool);
            }

            if (!_tools.IsNullOrEmpty())
            {
                SelectedTool = _tools[0];
                _tools[0].Toggle.isOn = true;
            }
        }

        private void InitTool(BaseLevelEditorTool tool)
        {
            tool.Toggle.group = _toggleGroup;
            tool.Toggle.onValueChanged.AddListener(value =>
            {
                if (value)
                {
                    SelectedTool = tool;
                }
            });
        }
    }
}