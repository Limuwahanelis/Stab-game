using SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettingsSaver : MonoBehaviour
{
    public static readonly string audioSettingsFileName = "audioConfigs";
    public void SaveAudioSettings()
    {
        AudioSettingsData audioData = new AudioSettingsData(AudioVolumes.AudioChannels);
        JsonSave.SaveToFile(audioData,JsonSave.gameConfigsFolderPath ,audioSettingsFileName);
    }
    public static AudioSettingsData LoadAudioSettings()
    {
        return JsonSave.GetDataFromJson<AudioSettingsData>(JsonSave.gameConfigsFolderPath, audioSettingsFileName);
    }
    public static void SaveAudioSettings(AudioSettingsData screenData)
    {
        JsonSave.SaveToFile(screenData, JsonSave.gameConfigsFolderPath, audioSettingsFileName);
    }
}
