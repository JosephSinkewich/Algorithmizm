using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class ToolsController : MonoBehaviour
    {
        [SerializeField] private ToggleGroup _toggleGroup;
        
        private LevelEditorTool[] _tools;
        
        public LevelEditorTool SelectedTool { get; private set; }

        private void Start()
        {
            _tools = GetComponentsInChildren<LevelEditorTool>();
            
            foreach (LevelEditorTool itTool in _tools)
            {
                InitTool(itTool);
            }

            if (!_tools.IsNullOrEmpty())
            {
                SelectedTool = _tools[0];
                _tools[0].Toggle.isOn = true;
            }
        }

        private void InitTool(LevelEditorTool tool)
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