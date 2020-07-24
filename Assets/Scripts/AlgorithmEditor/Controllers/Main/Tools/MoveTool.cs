using AlgorithmizmModels.Blocks;

namespace Algorithmizm
{
    public class MoveTool
    {
        private EditPanel _editPanel;
        private TreePanel _treePanel;

        private AlgorithmBlockUI _selectedBlock;

        public MoveTool(EditPanel editPanel, TreePanel treePanel)
        {
            _editPanel = editPanel;
            _treePanel = treePanel;
        }

        public void Init()
        {
            _selectedBlock = null;
        }

        public void OnBlockClick(AlgorithmBlockUI sender)
        {
            if (_selectedBlock == null && sender.BlockData.Type != BlockType.Begin)
            {
                _selectedBlock = sender;
            }
            else if (_selectedBlock != null)
            {
                _treePanel.MoveBlock(_selectedBlock, sender);
                _editPanel.CurrentTool = EditTools.Cursor;
            }
        }
    }
}
