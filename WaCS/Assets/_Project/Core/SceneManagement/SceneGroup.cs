using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class SceneGroup
{
    public string groupName = "New Scene Group";
    public int buildIndex;
    public List<SceneData> scenes;
    
    // tìm scene bằng type
    public string FindSceneNameByType(SceneType sceneType)
    {
        return scenes.FirstOrDefault(s => s.sceneType == sceneType)
            ?.sceneReference.SceneName;
    }
}

[System.Serializable]
public class SceneData
{
    public SceneField sceneReference;
    public string SceneName =>  sceneReference.SceneName;
    public SceneType sceneType;
}

public enum SceneType
{
    ActiveScene,
    MainMenu,
    UserInterface,
    Cinematic,
    Environment,
}