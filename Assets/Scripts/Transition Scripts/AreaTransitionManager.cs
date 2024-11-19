using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTransitionManager : MonoBehaviour
{
    [Tooltip("Delay between transitions")]
    public float transitionOffset;
    public Animator anim;
    [HideInInspector]public float clipLength;

    // Start is called before the first frame update
    void Start()
    {
        AnimationClip clip = anim.runtimeAnimatorController.animationClips[0];
        clipLength = clip.length;
    }

    public void playFadeIn()
    {
        anim.Play("SwipeIn");
    }

    public void playFadeOut()
    {
        anim.Play("SwipeOut");
    }
}
