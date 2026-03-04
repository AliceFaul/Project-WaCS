using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsUI : MonoBehaviour
{
    public Slider volumeSlider;
    public TMP_Dropdown fpsDropdown;
    public TMP_Dropdown resolutionDropdown;

    void Start()
    {
        volumeSlider.onValueChanged.AddListener(SetVolume);
        fpsDropdown.onValueChanged.AddListener(SetFPS);
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    void SetVolume(float v)
    {
        GameSettingsManager.Instance.SetVolume(v);
    }

    void SetFPS(int index)
    {
        if (index == 0) GameSettingsManager.Instance.SetFPS(30);
        if (index == 1) GameSettingsManager.Instance.SetFPS(60);
        if (index == 2) GameSettingsManager.Instance.SetFPS(120);
    }

    void SetResolution(int index)
    {
        if (index == 0) GameSettingsManager.Instance.SetResolution(1920, 1080);
        if (index == 1) GameSettingsManager.Instance.SetResolution(1280, 720);
        if (index == 2) GameSettingsManager.Instance.SetResolution(800, 600);
    }
}