using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public PlayerData _playerData;
    public LevelData _levelData;

    public SaveData(PlayerData pd, LevelData ld)
    {
        _playerData = pd;
        _levelData = ld;
    }
}

[System.Serializable]
public class PlayerData
{
    //Caius data
    public int health;
    public float[] caiusPosition;
    public bool UzukiAccompanying;
    public float[] UzukiPosition;

    //constructor class
    // accounts for Uzuki
    public PlayerData(GameObject Caius, GameObject Uzuki)
    {
        health = Caius.GetComponent<PlayerInfo>().playerHealth;
        caiusPosition = new float[3];
        caiusPosition[0] = Caius.transform.position.x;
        caiusPosition[1] = Caius.transform.position.y;
        caiusPosition[2] = Caius.transform.position.z;

        UzukiAccompanying = Uzuki.GetComponent<UzukiMovement>().behaviour.followCaius;
        UzukiPosition = new float[3];
        UzukiPosition[0] = Uzuki.transform.position.x;
        UzukiPosition[1] = Uzuki.transform.position.y;
        UzukiPosition[2] = Uzuki.transform.position.z;
    }
    //no Uzuki
    public PlayerData(GameObject Caius)
    {
        health = Caius.GetComponent<PlayerInfo>().playerHealth;
        caiusPosition = new float[3];
        caiusPosition[0] = Caius.transform.position.x;
        caiusPosition[1] = Caius.transform.position.y;
        caiusPosition[2] = Caius.transform.position.z;
    }
}

[System.Serializable]
public class LevelData
{
    public int levelIndex;
    public int curretnScenarioIndex;

    public LevelData(int LevelIndex, int scenarioIndex)
    {
        levelIndex = LevelIndex;
        curretnScenarioIndex = scenarioIndex;
    }
}
