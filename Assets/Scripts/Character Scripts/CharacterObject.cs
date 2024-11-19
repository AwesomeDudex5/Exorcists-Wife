using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu][System.Serializable]
public class CharacterObject : ScriptableObject
{

    [Rename("Character Name")]
    public string characteName;
    [Rename("Character Sprites")][SerializeField]
    public Sprite[] sprites;

    //indices for character reference
    [HideInInspector]
    public int characterIndex;
    [HideInInspector]
    public int spriteIndex;

}
