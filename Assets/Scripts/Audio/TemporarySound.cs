using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TemporarySound : MonoBehaviour
{
    public AudioClip clip;

    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(PlayClipAndDestroy());
    }

    private IEnumerator PlayClipAndDestroy()
    {
        _source.clip = clip;
        _source.Play();
        yield return new WaitUntil(() => _source.time >= _source.clip.length);
        Destroy(gameObject);
    }
}
