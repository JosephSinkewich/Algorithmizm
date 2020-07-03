using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlgorithmizmModels.Level;
using UnityEngine;

namespace LevelManagement
{
    public static class LevelSaveLoader
    {
        private const string  LEVEL_LIST_KEY = "levelList";
        private const string  LEVEL_NAME_PREFIX = "_level_";

        public static string[] GetLevelList()
        {
            return PlayerPrefs.GetString(LEVEL_LIST_KEY).Split();
        }

        public static void SaveLevel(Level levelDesign)
        {
            List<string> levels = GetLevelList().ToList();
            if (!levels.Contains(levelDesign.name))
            {
                levels.Add(levelDesign.name);
            }

            PlayerPrefs.SetString(LEVEL_LIST_KEY, CreateLevelListString(levels));
            
            string levelJson = JsonUtility.ToJson(levelDesign);
            PlayerPrefs.SetString($"{LEVEL_NAME_PREFIX}{levelDesign.name}", levelJson);
            
            PlayerPrefs.Save();
        }

        public static Level LoadLevel(string levelName)
        {
            string levelJson = PlayerPrefs.GetString($"{LEVEL_NAME_PREFIX}{levelName}");

            return JsonUtility.FromJson<Level>(levelJson);
        }

        private static string CreateLevelListString(List<string> list)
        {
            StringBuilder result = new StringBuilder();

            foreach (string itLevelName in list)
            {
                result.Append($"{itLevelName} ");
            }

            return result.ToString();
        }
    }
}