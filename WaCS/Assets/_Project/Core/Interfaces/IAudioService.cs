using UnityEngine;

public interface IAudioService : IManager
{
    void PlayBGM(string id, bool loop = true);
    void StopBGM();
    void PlaySFX(string id);
    void SetBGMVolume(float volume);
    void SetSFXVolume(float volume);
}
