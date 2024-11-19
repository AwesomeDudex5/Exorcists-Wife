using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    SceneLoader sl;
    // Start is called before the first frame update
    void Start()
    {
        sl = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();
    }

    public void loadScene(int sceneIndex)
    {
        sl.loadScene(sceneIndex);
    }

    public void loadSceneWithData()
    {
        //load save data from file
        SaveData sd = SaveSystem.LoadPlayerData();

        //set the player and level data to sceneLoader
        sl._playerData = sd._playerData;
        sl._levelData = sd._levelData;

        sl.canLoadData = true;

        //load appropiate scene
        sl.loadScene(sd._levelData.levelIndex);
    }

    public void quitGame()
    {
        Application.Quit();
    }

}
