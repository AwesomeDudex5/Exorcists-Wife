using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioObject
{
    public AudioClip audioClip;
    public int sceneToPlayAt;
    [HideInInspector] public bool isPlaying;
    public bool playSilent;
}
