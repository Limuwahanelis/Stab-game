using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Audio Event/SingleClipEvent")]
public class SingleClipAudioEvent : AudioEvent
{
    public AudioClip audioClip;
    [Range(0, 1), SerializeField]
    private float _volume = 1f;
    [Range(0, 2), SerializeField]
    private float _pitch = 1f;
    public override void Play(AudioSource audioSource)
    {
        audioSource.clip = audioClip;
        audioSource.volume = _volume * (AudioVolumes.AudioChannels != null ? AudioVolumes.AudioChannels[_masterAudioChannel.ChannelNum].Value : _masterAudioChannel.Value) / 100.0f
            * (AudioVolumes.AudioChannels != null ? AudioVolumes.AudioChannels[_audioChannel.ChannelNum].Value : _audioChannel.Value) / 100.0f;
        audioSource.pitch = _pitch;
        if (audioSource.isPlaying) return;
        audioSource.Play();
    }
    public override void Play(AudioSource audioSource, bool overPlay = false)
    {
        audioSource.clip = audioClip;
        audioSource.volume = _volume * (AudioVolumes.AudioChannels != null ? AudioVolumes.AudioChannels[_masterAudioChannel.ChannelNum].Value : _masterAudioChannel.Value) / 100.0f
            * (AudioVolumes.AudioChannels != null ? AudioVolumes.AudioChannels[_audioChannel.ChannelNum].Value : _audioChannel.Value) / 100.0f;
        audioSource.pitch = _pitch;
        if (!overPlay) return;
        audioSource.Play();
    }
    public override void Pause(AudioSource audioSource)
    {
        audioSource.Stop();
    }
    public override void Preview(AudioSource audioSource, float masterVol, float multVol)
    {
        audioSource.clip = audioClip;
        audioSource.volume = _volume * (_masterAudioChannel.Value / 100.0f) * (_audioChannel.Value / 100.0f);
        audioSource.pitch = _pitch;
        //if (audioSource.isPlaying) return;
        audioSource.Play();
    }
    protected override void Reset()
    {
        base.Reset();
        _audioChannel = Resources.Load<AudioChannel>($"{ScriptPaths.ResourcesChannelsPath}/SFX");
    }
}
