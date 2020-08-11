using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SA.SpaceShooter
{
    public class SaveLoadManager
    {
        #region Var

        private static object syncRoot = new object();
        private static SaveLoadManager instance;

        private static string SAVE_GAME_PATH = Application.persistentDataPath + "/save.bin";

        BinaryFormatter formatter;

        #endregion


        #region Init

        private SaveLoadManager()
        {
            formatter = new BinaryFormatter();

            Debug.Log("SaveLoadManager => Construct()");
        }


        public static SaveLoadManager GetInstance()
        {
            lock (syncRoot)
            {
                if (instance == null)
                {
                    instance = new SaveLoadManager();
                }
            }

            return instance;
        }


        #endregion


        #region Save

        public void SaveGame(PlayerSave playerSave)
        {
            using (FileStream file = new FileStream(SAVE_GAME_PATH, FileMode.Create))
            {
                formatter.Serialize(file, playerSave);
            }

            Debug.Log("Save data");
        }

        #endregion


        #region Load

        public bool LoadGame(out PlayerSave playerSave)
        {
            Debug.Log("Load data");

            playerSave = null;

            if (File.Exists(SAVE_GAME_PATH))
            {
                using (FileStream file = new FileStream(SAVE_GAME_PATH, FileMode.OpenOrCreate))
                {
                    playerSave = (PlayerSave)formatter.Deserialize(file);
                }

                return true;
            }

            return false;
        }

        #endregion

    }
}
