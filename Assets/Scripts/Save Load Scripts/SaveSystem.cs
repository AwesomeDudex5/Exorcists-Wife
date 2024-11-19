using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SavePlayerData(PlayerData pd, LevelData ld)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savedData.low";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(pd, ld);
        formatter.Serialize(stream, data);

        stream.Close();
    }

    public static SaveData LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/savedData.low";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save File not Found in " + path);
            return null;
        }
    }


}
