using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Timeline;

public enum conditions { story, killAll, killBoss, pressToInteract, interactWithAll, onCollision, None }
/*
 * killAll - kill all enemies to progress to the next scene
 * pressToInteract - will most likely be a seperate gameObject, triggers when within an area and can interact with object or character
 * interactWithAll - interact with a number of interactables and trigger scene when all accounted for
 * story - plays automatically when loading scene
 * onCollison - a trigger based condition, when player enters an area, collides with box collider
 * */

[System.Serializable]
public class SceneObj
{
    public TimelineAsset _animation;
    //scene will play if current flag matches this flag
    [Range(0, 1000)]
    public int flag;
    public string missionDescription;
    public conditions condition;
    public int interactCount;
    public int enemiesToKill;
    public bool canPlay, isFinished;
    [Header("Write Dialogue for this Scene")]
    public List<Dialogue> dialogues = new List<Dialogue>();
    public int numberOfDialogues;
}


