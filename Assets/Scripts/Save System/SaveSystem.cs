using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static LevelData loadedLevelData { get; private set; }
    public static bool shouldLevelBeLoaded = false;

    public static void SaveLevel(PlayerController playerController, GameObject collectibles)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/levelData.pgs"; // The generated binary file will custom extensions. pgs stands for poly-goners
        FileStream stream = new FileStream(path, FileMode.Create);

        LevelData data = new LevelData(playerController, collectibles);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void LoadLevel()
    {
        string path = Application.persistentDataPath + "/levelData.pgs"; // The generated binary file will custom extensions. pgs stands for poly-goners

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            loadedLevelData = formatter.Deserialize(stream) as LevelData;
            stream.Close();
        }
        else
        {
            Debug.LogError("Could not file level data in " + path + "!");
            loadedLevelData = null;
        }
    }
}
