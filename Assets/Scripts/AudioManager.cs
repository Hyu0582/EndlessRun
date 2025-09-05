using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource bgAudioSource;
    [SerializeField] private AudioSource effectAudioSource;


    [SerializeField] private AudioClip bgAudioClip;
    [SerializeField] private AudioClip runAudioClip;
    [SerializeField] private AudioClip jumpAudioClip;
    [SerializeField] private AudioClip stabAudioClip;
    [SerializeField] private AudioClip itemAudioClip;
    [SerializeField] private AudioClip selectAudioClip;
    [SerializeField] private AudioClip endAudioClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayBgMusic();
    }

    public void PlayBgMusic()
    {
        bgAudioSource.clip = bgAudioClip;
        bgAudioSource.Play();
    }

    public void PlaySfxJump()
    {
        if (effectAudioSource != null) effectAudioSource.PlayOneShot(jumpAudioClip);
    }
    public void PlaySfxStab()
    {
        if (effectAudioSource != null) effectAudioSource.PlayOneShot(stabAudioClip);
    }

    public void PlaySfxItem()
    {
        effectAudioSource.PlayOneShot(itemAudioClip);
    }
    public void PlaySfxSelect()
    {
        if (effectAudioSource != null) effectAudioSource.PlayOneShot(selectAudioClip);
    }
    public void PlaySfxEnd()
    {
        effectAudioSource.PlayOneShot(endAudioClip);
    }
}
