using Project.Systems.SaveLoad;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SaveLoadWindow : EditorWindow
{
    private string _saveName = "DefaultSave";

    [MenuItem("Supermarket/Save Load Debug")]
    public static void ShowWindow()
    {
        GetWindow<SaveLoadWindow>("Save Load Debug");
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        GUILayout.Label("SAVE LOAD DEBUG PANEL", EditorStyles.boldLabel);

        GUILayout.Space(10);

        _saveName = EditorGUILayout.TextField("Save Name", _saveName);

        GUILayout.Space(15);

        GUI.enabled = Application.isPlaying;

        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Enter Play Mode to use Save/Load.", MessageType.Info);
        }

        GUILayout.Space(5);

        if (GUILayout.Button("New Game", GUILayout.Height(30)))
        {
            SaveLoadService.Instance.NewGame();
            Debug.Log("New Game Created.");
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Save Game", GUILayout.Height(40)))
        {
            SaveLoadService.Instance.SaveGame();
            Debug.Log("Game Saved.");
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Load Game", GUILayout.Height(40)))
        {
            SaveLoadService.Instance.LoadGame(_saveName);
            Debug.Log("Game Loaded.");
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Delete Save", GUILayout.Height(30)))
        {
            SaveLoadService.Instance.DeleteGame(_saveName);
            Debug.Log("Save Deleted.");
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Print Current GameData"))
        {
            var data = SaveLoadService.Instance.CurrentGameData;
            if (data == null)
            {
                Debug.Log("No Current GameData.");
            }
            else
            {
                Debug.Log(JsonUtility.ToJson(data, true));
            }
        }

        GUI.enabled = true;
    }
}

