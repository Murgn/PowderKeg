using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.InputSystem;

namespace Murgn.Data
{
    public class DataManager : MonoBehaviour
    {
        private string dataDirPath = "";
        public string dataFileName = "";
        public string dataFileExtension = ".powderkeg";

        private bool useEncryption;
        private const string encryptionCode = "MURGN_SAYS_GO_AWAY!";

        private void Start() => dataDirPath = Application.persistentDataPath;

        private void Update()
        {
            if (Keyboard.current.sKey.wasPressedThisFrame)
            {
                MapData map = new MapData("defaultMap", ParticleManager.instance.map);
                Save(map);
            }
        }
        
        public void Load()
        {
            // Use Path.Combine to account for different OS's having different path seperators
            string fullPath = Path.Combine(dataDirPath, dataFileName);
            
            MapData loadedData = null;
            
            if (File.Exists(fullPath))
            {
                try
                {
                    // Load the serialized data from the file
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }
                    
                    // Decrypt data if using encryption
                    if (useEncryption)
                    {
                        dataToLoad = EncryptDecrypt(dataToLoad);
                    }
                    
                    // Deserialize the data from json back into the C# object
                    loadedData = JsonUtility.FromJson<MapData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
                }
            }

            //return loadedData;
        }

        private void Save(MapData data)
        {
            // Use Path.Combine to account for different OS's having different path seperators
            string fullPath = Path.Combine(dataDirPath, data.mapName + dataFileExtension);
            try
            {
                // Create the directory the file will be written to if it doesn't already exist
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                
                // Serialize the C# game data object into json
                string dataToStore = string.Empty;

                for (int y = 0; y < ParticleManager.instance.height; y++)
                {
                    for (int x = 0; x < ParticleManager.instance.width; x++)
                    {
                        dataToStore += JsonUtility.ToJson(ParticleManager.instance.map[x, y], true);
                    }
                }
                
                // Encrypt data if using encryption
                if (useEncryption)
                {
                    dataToStore = EncryptDecrypt(dataToStore);
                }
                
                // write the serialized data to the file
                using FileStream stream = new FileStream(fullPath, FileMode.Create);
                using StreamWriter writer = new StreamWriter(stream);
                
                writer.Write(dataToStore);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
            }
        }

        // XOR Encryption
        private string EncryptDecrypt(string data)
        {
            string modifiedData = "";
            for (int i = 0; i < data.Length; i++)
            {
                modifiedData += (char)(data[i] ^ encryptionCode[i % encryptionCode.Length]);
            }
            return modifiedData;
        }
    }   
}