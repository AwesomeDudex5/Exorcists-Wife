using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    GameManager gm;
    public AudioObject[] listOfAudios;
    AudioSource audioSrc;
    int currentFlag;
    bool playingAudio;

    // Start is called before the first frame update
    void Start()
    {
        gm = this.GetComponent<GameManager>();
        audioSrc = this.GetComponent<AudioSource>();
        playingAudio = false;
    }

    // Update is called once per frame
    void Update()
    {
        getCurrentFlag();
        playAudio();

    }

    void getCurrentFlag()
    {
        currentFlag = gm.currentFlag;
    }

    void playAudio()
    {
        foreach (AudioObject ao in listOfAudios)
        {
            if (ao.sceneToPlayAt == currentFlag && !ao.isPlaying)
            {

                audioSrc.clip = ao.audioClip;
                audioSrc.Play();
                ao.isPlaying = true;
                playingAudio = true;

                if (ao.playSilent == true && playingAudio)
                {

                    playingAudio = false;
                    audioSrc.Stop();
                }

            }

        }
    }
}
