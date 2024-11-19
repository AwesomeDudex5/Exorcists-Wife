using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum animationTypes { normal, jump, moveLeft, moveRight, moveLeftOffscreen, moveRightOffscreen }

[System.Serializable]
public class Dialogue
{
    public int characterIndex; //use for parser to pull data from characterManager
    public int spriteIndex;

    public string characterName;
    public Sprite characterSprite;
    public int spritePosition;

    public animationTypes animationType;
    public string sentences;



}
