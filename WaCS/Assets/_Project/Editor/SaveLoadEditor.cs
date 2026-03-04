using UnityEngine;
using Project.Systems.SaveLoad;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor(typeof(SaveLoadService))]
public class SaveLoadEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SaveLoadService service = (SaveLoadService)target;

        DrawDefaultInspector();

        if(GUILayout.Button("Save Game"))
        {
            service.SaveGame();
        }

        if(GUILayout.Button("Load Game"))
        {
            service.LoadGame(gameName);
        }

        if (GUILayout.Button("Delete Game"))
        {
            service.DeleteGame(gameName);
        }
    }
}
