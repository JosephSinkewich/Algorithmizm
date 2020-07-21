using AlgorithmizmModels.Level;
using LevelManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class LevelEditorUIController : MonoBehaviour
    {
        [SerializeField] private LevelEditorAssistant _levelEditorAssistant;

        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private TMP_InputField _levelNameInput;

        private void Start()
        {
            _saveButton.onClick.AddListener(SaveLevel);
            _loadButton.onClick.AddListener(LoadLevel);
        }

        private void OnDestroy()
        {
            _saveButton.onClick.RemoveListener(SaveLevel);
            _loadButton.onClick.RemoveListener(LoadLevel);
        }

        private void SaveLevel()
        {
            string levelName = _levelNameInput.text;
            Level level = _levelEditorAssistant.GenerateLevel(levelName);
            
            LevelSaveLoader.SaveLevel(level);
        }
        
        private void LoadLevel()
        {
            string levelName = _levelNameInput.text;
            Level level = LevelSaveLoader.LoadLevel(levelName);

            _levelEditorAssistant.InstantiateLevel(level);
        }
    }
}