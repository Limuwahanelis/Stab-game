using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEventPlayer : MonoBehaviour
{
    [SerializeField] ItemSpawner _audioSourceSpawner;
    public void PlayeAudioEvent(AudioEvent audioEvent)
    {
        audioEvent.Play(_audioSourceSpawner.GetItem().GetComponent<AudioSource>());
    }
    private void Reset()
    {
        _audioSourceSpawner = GameObject.FindGameObjectWithTag("Audio source spawner").GetComponent<ItemSpawner>();
    }
}
