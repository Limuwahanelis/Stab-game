using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioSettingsData
{
    public List<AudioSChannelSetting> ChannelsData = new List<AudioSChannelSetting>();

    public AudioSettingsData(List<AudioChannel> audioChannels)
    {
        for (int i = 0; i < audioChannels.Count; i++)
        {
            ChannelsData.Add(new AudioSChannelSetting()
            {
                channel = audioChannels[i].ChannelNum,
                value = audioChannels[i].Value,
            });
        }
    }


}
[System.Serializable]
public class AudioSChannelSetting
{
    public int value;
    public int channel;
}
