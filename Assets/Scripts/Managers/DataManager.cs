using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using Parse;

/// <summary>
/// Data Manager contains static methods to read and write PlayerData to disk.
///
/// @author - Alex I.
/// @version - 1.0.0
/// </summary>
public static class DataManager
{
    private static String fileName = ".playerInfo.dat";

    /// <summary>
    /// Saves last player position on the current level on the disk
    /// NOTE: MAC file location:  /Users/USERNAME/Library/Application Support/FJFTeam/Space Explorer/ 
    ///       PC file location: C:\Users\USERNAME\AppData\LocalLow\FJFTeam\Space Explorer
    /// </summary>
    public static void SaveFile(PlayerData data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + fileName, FileMode.Create);
        bf.Serialize(file, data);
        file.Close();
    }

    /// <summary>
    /// Loads last player position on the last current level
    /// </summary>
    public static PlayerData LoadFile()
    {
        PlayerData data = null;

        if (File.Exists(Application.persistentDataPath + fileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + fileName, FileMode.Open);
            try
            {
                data = (PlayerData)bf.Deserialize(file);
            }
            catch (SerializationException e)
            {
                Debug.Log("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                file.Close();
            }
        }

        return data;
    }

    /// <summary>
    /// Delete all locally saved information
    /// </summary>
    public static void ClearSavedData()
    {
        if (File.Exists(Application.persistentDataPath + fileName))
        {
            File.Delete(Application.persistentDataPath + fileName);
        }
    }

    /// <summary>
    /// Saves information about player state to Parse
    /// </summary>
    public static void SaveToParse(PlayerData data)
    {
        ParseObject gameData = new ParseObject("Player");
        gameData["levelID"] = data.levelID;
        gameData["playerID"] = data.playerID;
        gameData["playerEnergy"] = data.playerEnergy;
        gameData["playerPosition"] = data.playerPosition;
        gameData["playerRotation"] = data.playerRotation;
        gameData["playerScale"] = data.playerScale;
        gameData.SaveAsync();
    }

    /// <summary>
    /// Load information about Player state from Parse
    /// </summary>
    public static PlayerData Load(string parsePlayerId)
    {
        ParseQuery<ParseObject> query = ParseObject.GetQuery("Player");
        query.GetAsync(parsePlayerId).ContinueWith(t =>
        {
            ParseObject gameData = t.Result;
            PlayerData data = new PlayerData();
            data.levelID = int.Parse(gameData["levelID"].ToString());
            data.playerID = int.Parse(gameData["playerID"].ToString());
            data.playerEnergy = float.Parse(gameData["playerEnergy"].ToString());
            data.playerPosition = gameData["playerPosition"].ToString();
            data.playerRotation = gameData["playerRotation"].ToString();
            data.playerScale = gameData["playerScale"].ToString();
            return data;
        });
        return null;
    }

}

/// <summary>
/// Model holding player data
/// </summary>
[Serializable]
public class PlayerData
{
    public int playerID;
    public int levelID;
    public float playerEnergy;
    public String playerPosition;
    public String playerRotation;
    public String playerScale;
}