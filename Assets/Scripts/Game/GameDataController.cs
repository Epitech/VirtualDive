using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class WorldRecordData
{
    public WorldRecordData() 
    { 
    }

    public string name = "";
    public float topSpeed = 0.0f;
    public float topTime = 0.0f;
    public float topScore = 0.0f;
};

[System.Serializable]
public class GameData
{
    public GameData() 
    {
        worldRecords = new List<WorldRecordData>();
    }

    public List<WorldRecordData> worldRecords;
    public int userLevel = 0;
};

public class GameDataController
{
    public GameData data;

    public WorldRecordData GetWorldRecord(string world)
    {
        foreach (WorldRecordData rd in data.worldRecords)
        {
            if (rd.name == world)
                return (rd);
        }
        return (null);
    }

    public void Pack(GameController gc)
    {
        data = new GameData();
        data.userLevel = 1;
    }

    public void Unpack(GameController gc)
    {

    }

    public void SaveGamedata(GameController gc)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/userdata.gd");

        Pack(gc);
        bf.Serialize(file, data);
        file.Close();
    }
    
    public void LoadGamedata(GameController gc)
    {
        if(File.Exists(Application.persistentDataPath + "/userdata.gd")) 
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/userdata.gd", FileMode.Open);
            
            data = (GameData)bf.Deserialize(file);
            Unpack(gc);
            file.Close();
        }
    }
}