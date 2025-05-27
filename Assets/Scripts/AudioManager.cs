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

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayBgMusic()
    {
        bgAudioSource.clip = bgAudioClip;
        bgAudioSource.Play();
    }

    public void PlaySfxJump()
    {
        effectAudioSource.PlayOneShot(jumpAudioClip);
    }
    public void PlaySfxStab()
    {
        effectAudioSource.PlayOneShot(stabAudioClip);
    }

    public void PlaySfxItem()
    {
        effectAudioSource.PlayOneShot(itemAudioClip);
    }
    public void PlaySfxSelect()
    {
        effectAudioSource.PlayOneShot(selectAudioClip);
    }
    public void PlaySfxEnd()
    {
        effectAudioSource.PlayOneShot(endAudioClip);
    }
}
