using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class AudioVolumes:MonoBehaviour
{
    public static List<AudioChannel> AudioChannels
    {
        get
        {
            if (_audioChannels != null) return _audioChannels;
            else
            {
                _audioChannels = Resources.LoadAll<AudioChannel>($"{ScriptPaths.ResourcesChannelsPath}/").ToList();
                _audioChannels.Sort((c1, c2) => c1.ChannelNum.CompareTo(c2.ChannelNum));
            }
            return _audioChannels;
        }
    }

    [SerializeField] List<AudioChannel> _audioChannelsReferences = new List<AudioChannel>();

    private static List<AudioChannel> _audioChannels;


    private void Awake()
    {
        _audioChannels = _audioChannelsReferences;
        _audioChannels.Sort((c1, c2) => c1.ChannelNum.CompareTo(c2.ChannelNum));
    }
    private void Reset()
    {
        _audioChannelsReferences = Resources.LoadAll<AudioChannel>($"{ScriptPaths.ResourcesChannelsPath}/").ToList();
        _audioChannelsReferences.Sort((c1, c2) => c1.ChannelNum.CompareTo(c2.ChannelNum));
    }
}
