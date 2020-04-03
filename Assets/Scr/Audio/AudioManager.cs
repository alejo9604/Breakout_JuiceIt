using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Space(20)]
    public AudioClip music;

    [Space(20)]
    public AudioClip wallHitClip;
    public AudioClip paddleHitClip;

    [Space(20)]
    public AudioClip[] brickHitClip;

    //Control var.
    private AudioSource bgAudioSource;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this) {
            Destroy(this.gameObject);
            return;
        }

        this.Init();
    }

    public void Init()
    {
        if(this.bgAudioSource == null) {
            if (!this.TryGetComponent<AudioSource>(out this.bgAudioSource))
                this.bgAudioSource = this.gameObject.AddComponent<AudioSource>();
        }

        this.bgAudioSource.playOnAwake = false;
        this.bgAudioSource.loop = true;
        this.bgAudioSource.volume = 0.1f;
        this.bgAudioSource.clip = this.music;
    }


    public void ToggleMusic(bool play)
    {
        if (play)
            this.PlayMusic();
        else
            this.StopMusic();
    }


    public void PlayMusic()
    {
        if (!this.bgAudioSource.isPlaying)
            this.bgAudioSource.Play();
    }

    public void StopMusic()
    {
        if (this.bgAudioSource.isPlaying)
            this.bgAudioSource.Stop();
    }

    public void PlayPaddleClip()
    {
        if (this.bgAudioSource == null || this.paddleHitClip == null)
            return;

        AudioSource.PlayClipAtPoint(this.paddleHitClip, Vector3.zero);
    }

    public void PlayWallClip()
    {
        if (this.bgAudioSource == null || this.wallHitClip == null)
            return;

        AudioSource.PlayClipAtPoint(this.wallHitClip, Vector3.zero);
    }

    public void PlayBrickClip(int id)
    {
        if (this.bgAudioSource == null || this.brickHitClip == null || this.brickHitClip.Length == 0)
            return;

        AudioSource.PlayClipAtPoint(this.brickHitClip[id % this.brickHitClip.Length], Vector3.zero);
    }

}
