using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMPlayer : MonoBehaviour
{
    [SerializeField] List<BGMAudioEvent> _bgms;
    [SerializeField] AudioChannel _bgmChannel;
    [SerializeField] AudioChannel _masterChannel;
    [SerializeField] AudioSource _source;
    int _startingIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioVolumes.AudioChannels.Find(x => x.ChannelNum == _bgmChannel.ChannelNum).OnValueChanged += UpdateVolume;
        AudioVolumes.AudioChannels.Find(x => x.ChannelNum == _masterChannel.ChannelNum).OnValueChanged += UpdateVolume;
        _startingIndex = Random.Range(0, _bgms.Count);
        _bgms[_startingIndex].Play(_source);
    }
    private void UpdateVolume(int bgmVolume)
    {
        _bgms[_startingIndex].Play(_source);
    }
    private void Update()
    {
        if (!Application.isFocused) return;
        if (!_source.isPlaying)
        {
            _startingIndex++;
            if (_startingIndex >= _bgms.Count)
            {
                _startingIndex = 0;
            }
            _bgms[_startingIndex].Play(_source);
        }
    }
    private void Reset()
    {
        _source=GetComponent<AudioSource>();
        _bgmChannel = Resources.Load<AudioChannel>($"{ScriptPaths.ResourcesChannelsPath}/BGM");
        _masterChannel = Resources.Load<AudioChannel>($"{ScriptPaths.ResourcesChannelsPath}/Master");
    }
    private void OnDestroy()
    {
        AudioVolumes.AudioChannels.Find(x => x.ChannelNum == _bgmChannel.ChannelNum).OnValueChanged -= UpdateVolume;
        AudioVolumes.AudioChannels.Find(x => x.ChannelNum == _masterChannel.ChannelNum).OnValueChanged -= UpdateVolume;
    }
}
