using UnityEngine;

public class GameSettingsManager : MonoBehaviour
{
    public static GameSettingsManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings();
        }
        else Destroy(gameObject);
    }

    public void SetVolume(float v)
    {
        AudioListener.volume = v;
        PlayerPrefs.SetFloat("volume", v);
    }

    public void SetFPS(int fps)
    {
        Application.targetFrameRate = fps;
        PlayerPrefs.SetInt("fps", fps);
    }

    public void SetResolution(int w, int h)
    {
        Screen.SetResolution(w, h, true);
        PlayerPrefs.SetInt("width", w);
        PlayerPrefs.SetInt("height", h);
    }

    void LoadSettings()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("volume", 1f);
        Application.targetFrameRate = PlayerPrefs.GetInt("fps", 60);

        int w = PlayerPrefs.GetInt("width", Screen.currentResolution.width);
        int h = PlayerPrefs.GetInt("height", Screen.currentResolution.height);

        Screen.SetResolution(w, h, true);
    }
}