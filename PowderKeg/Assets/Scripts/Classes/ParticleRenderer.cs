using Murgn;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Classes
{
    public class ParticleRenderer : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private TileBase tile;

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

            tilemap.transform.position = particleManager.offset;

            for (int y = 0; y < particleManager.height; y++)
            {
                for (int x = 0; x < particleManager.width; x++)
                {
                    tilemap.SetTile(new Vector3Int(x, y), tile);
                    tilemap.SetTileFlags(new Vector3Int(x, y), TileFlags.None);
                }
            }
        }
        
        // Update to do particle renderering
        private void OnRender()
        {
            for (int y = 0; y < particleManager.height; y++)
            {
                for (int x = 0; x < particleManager.width; x++)
                {
                    Vector2Int position = new Vector2Int(x, y);
                    tilemap.SetColor((Vector3Int)position, particleManager.map[x, y].color);
                }
            }
        }
    }
}