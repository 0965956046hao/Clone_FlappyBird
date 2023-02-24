using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource[] sfx;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlaySFX(int sfxToPlay)
    {
        sfx[sfxToPlay].Stop();
        sfx[sfxToPlay].Play();
    }
    public void FlySFX()
    {
        PlaySFX(1);
    }
    public void GetScoreSFX()
    {
        PlaySFX(0);
    }
    public void HitPipesSFX()
    {
        PlaySFX(2);
    }

    public void SwooshingSFX()
    {
        PlaySFX(3);
    }
}
