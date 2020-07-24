using AlgorithmizmModels.Blocks;

namespace Algorithmizm
{
    public class DeleteTool
    {
        private EditPanel _editPanel;
        private TreePanel _treePanel;

        public DeleteTool(EditPanel editPanel, TreePanel treePanel)
        {
            _editPanel = editPanel;
            _treePanel = treePanel;
        }

        public void OnBlockClick(AlgorithmBlockUI sender)
        {
            if (sender.BlockData.Type != BlockType.Begin)
            {
                _treePanel.DeleteBlock(sender);
            }

            _editPanel.CurrentTool = EditTools.Cursor;
        }
    }
}
