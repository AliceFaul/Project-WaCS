using System.Collections.Generic;
using Project.Systems.SaveLoad;

public interface IDataService
{
    void Save(GameData data, bool overwrite = true); // Save the game data, with an option to overwrite existing data
    GameData Load(string name); // Load the game data with the specified name
    void Delete(string name); // Delete the saved game data with the specified name
    void DeleteAll(); // Delete all saved game data
    IEnumerable<string> ListSaves(); // Get a list of all saved game data names
}
