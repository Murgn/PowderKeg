using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using Murgn.OdinSerializer;
using UnityEngine.InputSystem;

namespace Murgn.Data
{
    // TODO: Need to add map settings to MapData.cs
    public class DataManager : MonoBehaviour
    {
        private string dataDirPath = "";
        public string dataFileName = "";
        public string dataFileExtension = ".powderkeg";

        private const DataFormat dataFormat = DataFormat.Binary;
        
        private void Start() => dataDirPath = Application.persistentDataPath;

        private void Update()
        {
            if (Keyboard.current.sKey.wasPressedThisFrame)
            {
                MapData map = new MapData(dataFileName, ParticleManager.instance.map, ParticleManager.instance.mapSize);
                Save(map);
            }
            
            if (Keyboard.current.lKey.wasPressedThisFrame)
            {
                Load(dataFileName);
            }
        }
        
        public void Load(string mapName)
        {
            // Use Path.Combine to account for different OS's having different path seperators
            string directory = string.Format("{0}/Saves/", dataDirPath);
            string fullPath = Path.Combine(directory, mapName + dataFileExtension);
            
            if (File.Exists(fullPath))
            {
                try
                {
                    byte[] bytes = File.ReadAllBytes(fullPath);
                    
                    MapData loadedData = SerializationUtility.DeserializeValue<MapData>(bytes, dataFormat);
                    Debug.Log("Loaded map: " + loadedData.mapName);
                    ParticleManager.instance.map = loadedData.map;
                }
                catch (Exception e)
                {
                    Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
                }
            }
        }

        private void Save(MapData data)
        {
            // Use Path.Combine to account for different OS's having different path seperators
            string directory = string.Format("{0}/Saves/", dataDirPath);
            string fullPath = Path.Combine(directory, data.mapName + dataFileExtension);
            try
            {
                // Create the directory the file will be written to if it doesn't already exist
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                
                // Serialize the C# game data object into json
                byte[] bytes = SerializationUtility.SerializeValue(data, dataFormat);
                File.WriteAllBytes(fullPath, bytes);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
            }
        }
    }   
}