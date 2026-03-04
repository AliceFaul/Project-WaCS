using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/AudioDictionary")]
public class AudioDictionary : ScriptableObject
{
    [SerializeField] private List<AudioData> audioList;

    private Dictionary<string, AudioClip> _lookup;

    public void Init()
    {
        _lookup = new Dictionary<string, AudioClip>();

        foreach (var data in audioList)
        {
            if (!_lookup.ContainsKey(data.id))
                _lookup.Add(data.id, data.clip);
        }
    }

    public AudioClip Get(string id)
    {
        if (_lookup == null)
            Init();

        if (_lookup.TryGetValue(id, out var clip))
            return clip;

        Debug.LogWarning($"[AudioDictionary] Clip not found: {id}");
        return null;
    }
}

[System.Serializable]
public class AudioData
{
    public string id;
    public AudioClip clip;
}
