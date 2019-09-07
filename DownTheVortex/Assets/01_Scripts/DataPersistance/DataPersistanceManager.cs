using UnityEngine;
using System.Collections;
using BaseSystems.Managers;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BaseSystems.DataPersistance
{
    public class DataPersistanceManager : Manager
    {
        public static PlayerData PlayerData;

        BinaryFormatter _formatter;
        const string FILE_EXT = ".sdata";

        public override void Initialize()
        {
            _formatter = new BinaryFormatter();
            Load();
        }

        public void Save()
        {
            string filePath = Path.Combine(Application.persistentDataPath, "save" + FILE_EXT);
            FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);

            _formatter.Serialize(fs, PlayerData);
            fs.Close();
        }

        public void Load()
        {
            string filePath = Path.Combine(Application.persistentDataPath, "save" + FILE_EXT);
            if (!File.Exists(filePath))
            {
                PlayerData = new PlayerData();
                return;
            }
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            PlayerData = (PlayerData)_formatter.Deserialize(fs);
            fs.Close();
        }

#if UNITY_EDITOR
        [ContextMenu("Delete saved data")]
        public void Delete()
        {
            string filePath = Path.Combine(Application.persistentDataPath, "save" + FILE_EXT);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
#endif
    }
}