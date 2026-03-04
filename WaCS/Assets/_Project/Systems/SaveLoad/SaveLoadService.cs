using _Project.Core.Singleton;
using UnityEngine;
using System.Collections.Generic;
using System;
using _Project.Gameplay.Player;
using _Project.Systems.Game;

namespace Project.Systems.SaveLoad
{
    [System.Serializable]
    public class GameData
    {
        public string Name;
        public long LastSaveTime;
        public PlayerData Player;
        public EconomySaveData Economy;
        public List<ShelfSaveData> Shelves = new();
    }

    public class SaveLoadService : PersistentSingleton<SaveLoadService>
    {
        private IDataService _dataService;
        private readonly List<ISaveable> _saveables = new();

        public GameData CurrentGameData { get; private set; }

        private bool _hasLoadedGame;

        protected override void Awake()
        {
            base.Awake();
            _dataService = new FileDataService(new JsonSerializer());
        }

        #region Registration

        public void RegisterSaveData(ISaveable saveable)
        {
            if (saveable == null)
                return;

            if (!_saveables.Contains(saveable))
                _saveables.Add(saveable);

            if (_hasLoadedGame && CurrentGameData != null)
            {
                saveable.LoadState(CurrentGameData);
            }
        }

        #endregion

        #region Game Flow

        public void NewGame()
        {
            CurrentGameData = new GameData
            {
                Name = $"DefaultSave",
                LastSaveTime = DateTimeOffset.UtcNow.Ticks
            };
            _hasLoadedGame = true;
        }

        public void SaveGame()
        {
            if (CurrentGameData == null)
            {
                Debug.LogWarning("No game data found. Creating new game data.");
                NewGame();
            }

            CurrentGameData.LastSaveTime = DateTimeOffset.UtcNow.Ticks;
            foreach (var saveable in _saveables)
            {
                saveable.SaveState(CurrentGameData);
            }
            _dataService.Save(CurrentGameData);
            Debug.Log("Game saved successfully.");
        }

        public void LoadGame(string name)
        {
            var loadedData = _dataService.Load(name);
            if (loadedData == null)
            {
                Debug.LogWarning("Save file not found. Starting new game.");
                NewGame();
                return;
            }
            CurrentGameData = loadedData;
            _hasLoadedGame = true;

            ApplyLoadedData();
        }

        private void ApplyLoadedData()
        {
            if (CurrentGameData == null)
                return;

            foreach (var saveable in _saveables)
            {
                saveable.LoadState(CurrentGameData);
            }

            Debug.Log("Save data applied to all registered systems.");
        }

        public void DeleteGame(string gameName)
        {
            _dataService.Delete(gameName);
        }

        #endregion
    }
}