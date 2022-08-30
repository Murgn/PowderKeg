using System;
using Murgn.Utils;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Murgn
{
    public class ParticleUpdater : MonoBehaviour
    {
        private ParticleManager particleManager;
        
        private static readonly Vector2Int topLeftDirection = new (-1, 1);
        private static readonly Vector2Int topDirection = new (0, 1);
        private static readonly Vector2Int topRightDirection = new (1, 1);
        private static readonly Vector2Int rightDirection = new (1, 0);
        private static readonly Vector2Int bottomRightDirection = new (1, -1);
        private static readonly Vector2Int bottomDirection = new (0, -1);
        private static readonly Vector2Int bottomLeftDirection = new (-1, -1);
        private static readonly Vector2Int leftDirection = new (-1, 0);

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

        /// TODO: To prevent particles falling through diagonal holes,
        /// TODO: I should check if the left + right cells are empty too.
        
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
                        
                        LifetimeUpdate(position);
                        
                        switch (particleManager.map[x, y].id)
                        {
                            // Static & empty objects don't need any logic

                            case ParticleId.Dirt:
                                // Dirt Update
                                DirtUpdate(position);
                                particleManager.particleCount++;
                                break;
                            
                            case ParticleId.Grass:
                                // Grass Update
                                GrassUpdate(position);
                                particleManager.particleCount++;
                                break;

                            case ParticleId.Sand:
                                // Sand Update
                                SandUpdate(position);
                                particleManager.particleCount++;
                                break;

                            case ParticleId.Water:
                            case ParticleId.Oil:
                                // Water & Oil Update
                                WaterUpdate(position);
                                particleManager.particleCount++;
                                break;

                            case ParticleId.Steam:
                            case ParticleId.Smoke:
                                // Steam & Smoke Update
                                SteamUpdate(position);
                                particleManager.particleCount++;
                                break;

                            case ParticleId.Fire:
                                // Fire Update
                                FireUpdate(position);
                                particleManager.particleCount++;
                                break;
                        }
                    }
                }
            }
        }

        private void LifetimeUpdate(Vector2Int position)
        {
            Particle thisCell = particleManager.GetParticle(position);

            if (thisCell.lifetime == 0)
            {
                particleManager.DestroyParticle(position);
            }
            else
            {
                particleManager.map[position.x, position.y].lifetime--;
            }
        }

        // this is super slow
        // private void ActiveCheck(Vector2Int position)
        // {
        //     ParticleId topLeftCell = particleManager.GetParticle(position + topLeftDirection).id;
        //     ParticleId topCell = particleManager.GetParticle(position + topDirection).id;
        //     ParticleId topRightCell = particleManager.GetParticle(position + topRightDirection).id;
        //     ParticleId rightCell = particleManager.GetParticle(position + rightDirection).id;
        //     ParticleId bottomRightCell = particleManager.GetParticle(position + bottomRightDirection).id;
        //     ParticleId bottomCell = particleManager.GetParticle(position + bottomDirection).id;
        //     ParticleId bottomLeftCell = particleManager.GetParticle(position + bottomLeftDirection).id;
        //     ParticleId leftCell = particleManager.GetParticle(position + leftDirection).id;
        //     
        //     bool topLeft = topLeftCell == ParticleId.Air;
        //     bool top = topCell == ParticleId.Air;
        //     bool topRight = topRightCell == ParticleId.Air;
        //     bool right = rightCell == ParticleId.Air;
        //     bool bottomRight = bottomRightCell == ParticleId.Air;
        //     bool bottom = bottomCell == ParticleId.Air;
        //     bool bottomLeft = bottomLeftCell == ParticleId.Air;
        //     bool left = leftCell == ParticleId.Air;
        //
        // }

        /// If there is air above the dirt, turn into grass,
        /// If there is a solid block above grass, turn into dirt
        private void DirtUpdate(Vector2Int position)
        {
            ParticleId topCell = particleManager.GetParticle(position + topDirection).id;
            ParticleId bottomCell = particleManager.GetParticle(position + bottomDirection).id;

            bool top = topCell == ParticleId.Air;
            bool bottom = bottomCell == ParticleId.Air;

            // If there is nothing above the cell, and something below the cell
            if (top && !bottom)
            {
                particleManager.map[position.x, position.y].timer -= Utilities.RandomChance(50) ? 1 : 0;
            }
            else
                particleManager.map[position.x, position.y].timer = 100;

            if (particleManager.map[position.x, position.y].timer <= 0)
                particleManager.PlaceParticle(position, ParticleTypes.Grass, true, true);

            if (bottom)
                particleManager.MoveParticle(position, bottomDirection);
        }

        private void GrassUpdate(Vector2Int position)
        {
            ParticleState topCell = particleManager.GetParticle(position + topDirection).state;
            ParticleState bottomCell = particleManager.GetParticle(position + bottomDirection).state;

            bool top = topCell != ParticleState.Solid;
            bool bottom = bottomCell != ParticleState.Solid;

            if (top)
                particleManager.map[position.x, position.y].timer = 50;
            else
            {
                particleManager.map[position.x, position.y].timer -= Utilities.RandomChance(10) ? 1 : 0;
            }
            
            if (particleManager.map[position.x, position.y].timer <= 0)
            {
                particleManager.PlaceParticle(position, ParticleTypes.Dirt, true, true);
            }
            
            
            if (bottom)
                particleManager.MoveParticle(position, bottomDirection);
        }

    private void SandUpdate(Vector2Int position)
        {
            ParticleId bottomCell = particleManager.GetParticle(position + bottomDirection).id;
            ParticleId bottomLeftCell = particleManager.GetParticle(position + bottomLeftDirection).id;
            ParticleId bottomRightCell = particleManager.GetParticle(position + bottomRightDirection).id;

            bool bottom = bottomCell == ParticleId.Air;
            bool bottomLeft = bottomLeftCell == ParticleId.Air;
            bool bottomRight = bottomRightCell == ParticleId.Air;

            // Makes sand dispersal random.
            if (bottomLeft && bottomRight)
            {
                bool rand = Utilities.RandomChance(50);
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
            Particle thisCell = particleManager.GetParticle(position);
            Particle topLeftCell = particleManager.GetParticle(position + topLeftDirection);
            Particle topCell = particleManager.GetParticle(position + topDirection);
            Particle topRightCell = particleManager.GetParticle(position + topRightDirection);
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

            

            if (/*topCell.state != ParticleState.Liquid && */topCell.weight > thisCell.weight)
                particleManager.SwapParticle(position, position + topDirection);
            // Randomise this or get rid of it
            // else if (topLeftCell.weight > thisCell.weight)
            //     particleManager.SwapParticle(position, position + topLeftDirection);
            // else if (topRightCell.weight > thisCell.weight)
            //     particleManager.SwapParticle(position, position + topRightDirection);
            
           
            if (!bottom && !bottomLeft && !bottomRight)
            {
                bool rand = Utilities.RandomChance(50);
                left = rand ? true : false;
                right = rand ? false : true;
            }
                
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
        
        // TODO: Steam needs to swap with fire
        private void SteamUpdate(Vector2Int position)
        {
            Particle thisCell = particleManager.GetParticle(position);
            Particle leftCell = particleManager.GetParticle(position + leftDirection);
            Particle rightCell = particleManager.GetParticle(position + rightDirection);
            Particle topCell = particleManager.GetParticle(position + topDirection);
            Particle topLeftCell = particleManager.GetParticle(position + topLeftDirection);
            Particle topRightCell = particleManager.GetParticle(position + topRightDirection);
            
            bool top = topCell.id == ParticleId.Air;
            bool topLeft = topLeftCell.id == ParticleId.Air;
            bool topRight = topRightCell.id == ParticleId.Air;
            bool left = leftCell.id == ParticleId.Air;
            bool right = rightCell.id == ParticleId.Air;

            //Makes steam dispersal random.
            if (!top && !topLeft && !topRight)
            {
                bool rand = Utilities.RandomChance(50);
                left = rand ? true : false;
                right = rand ? false : true;
            }

            if (/*topCell.state != ParticleState.Liquid && */topCell.weight > thisCell.weight)
                particleManager.SwapParticle(position, position + topDirection);
            // Randomise this or get rid of it
            // else if (topLeftCell.weight > thisCell.weight)
            //     particleManager.SwapParticle(position, position + topLeftDirection);
            // else if (topRightCell.weight > thisCell.weight)
            //     particleManager.SwapParticle(position, position + topRightDirection);
            
            if (top)
                particleManager.MoveParticle(position, topDirection);
            else if(topLeft)
                particleManager.MoveParticle(position, topLeftDirection);
            else if(topRight)
                particleManager.MoveParticle(position, topRightDirection);
            
            if(left)
                particleManager.MoveParticle(position, leftDirection);
            else if(right)
                particleManager.MoveParticle(position, rightDirection);
        }

        // TODO: Randomise fire direction
        private void FireUpdate(Vector2Int position)
        {
            Particle topLeftCell = particleManager.GetParticle(position + topLeftDirection);
            Particle topCell = particleManager.GetParticle(position + topDirection);
            Particle topRightCell = particleManager.GetParticle(position + topRightDirection);
            Particle rightCell = particleManager.GetParticle(position + rightDirection);
            Particle bottomRightCell = particleManager.GetParticle(position + bottomRightDirection);
            Particle bottomCell = particleManager.GetParticle(position + bottomDirection);
            Particle bottomLeftCell = particleManager.GetParticle(position + bottomLeftDirection);
            Particle leftCell = particleManager.GetParticle(position + leftDirection);
            
            bool top = topCell.id == ParticleId.Air;

            bool topLeftFlammable = topLeftCell.flammable;
            bool topFlammable = topCell.flammable;
            bool topRightFlammable = topRightCell.flammable;
            bool rightFlammable = rightCell.flammable;
            bool bottomRightFlammable = bottomRightCell.flammable;
            bool bottomFlammable = bottomCell.flammable;
            bool bottomLeftFlammable = bottomLeftCell.flammable;
            bool leftFlammable = leftCell.flammable;

            if (top)
                particleManager.MoveParticle(position, topDirection);

            int smokeChance = 20;
            
            // Set fire to flammable objects
            if (topLeftFlammable)
            {
                particleManager.PlaceParticle(position + topLeftDirection, ParticleTypes.Fire, true, true);
                particleManager.map[position.x + topLeftDirection.x, position.y + topLeftDirection.y].hasBeenUpdated = true;
                
                // Smoke
                if (particleManager.IsWithinMap(position + bottomRightDirection) && Utilities.RandomChance(smokeChance))
                {
                    particleManager.PlaceParticle(position + bottomRightDirection, ParticleTypes.Smoke, true, true);
                    particleManager.map[position.x + bottomRightDirection.x, position.y + bottomRightDirection.y].hasBeenUpdated = true;
                }
            }
            else if (topFlammable)
            {
                particleManager.PlaceParticle(position + topDirection, ParticleTypes.Fire, true, true);
                particleManager.map[position.x + topDirection.x, position.y + topDirection.y].hasBeenUpdated = true;
                
                // Smoke
                if (particleManager.IsWithinMap(position + bottomDirection) && Utilities.RandomChance(smokeChance))
                {
                    particleManager.PlaceParticle(position + bottomDirection, ParticleTypes.Smoke, true, true);
                    particleManager.map[position.x + bottomDirection.x, position.y + bottomDirection.y].hasBeenUpdated = true;
                }            
            }
            else if (topRightFlammable)
            {
                particleManager.PlaceParticle(position + topRightDirection, ParticleTypes.Fire, true, true);
                particleManager.map[position.x + topRightDirection.x, position.y + topRightDirection.y].hasBeenUpdated = true;
                
                // Smoke
                if (particleManager.IsWithinMap(position + bottomLeftDirection) && Utilities.RandomChance(smokeChance))
                {
                    particleManager.PlaceParticle(position + bottomLeftDirection, ParticleTypes.Smoke, true, true);
                    particleManager.map[position.x + bottomLeftDirection.x, position.y + bottomLeftDirection.y].hasBeenUpdated = true;
                }            
            }
            else if (rightFlammable)
            {
                particleManager.PlaceParticle(position + rightDirection, ParticleTypes.Fire, true, true);
                particleManager.map[position.x + rightDirection.x, position.y + rightDirection.y].hasBeenUpdated = true;
                
                // Smoke
                if (particleManager.IsWithinMap(position + leftDirection) && Utilities.RandomChance(smokeChance))

                {
                    particleManager.PlaceParticle(position + leftDirection, ParticleTypes.Smoke, true, true);
                    particleManager.map[position.x + leftDirection.x, position.y + leftDirection.y].hasBeenUpdated = true;
                }
            }
            else if (bottomRightFlammable)
            {
                particleManager.PlaceParticle(position + bottomRightDirection, ParticleTypes.Fire, true, true);
                particleManager.map[position.x + bottomRightDirection.x, position.y + bottomRightDirection.y].hasBeenUpdated = true;
                
                // Smoke
                if (particleManager.IsWithinMap(position + topLeftDirection) && Utilities.RandomChance(smokeChance))
                {
                    particleManager.PlaceParticle(position + topLeftDirection, ParticleTypes.Smoke, true, true);
                    particleManager.map[position.x + topLeftDirection.x, position.y + topLeftDirection.y].hasBeenUpdated = true;
                }
            }
            else if (bottomFlammable)
            {
                particleManager.PlaceParticle(position + bottomDirection, ParticleTypes.Fire, true, true);
                particleManager.map[position.x + bottomDirection.x, position.y + bottomDirection.y].hasBeenUpdated = true;
                
                // Smoke
                if (particleManager.IsWithinMap(position + topDirection) && Utilities.RandomChance(smokeChance))
                {
                    particleManager.PlaceParticle(position + topDirection, ParticleTypes.Smoke, true, true);
                    particleManager.map[position.x + topDirection.x, position.y + topDirection.y].hasBeenUpdated = true;
                }
            }
            else if (bottomLeftFlammable)
            {
                particleManager.PlaceParticle(position + bottomLeftDirection, ParticleTypes.Fire, true, true);
                particleManager.map[position.x + bottomLeftDirection.x, position.y + bottomLeftDirection.y].hasBeenUpdated = true;
                
                // Smoke
                if (particleManager.IsWithinMap(position + topRightDirection) && Utilities.RandomChance(smokeChance))
                {
                    particleManager.PlaceParticle(position + topRightDirection, ParticleTypes.Smoke, true, true);
                    particleManager.map[position.x + topRightDirection.x, position.y + topRightDirection.y].hasBeenUpdated = true;
                }
            }
            else if (leftFlammable)
            {
                particleManager.PlaceParticle(position + leftDirection, ParticleTypes.Fire, true, true);
                particleManager.map[position.x + leftDirection.x, position.y + leftDirection.y].hasBeenUpdated = true;
                
                // Smoke
                if (particleManager.IsWithinMap(position + rightDirection) && Utilities.RandomChance(smokeChance))
                {
                    particleManager.PlaceParticle(position + rightDirection, ParticleTypes.Smoke, true, true);
                    particleManager.map[position.x + rightDirection.x, position.y + rightDirection.y].hasBeenUpdated = true;
                }
            }
            
            // Make this work for oil eventually
            // Fire + Water = steam
            if (topLeftCell.id == ParticleId.Water)
            {
                particleManager.PlaceParticle(position + topLeftDirection, ParticleTypes.Steam, true, true);
                particleManager.map[position.x + topLeftDirection.x, position.y + topLeftDirection.y].hasBeenUpdated = true;
            }
            else if (topCell.id == ParticleId.Water)
            {
                particleManager.PlaceParticle(position + topDirection, ParticleTypes.Steam, true, true);
                particleManager.map[position.x + topDirection.x, position.y + topDirection.y].hasBeenUpdated = true;
            }
            else if (topRightCell.id == ParticleId.Water)
            {
                particleManager.PlaceParticle(position + topRightDirection, ParticleTypes.Steam, true, true);
                particleManager.map[position.x + topRightDirection.x, position.y + topRightDirection.y].hasBeenUpdated = true;
            }
            else if (rightCell.id == ParticleId.Water)
            {
                particleManager.PlaceParticle(position + rightDirection, ParticleTypes.Steam, true, true);
                particleManager.map[position.x + rightDirection.x, position.y + rightDirection.y].hasBeenUpdated = true;
            }
            else if (bottomRightCell.id == ParticleId.Water)
            {
                particleManager.PlaceParticle(position + bottomRightDirection, ParticleTypes.Steam, true, true);
                particleManager.map[position.x + bottomRightDirection.x, position.y + bottomRightDirection.y].hasBeenUpdated = true;
            }
            else if (bottomCell.id == ParticleId.Water)
            {
                particleManager.PlaceParticle(position + bottomDirection, ParticleTypes.Steam, true, true);
                particleManager.map[position.x + bottomDirection.x, position.y + bottomDirection.y].hasBeenUpdated = true;
            }
            else if (bottomLeftCell.id == ParticleId.Water)
            {
                particleManager.PlaceParticle(position + bottomLeftDirection, ParticleTypes.Steam, true, true);
                particleManager.map[position.x + bottomLeftDirection.x, position.y + bottomLeftDirection.y].hasBeenUpdated = true;
            }
            else if (leftCell.id == ParticleId.Water)
            {
                particleManager.PlaceParticle(position + leftDirection, ParticleTypes.Steam, true, true);
                particleManager.map[position.x + leftDirection.x, position.y + leftDirection.y].hasBeenUpdated = true;
            }
            
            // // Fire + Oil = Smoke
            // if (topLeftCell.id == ParticleId.Oil)
            // {
            //     particleManager.PlaceParticle(position, ParticleTypes.Smoke, false, true);
            //     particleManager.map[position.x, position.y].hasBeenUpdated = true;
            // }
            // else if (topCell.id == ParticleId.Oil)
            // {
            //     particleManager.PlaceParticle(position, ParticleTypes.Smoke, false, true);
            //     particleManager.map[position.x, position.y].hasBeenUpdated = true;
            // }
            // else if (topRightCell.id == ParticleId.Oil)
            // {
            //     particleManager.PlaceParticle(position, ParticleTypes.Smoke, false, true);
            //     particleManager.map[position.x, position.y].hasBeenUpdated = true;
            // }
            // else if (rightCell.id == ParticleId.Oil)
            // {
            //     particleManager.PlaceParticle(position, ParticleTypes.Smoke, false, true);
            //     particleManager.map[position.x, position.y].hasBeenUpdated = true;
            // }
            // else if (bottomRightCell.id == ParticleId.Oil)
            // {
            //     particleManager.PlaceParticle(position, ParticleTypes.Smoke, false, true);
            //     particleManager.map[position.x, position.y].hasBeenUpdated = true;
            // }
            // else if (bottomCell.id == ParticleId.Oil)
            // {
            //     particleManager.PlaceParticle(position, ParticleTypes.Smoke, false, true);
            //     particleManager.map[position.x, position.y].hasBeenUpdated = true;
            // }
            // else if (bottomLeftCell.id == ParticleId.Oil)
            // {
            //     particleManager.PlaceParticle(position, ParticleTypes.Smoke, false, true);
            //     particleManager.map[position.x, position.y].hasBeenUpdated = true;
            // }
            // else if (leftCell.id == ParticleId.Oil)
            // {
            //     particleManager.PlaceParticle(position, ParticleTypes.Smoke, false, true);
            //     particleManager.map[position.x, position.y].hasBeenUpdated = true;
            // }

        }
    }   
}