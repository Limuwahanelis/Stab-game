using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio Event/SingleClipDestroyEvent")]
public class DestroyAfterPlayingSimpleAudioEvent : AudioEvent
{
    [Range(0, 1)]
    private float _volume = 1f;
    [Range(0, 2)]
    private float _pitch = 1f;
    public AudioClip audioClip;
    public override void Play(AudioSource audioSource)
    {
        audioSource.clip = audioClip;
        audioSource.volume = _volume * (AudioVolumes.AudioChannels != null ? AudioVolumes.AudioChannels[_masterAudioChannel.ChannelNum].Value : _masterAudioChannel.Value) / 100.0f
            * (AudioVolumes.AudioChannels != null ? AudioVolumes.AudioChannels[_audioChannel.ChannelNum].Value : _audioChannel.Value) / 100.0f;
        audioSource.pitch = _pitch;
        if (audioSource.isPlaying) return;
        audioSource.Play();
    }
    public override void Preview(AudioSource audioSource, float masterVol, float multVol)
    {
        audioSource.clip = audioClip;
        audioSource.volume = _volume * (_masterAudioChannel.Value / 100.0f) * (_audioChannel.Value / 100.0f);
        audioSource.pitch = _pitch;
        if (audioSource.isPlaying) return;
        audioSource.Play();
    }
    protected override void Reset()
    {
        base.Reset();
        _audioChannel = Resources.Load<AudioChannel>("SFX");
    }

}
