
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEngine.Timeline;

[CustomEditor(typeof(ScenarioManager))] //this shows up in inspector
public class ScenarioManagerEditor : Editor
{
    protected static List<SceneObj> sceneObjArray = new List<SceneObj>();

    //bools to keep track of foldouts
    //runs parallel to each other
    protected static List<bool> showSceneBools = new List<bool>();
    protected static List<List<bool>> showDialogueBools = new List<List<bool>>();

    protected static List<CharacterObject> characterObjectArray = new List<CharacterObject>();
    protected static List<string> characterNames = new List<string>();
    protected static List<List<string>> characterSpriteNames = new List<List<string>>();
    protected static List<Sprite[]> characterSprites = new List<Sprite[]>();
    protected static List<string> spritePositions = new List<string>();

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); //loads public elements of scenariomanager
        EditorGUI.BeginChangeCheck();
        GUILayout.Space(10);

        ScenarioManager script = (ScenarioManager)target; //sets target of Scenario Manager Instance
                                                          // Debug.Log("Number of Scenes: " + script.scenarios.Count);
                                                          //script.numberOfScenes = script.scenarios.Count;
                                                          //GUI.changed = false;

        //instantiate empty arrays/lists
        if (sceneObjArray == null || sceneObjArray.ToArray().Length == 0)
            sceneObjArray = script.scenarios;
        if (script.scenarios == null || script.scenarios.ToArray().Length < script.numberOfScenes)
            script.scenarios = sceneObjArray;

        if (showSceneBools.ToArray().Length < script.numberOfScenes || showSceneBools == null)
        {
            for (int i = 0; i < script.numberOfScenes; i++)
            {
                showSceneBools.Add(false);
                showDialogueBools.Add(new List<bool>());
                for (int j = 0; j < script.scenarios[i].numberOfDialogues; j++)
                {
                    showDialogueBools[i].Add(true);
                }
            }
        }

        if (characterObjectArray.Count < script.cm.characters.Length)
        {
            for (int i = 0; i < script.cm.characters.Length; i++)
            {
                characterObjectArray.Add(script.cm.characters[i]);
            }
        }

        /* for (int i = 0; i < characterObjectArray.ToArray().Length; i++)
         {
             Debug.Log("Character Name: " + characterObjectArray[i].characteName);
             for (int j = 0; j < characterObjectArray[i].sprites.Length; j++)
             {
                 Debug.Log("Sprite " + j + ": " + characterObjectArray[i].sprites[j]);
             }
         }*/

        //set up character properties (name/sprites from character storage)
        //character names from character storage      
        if (characterNames.ToArray().Length < characterObjectArray.ToArray().Length)
        {
            characterNames = new List<string>();
            characterSprites = new List<Sprite[]>();


            characterSpriteNames = new List<List<string>>();
            if (characterNames.ToArray().Length == 0 || characterNames == null) //if the character arrays are new, empty, or null
            {
                for (int i = 0; i < characterObjectArray.ToArray().Length; i++)
                {
                    characterNames.Add(characterObjectArray[i].characteName);
                    //gotta initialize the arrays within the list/arrays
                    characterSprites.Add(new Sprite[characterObjectArray[i].sprites.Length]);
                    characterSpriteNames.Add(new List<string>());
                    characterSprites[i] = characterObjectArray[i].sprites;
                    for (int j = 0; j < characterSprites[i].Length; j++)
                    {
                        characterSpriteNames[i].Add(characterSprites[i][j].name);
                    }
                }
            }
        }

        if (spritePositions.Count < script.gameSpritePositions.Length)
        {
            for (int i = 0; i < script.gameSpritePositions.Length; i++)
            {
                spritePositions.Add(i + "");
            }
        }

        /*
        for (int i = 0; i < characterNames.ToArray().Length; i++)
        {
            Debug.Log("Character Name: " + characterNames[i]);
            for (int j = 0; j < characterSprites[i].Length; j++)
            {
                Debug.Log("Sprite " + j + ": " + characterSpriteNames[i][j]);
              //  Debug.Log("Sprite " + j + ": " + characterSprites[i][j]);
            }
        }
      //  */


        //button to add scene at end of list
        if (GUILayout.Button("Add Scene"))
        {
            sceneObjArray.Add(new SceneObj());
            showDialogueBools.Add(new List<bool>());
            script.numberOfScenes++;
            script.scenarios = sceneObjArray;
            showSceneBools.Add(false);
            //need to create new list for dialogue???
        }

        if (script.numberOfScenes != 0)
        {
            EditorGUILayout.LabelField("Scenes Available: " + script.numberOfScenes);
        }

        //flags to keep track of removing and adding scenes/dialogues
        /*
        bool addSceneFlag, addDialogueFlag;
        int addSceneIndex, addDialogueIndex;
        */
        bool removeSceneFlag = false;
        int removeSceneIndex = 0;


        //bold foldoutstyle
        GUIStyle style = EditorStyles.foldout;
        FontStyle previousStyle = style.fontStyle;
        style.fontStyle = FontStyle.Bold;


        //Debug.Log("Number of Scenes: " + script.numberOfScenes);
        for (int i = 0; i < script.scenarios.Count; i++)
        {
            EditorGUI.indentLevel = 1;
            //foldout for scenes, show each element
            GUIContent showSceneLabel = new GUIContent("Scene: " + i);
            showSceneBools[i] = EditorGUILayout.Foldout(showSceneBools[i], showSceneLabel, style);
            if (showSceneBools[i])
            {

                //field for mission description
                EditorGUILayout.LabelField("Mission Description");
                script.scenarios[i].missionDescription = EditorGUILayout.TextArea(script.scenarios[i].missionDescription, GUILayout.MinHeight(20));

                //field for animation to play at the beginning of each scene
                EditorGUILayout.LabelField("Animation");
                script.scenarios[i]._animation = EditorGUILayout.ObjectField(script.scenarios[i]._animation, typeof(TimelineAsset), false) as TimelineAsset;

                GUILayout.BeginHorizontal();

                EditorGUIUtility.labelWidth = 50;
                EditorGUIUtility.fieldWidth = 60;
                //field for flag input
                script.scenarios[i].flag = i;
                EditorGUILayout.LabelField("Scene Flag: " + script.scenarios[i].flag);
                // script.scenarios[i].flag = EditorGUILayout.IntField("Flag ", script.scenarios[i].flag, GUILayout.ExpandWidth(false));

                EditorGUIUtility.labelWidth = 80;
                EditorGUIUtility.fieldWidth = 120;
                //field for conditions drowdown
                script.scenarios[i].condition = (conditions)EditorGUILayout.EnumPopup("Condiitons ", script.scenarios[i].condition, GUILayout.ExpandWidth(false));

                EditorGUIUtility.labelWidth = 140;
                EditorGUIUtility.fieldWidth = 60;
                //field for number of dialogues input
                script.scenarios[i].numberOfDialogues = EditorGUILayout.IntField("Number Of Dialogues ", script.scenarios[i].numberOfDialogues, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                //interact all parameter
                if (script.scenarios[i].condition == conditions.interactWithAll)
                {
                    EditorGUIUtility.labelWidth = 140;
                    EditorGUIUtility.fieldWidth = 40;
                    script.scenarios[i].interactCount = EditorGUILayout.IntField("Number of Interacts ", script.scenarios[i].interactCount, GUILayout.ExpandWidth(false));
                }

                if(script.scenarios[i].condition == conditions.killAll)
                {
                    EditorGUIUtility.labelWidth = 140;
                    EditorGUIUtility.fieldWidth = 40;
                    script.scenarios[i].enemiesToKill = EditorGUILayout.IntField("Number Of Enemies ", script.scenarios[i].enemiesToKill, GUILayout.ExpandWidth(false));
                }
                GUILayout.EndHorizontal();

                //populate/decrement dialogue arrays
                while (script.scenarios[i].dialogues.Count < script.scenarios[i].numberOfDialogues)
                {
                    script.scenarios[i].dialogues.Add(new Dialogue());
                    showDialogueBools[i].Add(true);
                }
                if (script.scenarios[i].dialogues.Count - script.scenarios[i].numberOfDialogues > 0)
                {
                    script.scenarios[i].dialogues.RemoveRange(script.scenarios[i].numberOfDialogues, script.scenarios[i].dialogues.Count - script.scenarios[i].numberOfDialogues);
                    showDialogueBools[i].RemoveRange(script.scenarios[i].numberOfDialogues, script.scenarios[i].dialogues.Count - script.scenarios[i].numberOfDialogues);
                }


                GUILayout.Space(10);
                // Debug.Log("Scene " + i + " Number of Dialogues: " + script.scenarios[i].dialogues.Count);

                //display list of dialogues                  
                for (int j = 0; j < script.scenarios[i].dialogues.Count; j++)
                {
                    EditorGUI.indentLevel = 2;
                    GUIContent showDalogueLabel = new GUIContent("Dialogue " + j);
                    showDialogueBools[i][j] = EditorGUILayout.Foldout(showDialogueBools[i][j], showDalogueLabel, style);
                    if (showDialogueBools[i][j])
                    {
                        GUILayout.BeginHorizontal();
                        //field for character names availaible
                        EditorGUIUtility.labelWidth = 100;
                        EditorGUIUtility.fieldWidth = 90;
                        script.scenarios[i].dialogues[j].characterIndex = EditorGUILayout.Popup("Character ", script.scenarios[i].dialogues[j].characterIndex, characterNames.ToArray(), GUILayout.ExpandWidth(false));
                        int characterIndex = script.scenarios[i].dialogues[j].characterIndex;
                        //   Debug.Log("Scene " + i + " Dialogue " + j + " Character Index: " + characterIndex);
                        script.scenarios[i].dialogues[j].characterName = characterNames[characterIndex];
                        // Debug.Log("Scene " + i + " Dialogue " + j + " Character Name: " + script.scenarios[i].dialogues[j].characterName);

                        //field for sprite input
                        EditorGUIUtility.labelWidth = 80;
                        EditorGUIUtility.fieldWidth = 90;
                        script.scenarios[i].dialogues[j].spriteIndex = EditorGUILayout.Popup("Sprite ", script.scenarios[i].dialogues[j].spriteIndex, characterSpriteNames[characterIndex].ToArray(), GUILayout.ExpandWidth(false));
                        int spriteIndex = script.scenarios[i].dialogues[j].spriteIndex;
                        // Debug.Log("Scene " + i + " Dialogue " + j + " Sprite Index: " + spriteIndex);
                        script.scenarios[i].dialogues[j].characterSprite = characterSprites[characterIndex][spriteIndex];
                        // Debug.Log("Scene " + i + " Dialogue " + j + " Sprite Sprite: " + characterSprites[characterIndex][spriteIndex]);
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        //field for spritePosition Input
                        EditorGUIUtility.labelWidth = 120;
                        EditorGUIUtility.fieldWidth = 30;
                        script.scenarios[i].dialogues[j].spritePosition = EditorGUILayout.Popup("Sprite Position", script.scenarios[i].dialogues[j].spritePosition, spritePositions.ToArray(), GUILayout.ExpandWidth(false));

                        //field for enum animation type
                        EditorGUIUtility.labelWidth = 90;
                        EditorGUIUtility.fieldWidth = 80;
                        script.scenarios[i].dialogues[j].animationType = (animationTypes)EditorGUILayout.EnumPopup("Animation ", script.scenarios[i].dialogues[j].animationType, GUILayout.ExpandWidth(false));
                        GUILayout.EndHorizontal();

                        //field for text
                        EditorGUILayout.LabelField("Sentences");
                        script.scenarios[i].dialogues[j].sentences = EditorGUILayout.TextArea(script.scenarios[i].dialogues[j].sentences, GUILayout.MinHeight(40));
                        GUILayout.Space(10);

                    } //end of if dialogue bool

                } //end of dialogue array for loop
                  //button to add scene at end of list
                if (GUILayout.Button("Remove Scene"))
                {
                    removeSceneFlag = true;
                    removeSceneIndex = i;

                }
            } // end of if scene bool
        } // end of scene array for loop

        //if remove flag is triggered
        if (removeSceneFlag)
        {
            sceneObjArray.RemoveAt(removeSceneIndex);
            script.numberOfScenes--;
            showSceneBools.RemoveAt(removeSceneIndex);
            showDialogueBools.RemoveAt(removeSceneIndex);
            script.scenarios = sceneObjArray; //set script to the changed editor array
            removeSceneFlag = false;
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            Undo.RecordObject(script, "Changes Noticed Senpai");
        }

        EditorUtility.SetDirty(script);

        serializedObject.ApplyModifiedProperties();
    }
}
