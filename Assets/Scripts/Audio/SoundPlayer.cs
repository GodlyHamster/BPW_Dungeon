using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> _clips;

    [SerializeField]
    private GameObject _tempSoundPrefab;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Play(int clipIndex)
    {
        if (clipIndex > _clips.Count) return;
        _audioSource.PlayOneShot(_clips[clipIndex]);
    }

    public void PlayRandom()
    {
        int randomClip = Random.Range(0, _clips.Count);
        _audioSource.PlayOneShot(_clips[randomClip]);
    }

    public void PlayInstanceSound(int clipIndex)
    {
        GameObject tempSound = Instantiate(_tempSoundPrefab);
        tempSound.GetComponent<TemporarySound>().clip = _clips[clipIndex];
    }
}
