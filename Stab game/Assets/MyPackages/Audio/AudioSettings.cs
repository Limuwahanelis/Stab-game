using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AudioSettings : MonoBehaviour
{
    [SerializeField] AudioChannel _masterChannel;
    [SerializeField] AudioChannel _BGMChannel;
    [SerializeField] AudioChannel _SFXChannel;
    [SerializeField] Slider _masterVolumeSlider;
    [SerializeField] Slider _BGMVolumeSlider;
    [SerializeField] Slider _sfxVolumeSlider;
    public void SetMasterVolumee(float volume) => AudioVolumes.AudioChannels[_masterChannel.ChannelNum].Value = (int)volume;
    public void SetBGMVolume(float volume) => AudioVolumes.AudioChannels[_BGMChannel.ChannelNum].Value = (int)volume;
    public void SetSfxVolume(float volume) => AudioVolumes.AudioChannels[_SFXChannel.ChannelNum].Value = (int)volume;

    private void OnEnable()
    {

        _masterVolumeSlider.value = AudioVolumes.AudioChannels[_masterChannel.ChannelNum].Value;
        _BGMVolumeSlider.value = AudioVolumes.AudioChannels[_BGMChannel.ChannelNum].Value;
        _sfxVolumeSlider.value = AudioVolumes.AudioChannels[_SFXChannel.ChannelNum].Value;
    }
}
