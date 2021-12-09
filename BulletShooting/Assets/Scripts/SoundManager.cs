using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; //singleton

    public AudioClip sndExplosion;
    public AudioClip bgmSound;
    public AudioClip linearBulletSound;
    private AudioSource myAudio;
    private AudioSource bgmAudio;
    private void Awake() //constructor? is it thread safe?
    {
        if (SoundManager.Instance == null)
        {
            SoundManager.Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
        bgmAudio = GetComponent<AudioSource>();
        bgmAudio.clip = bgmSound;
        bgmAudio.loop = true;
        bgmAudio.volume = 0.2f;

    }

    public void PlaySound()
    {
        myAudio.PlayOneShot(sndExplosion);
    }

    public void PlayLinearBulletSound()
    {
        myAudio.PlayOneShot(linearBulletSound);
    }
    public void PlayBGM()
    {
        bgmAudio.Play();
    }

    public void StopBGM()
    {
        bgmAudio.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
