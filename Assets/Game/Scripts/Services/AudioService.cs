using PrimeTween;
using UnityEngine;

public class AudioService
{
    readonly AudioSource _musicSource;
    readonly AudioSource _soundSource;

    public AudioService()
    {
        _soundSource = ComponentFactory.CreatePersistent<AudioSource>();
        _musicSource = ComponentFactory.CreatePersistent<AudioSource>();
        _musicSource.volume = .4f;
        _musicSource.loop = true;
    }

    public async void PlayMusic(AudioClip clip)
    {
        await Tween.AudioVolume(_musicSource, 0, .3f);
        _musicSource.clip = clip;
        _musicSource.Play();
        await Tween.AudioVolume(_musicSource, 1, .2f);
    }

    public async void StopMusic()
    {
        await Tween.AudioVolume(_musicSource, 0, .2f);
        _musicSource.Stop();
    }

    public void PlayClip(AudioClip clip)
    {
        _soundSource.PlayOneShot(clip, 0.7f);
    }
}