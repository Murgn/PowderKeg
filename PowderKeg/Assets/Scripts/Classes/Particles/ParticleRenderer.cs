using Murgn;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Classes
{
    public class ParticleRenderer : MonoBehaviour
    {
        [SerializeField] private Image image;

        private Texture2D texture;
        private ParticleManager particleManager;
        private Camera mainCamera;

        private void OnEnable()
        {
            EventManager.Render += OnRender;
        }

        private void OnDisable()
        {
            EventManager.Render -= OnRender;
        }
        
        private void Start()
        {
            particleManager = ParticleManager.instance;
            mainCamera = Camera.main;
            texture = new Texture2D(particleManager.width, particleManager.height);
            texture.filterMode = FilterMode.Point;
            texture.name = "Map";
            image.material.mainTexture = texture;
            image.canvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(particleManager.width, particleManager.height);
        }
        
        // Update to do particle renderering
        private void OnRender()
        {
            for (int y = 0; y < particleManager.height; y++)
            {
                for (int x = 0; x < particleManager.width; x++)
                {
                    Vector2Int position = new Vector2Int(x, y);
                    switch (particleManager.map[x, y].id)
                    {
                        case ParticleId.Air:
                            texture.SetPixel(position.x, position.y, mainCamera.backgroundColor);
                            break;
                        
                        default:
                            texture.SetPixel(position.x, position.y, particleManager.map[x, y].color);
                            break;
                    }
                }
            }
            
            texture.Apply();
        }
    }
}