using Murgn.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Murgn
{
    public class ParticleRenderer : Singleton<ParticleRenderer>
    {
        public Image image;
        public Color airColor;
        public Texture2D texture;
        
        private ParticleManager particleManager;

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
            texture = new Texture2D(particleManager.width, particleManager.height)
            {
                filterMode = FilterMode.Point,
                name = "Map"
            };
            image.material.mainTexture = texture;
            //image.canvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(particleManager.width, particleManager.height);
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
                            texture.SetPixel(position.x, position.y, airColor);
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