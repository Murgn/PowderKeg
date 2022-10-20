using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

namespace Murgn
{
    public class RecordManager : MonoBehaviour
    {
        private ParticleRenderer renderer;
        
        private void Start()
        {
            renderer = ParticleRenderer.instance;
            // Create a texture the size of the screen, RGB24 format
            // int width = Screen.width;
            // int height = Screen.height;
            // Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
            
            // Encode texture into PNG
            //Object.Destroy(renderer.texture);

            // For testing purposes, also write to a file in the project folder
            
        }

        private void Update()
        {
            if (Keyboard.current.pKey.wasPressedThisFrame)
            {
                CreateImage();
            }
        }

        public void CreateImage()
        {
            byte[] bytes = renderer.texture.EncodeToPNG();
                
            string directory = string.Format("{0}/Screenshots/", Application.persistentDataPath);
            FileInfo file = new FileInfo(directory);
            file.Directory.Create();
            int fileCount = file.Directory.GetFiles().Length;
            string filePath = string.Format("{0}PowderKeg_Screenshot{1}.png", directory, fileCount);
            Debug.Log("Took a screenshot at: " + filePath);
                
            File.WriteAllBytes(filePath, bytes);
            EditorUtility.RevealInFinder(filePath);
        }

        public void NextFrame()
        {
            EventManager.Update?.Invoke();
            EventManager.Render?.Invoke();
        }
    }   
}