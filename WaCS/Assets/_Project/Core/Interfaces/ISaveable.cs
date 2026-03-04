using Project.Systems.SaveLoad;

public interface ISaveable
{
    void SaveState(GameData gameData);
    void LoadState(GameData gameData);
}
