using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Murgn
{
    public class ParticleUpdater : MonoBehaviour
    {

        [SerializeField] private Tilemap tilemap;
        [SerializeField] private TileBase tile;

        private ParticleManager particleManager;

        private void OnEnable()
        {
            EventManager.Update += OnUpdate;
        }

        private void OnDisable()
        {
            EventManager.Update -= OnUpdate;
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
        // TODO: Move this to seperate class
        private void Update()
        {
            for (int y = 0; y < particleManager.height; y++)
            {
                for (int x = 0; x < particleManager.width; x++)
                {
                    Vector2Int position = new Vector2Int(x, y);
                    switch (particleManager.map[x, y])
                    {
                        case (byte)ParticleId.Air:
                            // Do nothing
                            tilemap.SetColor((Vector3Int)position, Color.white);
                            break;
                        
                        case (byte)ParticleId.Block:
                            // Block Render
                            tilemap.SetColor((Vector3Int)position, Color.gray);
                            break;
                        
                        case (byte)ParticleId.Dirt:
                            // Dirt Render
                            tilemap.SetColor((Vector3Int)position, new Color(0.38f, 0.267f, 0.137f));
                            break;

                        case (byte)ParticleId.Sand:
                            // Sand Render
                            tilemap.SetColor((Vector3Int)position, Color.yellow);
                            break;
                        
                        case (byte)ParticleId.Water:
                            // Water Render
                            tilemap.SetColor((Vector3Int)position, Color.blue);
                            break;
                        
                        case (byte)ParticleId.Steam:
                            // Steam Render
                            tilemap.SetColor((Vector3Int)position, Color.cyan);
                            break;
                    }
                }
            }
        }

        // OnUpdate to do particle logic
        private void OnUpdate()
        {
            for (int y = 0; y < particleManager.height; y++)
            {
                for (int x = 0; x < particleManager.width; x++)
                {
                    //Debug.Log($"X: {x}, Y: {y}, Id: {(ParticleId)manager.map[x,y]}");
                    Vector2Int position = new Vector2Int(x, y);
                    switch (particleManager.map[x, y])
                    {
                        case (byte)ParticleId.Air:
                            // Do nothing
                            break;
                        
                        case (byte)ParticleId.Block:
                            // Do nothing
                            break;
                        
                        case (byte)ParticleId.Dirt:
                            // Dirt Update
                            DirtUpdate(position); 
                            break;

                        case (byte)ParticleId.Sand:
                            // Sand Update
                            SandUpdate(position);
                            break;
                        
                        case (byte)ParticleId.Water:
                            // Water Update
                            WaterUpdate(position);
                            break;
                        
                        case (byte)ParticleId.Steam:
                            // Steam Update
                            SteamUpdate(position);
                            break;
                    }
                }
            }
        }

        private void DirtUpdate(Vector2Int position)
        {
            Vector2Int bottomDirection = new Vector2Int(0,-1);
            
            ParticleId bottomCell = particleManager.GetParticle(position + bottomDirection);

            if (bottomCell == ParticleId.Air)
                particleManager.MoveParticle(position, bottomDirection);
        }
        
        private void SandUpdate(Vector2Int position)
        {
            Vector2Int bottomDirection = new Vector2Int(0,-1);
            Vector2Int bottomLeftDirection = new Vector2Int(-1,-1);
            Vector2Int bottomRightDirection = new Vector2Int(1,-1);
            
            ParticleId bottomCell = particleManager.GetParticle(position + bottomDirection);
            ParticleId bottomLeftCell = particleManager.GetParticle(position + bottomLeftDirection);
            ParticleId bottomRightCell = particleManager.GetParticle(position + bottomRightDirection);

            bool bottom = bottomCell == ParticleId.Air;
            bool bottomLeft = bottomLeftCell == ParticleId.Air;
            bool bottomRight = bottomRightCell == ParticleId.Air;

            // Makes sand dispersal more random.
            if (bottomLeft && bottomRight)
            {
                bool rand = Random.Range(0.0f, 1.0f) > 0.5f;
                bottomLeft = rand ? true : false;
                bottomRight = rand ? false : true;
            }

            if (bottom)
                particleManager.MoveParticle(position, bottomDirection);
            else if(bottomLeft)
                particleManager.MoveParticle(position, bottomLeftDirection);
            else if(bottomRight)
                particleManager.MoveParticle(position, bottomRightDirection);
        }
        
        // Water is terrible, i need to fix this eventually
        private void WaterUpdate(Vector2Int position)
        {
            Vector2Int leftDirection = new Vector2Int(-1, 0);
            Vector2Int rightDirection = new Vector2Int(1, 0);
            Vector2Int bottomDirection = new Vector2Int(0,-1);
            Vector2Int bottomLeftDirection = new Vector2Int(-1,-1);
            Vector2Int bottomRightDirection = new Vector2Int(1,-1);
            
            ParticleId leftCell = particleManager.GetParticle(position + leftDirection);
            ParticleId rightCell = particleManager.GetParticle(position + rightDirection);
            ParticleId bottomCell = particleManager.GetParticle(position + bottomDirection);
            ParticleId bottomLeftCell = particleManager.GetParticle(position + bottomLeftDirection);
            ParticleId bottomRightCell = particleManager.GetParticle(position + bottomRightDirection);
            
            bool bottom = bottomCell == ParticleId.Air;
            bool bottomLeft = bottomLeftCell == ParticleId.Air;
            bool bottomRight = bottomRightCell == ParticleId.Air;
            bool left = leftCell == ParticleId.Air;
            bool right = rightCell == ParticleId.Air;

            // Makes water dispersal more random.
            if (!bottom && !bottomLeft && !bottomRight)
            {
                bool rand = Random.Range(0.0f, 1.0f) > 0.5f;
                left = rand ? true : false;
                right = rand ? false : true;
            }
            
            if (bottom)
                particleManager.MoveParticle(position, bottomDirection);
            else if(bottomLeft)
                particleManager.MoveParticle(position, bottomLeftDirection);
            else if(bottomRight)
                particleManager.MoveParticle(position, bottomRightDirection);
            else if(left)
                particleManager.MoveParticle(position, leftDirection);
            else if(right)
                particleManager.MoveParticle(position, rightDirection);
        }
        
        private void SteamUpdate(Vector2Int position)
        {
            Vector2Int upDirection = new Vector2Int(0,1);
            Vector2Int upLeftDirection = new Vector2Int(-1,1);
            Vector2Int upRightDirection = new Vector2Int(1,1);
            
            ParticleId upCell = particleManager.GetParticle(position + upDirection);
            ParticleId upLeftCell = particleManager.GetParticle(position + upLeftDirection);
            ParticleId upRightCell = particleManager.GetParticle(position + upRightDirection);

            bool up = upCell == ParticleId.Air;
            bool upLeft = upLeftCell == ParticleId.Air;
            bool upRight = upRightCell == ParticleId.Air;

            // Makes sand dispersal more random.
            if (upLeft && upRight)
            {
                bool rand = Random.Range(0.0f, 1.0f) > 0.5f;
                upLeft = rand ? true : false;
                upRight = rand ? false : true;
            }

            if (up)
                particleManager.MoveParticle(position, upDirection);
            else if(upLeft)
                particleManager.MoveParticle(position, upLeftDirection);
            else if(upRight)
                particleManager.MoveParticle(position, upRightDirection);
        }
}   
}