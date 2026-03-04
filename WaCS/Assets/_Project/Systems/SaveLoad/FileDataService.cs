using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

namespace Project.Systems.SaveLoad
{
    public class FileDataService : IDataService
    {
        private ISerializer _serializer;
        private string _dataPath;
        private string _fileExtension;

        public FileDataService(ISerializer serializer)
        {
            _serializer = serializer;
            _dataPath = Application.persistentDataPath; // Use Unity's persistent data path for saving files
            _fileExtension = "json"; // Use JSON file extension for saved data
        }

        public void Save(GameData data, bool overwrite = true)
        {
            string fileLocation = GetPathToFile(data.Name);
            if(!overwrite && File.Exists(fileLocation))
            {
                throw new Exception($"A save file with the name '{data.Name}' already exists. " +
                    $"Set overwrite to true to overwrite it.");
            }

            try
            {
                File.WriteAllText(fileLocation, _serializer.Serialize(data));
            }
            catch
            {
                throw new Exception($"Failed to save game data with the name '{data.Name}'.");
            }
        }

        public GameData Load(string name)
        {
            string fileLocation = GetPathToFile(name);
            if (!File.Exists(fileLocation))
            {
                throw new Exception($"No save file found with the name '{name}'.");
            }
            return _serializer.Deserialize<GameData>(File.ReadAllText(fileLocation));
        }

        public void Delete(string name)
        {
            string fileLocation = GetPathToFile(name);
            if (!File.Exists(fileLocation))
            {
                throw new Exception($"No save file found with the name '{name}' to delete.");
            }
            File.Delete(fileLocation);
        }

        public void DeleteAll()
        {
            foreach(string filePath in Directory.GetFiles(_dataPath))
            {
                if(Path.GetExtension(filePath) == _fileExtension)
                {
                    File.Delete(filePath);
                }
            }
        }

        public IEnumerable<string> ListSaves()
        {
            foreach(var path in Directory.EnumerateFiles(_dataPath))
            {
                if (Path.GetExtension(path) == _fileExtension)
                {
                    yield return Path.GetFileNameWithoutExtension(path);
                }
            }
        }

        // Helper method to get the full file path for a given save name
        private string GetPathToFile(string fileName)
        {
            return Path.Combine(_dataPath, string.Concat(fileName, ".", _fileExtension));
        }
    }
}
