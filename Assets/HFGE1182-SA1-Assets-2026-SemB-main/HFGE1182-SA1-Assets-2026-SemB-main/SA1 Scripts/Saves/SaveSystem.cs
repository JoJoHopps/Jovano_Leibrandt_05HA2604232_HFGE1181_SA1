using System;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(ShootHandler playerShootHandler,  HealthHandler playerHealthHandler)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/PlayerData.dat";
        FileStream stream = new FileStream(path, FileMode.Create);
        try
        {
            PlayerData data = new PlayerData(playerShootHandler,  playerHealthHandler);
            formatter.Serialize(stream, data);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        stream.Close();
    }

    public static void SaveEnemy( GameObject[] enemies)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/EnemyData.dat";

        FileStream stream;
        stream = new FileStream(path, FileMode.Create);
        try
        {
            EnemySaveData data = new EnemySaveData();
            
            foreach (var e in enemies)
            {
                data.enemies.Add(new EnemyData(e.GetComponent<HealthHandler>()));
            }
            formatter.Serialize(stream, data);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/PlayerData.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            stream.Position = 0;
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            
            return data;
            
        }
        else
        {
            return null;
        }
     
    }

    public static EnemySaveData LoadEnemy()
    {
        string path = Application.persistentDataPath + "/EnemyData.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            stream.Position = 0;
            EnemySaveData data = formatter.Deserialize(stream) as EnemySaveData;
            stream.Close();
            
            return data;
            
        }
        else
        {
            return null;
        }
    }
}
