using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSetUp : MonoBehaviour
{
    private void Awake()
    {
        AudioSettingsData audioData;
        if (AudioSettingsSaver.LoadAudioSettings() == null)
        {
            for (int i = 0; i < AudioVolumes.AudioChannels.Count; i++)
            {
                AudioVolumes.AudioChannels[i].Value = 50;
            }
            audioData = new AudioSettingsData(AudioVolumes.AudioChannels);

            AudioSettingsSaver.SaveAudioSettings(audioData);
        }
        else
        {
            audioData = AudioSettingsSaver.LoadAudioSettings();
        }
        for (int i = 0; i < audioData.ChannelsData.Count; i++)
        {
            AudioVolumes.AudioChannels[audioData.ChannelsData[i].channel].Value = audioData.ChannelsData[i].value;
        }
    }
}
