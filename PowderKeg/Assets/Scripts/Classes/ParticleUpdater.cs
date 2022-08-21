using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Murgn
{
    public class ParticleUpdater : MonoBehaviour
    {
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
        }
        
        // OnUpdate to do particle logic
        private void OnUpdate()
        {
            for (int y = 0; y < particleManager.height; y++)
            {
                for (int x = 0; x < particleManager.width; x++)
                {
                    if (!particleManager.map[x, y].hasBeenUpdated)
                    {
                        particleManager.map[x, y].hasBeenUpdated = true;
                        Vector2Int position = new Vector2Int(x, y);
                        switch (particleManager.map[x, y].id)
                        {
                            case ParticleId.Air:
                                // Do nothing
                                break;
                        
                            case ParticleId.Block:
                                // Do nothing
                                break;
                        
                            case ParticleId.Dirt:
                                // Dirt Update
                                DirtUpdate(position); 
                                break;

                            case ParticleId.Sand:
                                // Sand Update
                                SandUpdate(position);
                                break;
                        
                            case ParticleId.Water:
                                // Water Update
                                WaterUpdate(position);
                                break;
                        
                            case ParticleId.Steam:
                                // Steam Update
                                SteamUpdate(position);
                                break;
                        }
                    }
                }
            }
        }

        private void DirtUpdate(Vector2Int position)
        {
            Vector2Int bottomDirection = new Vector2Int(0,-1);
            
            ParticleId bottomCell = particleManager.GetParticle(position + bottomDirection).id;

            if (bottomCell == ParticleId.Air)
                particleManager.MoveParticle(position, bottomDirection);
        }
        
        private void SandUpdate(Vector2Int position)
        {
            Vector2Int bottomDirection = new Vector2Int(0,-1);
            Vector2Int bottomLeftDirection = new Vector2Int(-1,-1);
            Vector2Int bottomRightDirection = new Vector2Int(1,-1);
            
            ParticleId bottomCell = particleManager.GetParticle(position + bottomDirection).id;
            ParticleId bottomLeftCell = particleManager.GetParticle(position + bottomLeftDirection).id;
            ParticleId bottomRightCell = particleManager.GetParticle(position + bottomRightDirection).id;

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
            Vector2Int topDirection = new Vector2Int(0, 1);
            Vector2Int leftDirection = new Vector2Int(-1, 0);
            Vector2Int rightDirection = new Vector2Int(1, 0);
            Vector2Int bottomDirection = new Vector2Int(0,-1);
            Vector2Int bottomLeftDirection = new Vector2Int(-1,-1);
            Vector2Int bottomRightDirection = new Vector2Int(1,-1);

            Particle thisCell = particleManager.GetParticle(position);
            Particle topCell = particleManager.GetParticle(position + topDirection);
            Particle leftCell = particleManager.GetParticle(position + leftDirection);
            Particle rightCell = particleManager.GetParticle(position + rightDirection);
            Particle bottomCell = particleManager.GetParticle(position + bottomDirection);
            Particle bottomLeftCell = particleManager.GetParticle(position + bottomLeftDirection);
            Particle bottomRightCell = particleManager.GetParticle(position + bottomRightDirection);
            
            bool bottom = bottomCell.id == ParticleId.Air;
            bool bottomLeft = bottomLeftCell.id == ParticleId.Air;
            bool bottomRight = bottomRightCell.id == ParticleId.Air;
            bool left = leftCell.id == ParticleId.Air;
            bool right = rightCell.id == ParticleId.Air;

            //Makes water dispersal more random.
            if (!bottom && !bottomLeft && !bottomRight)
            {
                bool rand = Random.Range(0.0f, 1.0f) > 0.5f;
                left = rand ? true : false;
                right = rand ? false : true;
            }

            if (topCell.weight > thisCell.weight)
                particleManager.SwapParticle(position, position + topDirection);
            
            if (bottom)
                particleManager.MoveParticle(position, bottomDirection);
            else if(bottomLeft)
                particleManager.MoveParticle(position, bottomLeftDirection);
            else if(bottomRight)
                particleManager.MoveParticle(position, bottomRightDirection);
            
            if(left)
                particleManager.MoveParticle(position, leftDirection);
            else if(right)
                particleManager.MoveParticle(position, rightDirection);
        }
        
        private void SteamUpdate(Vector2Int position)
        {
            Vector2Int upDirection = new Vector2Int(0,1);
            Vector2Int upLeftDirection = new Vector2Int(-1,1);
            Vector2Int upRightDirection = new Vector2Int(1,1);
            
            ParticleId upCell = particleManager.GetParticle(position + upDirection).id;
            ParticleId upLeftCell = particleManager.GetParticle(position + upLeftDirection).id;
            ParticleId upRightCell = particleManager.GetParticle(position + upRightDirection).id;

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