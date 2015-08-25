using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class WorldRecordData
{
    public WorldRecordData() 
    { 
    }

    public void LogDebug()
    {
        Debug.Log("WorldRecordData: " + name + " sp="+topSpeed+", t="+topTime+", sc="+topScore);
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

    public WorldRecordData GetWorldRecordByName(string name)
    {
        foreach (WorldRecordData wd in data.worldRecords)
        {
            if (wd.name == name)
                return (wd);
        }
        return (null);
    }

    public bool SaveActiveScore(string worldName, GameController gc)
    {
        WorldRecordData wd = GetWorldRecordByName(worldName);
        bool newRecord = false;
        float sc = ScoreController.GetScore(gc.moveSpeedY, gc.timeSpent, gc.finalScore, gc.score);

        if (wd == null)
        {
            wd = new WorldRecordData();
            wd.name = worldName;
            data.worldRecords.Add(wd);
        }
        if (gc.moveSpeedY > wd.topSpeed)
        {
            wd.topSpeed = gc.moveSpeedY;
            newRecord = true;
        }
        if (gc.timeSpent > wd.topTime)
        {
            wd.topTime = gc.timeSpent;
            newRecord = true;
        }
        if (sc > wd.topScore)
        {
            wd.topScore = sc;
            newRecord = true;
        }
        return (newRecord);
    }

    public void Pack(GameController gc)
    {
        //data = new GameData();
        //data.userLevel = 1;
        //foreach (WorldGenerationPossibility wc in gc.generator.worlds)
        //{
        //}
    }

    public void Unpack(GameController gc)
    {
        foreach (WorldRecordData dt in data.worldRecords)
        {
            dt.LogDebug();
        }
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