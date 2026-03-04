using UnityEngine;
using System.Threading.Tasks;

[RequireComponent(typeof(AudioSource))]
public class AudioService : MonoBehaviour, IAudioService
{
    private AudioSource _bgmSource;
    private AudioSource _sfxSource;
    private AudioDictionary _audioDictionary;

    public void SetDictionary(AudioDictionary dictionary)
    {
        _audioDictionary = dictionary;
        _audioDictionary.Init();
    }

    public async Task<bool> InitAsync()
    {
        DontDestroyOnLoad(gameObject);

        _bgmSource = GetComponent<AudioSource>();
        _sfxSource = GetComponent<AudioSource>();

        _bgmSource.loop = true;
        _bgmSource.playOnAwake = false;

        _sfxSource.loop = false;
        _sfxSource.playOnAwake = false;

        await Task.CompletedTask;
        return true;
    }

    public void PlayBGM(string id, bool loop = true)
    {
        var clip = _audioDictionary.Get(id);
        if (clip == null) return;

        _bgmSource.clip = clip;
        _bgmSource.Play();
    }

    public void StopBGM()
    {
        _bgmSource.Stop();
    }

    public void PlaySFX(string id)
    {
        var clip = _audioDictionary.Get(id);
        if (clip == null) return;

        _sfxSource.PlayOneShot(clip);
    }

    public void SetBGMVolume(float volume)
    {
        _bgmSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        _sfxSource.volume = volume;
    }
}