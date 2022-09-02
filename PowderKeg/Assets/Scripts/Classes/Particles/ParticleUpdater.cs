using Murgn.Utils;
using UnityEngine;
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
                        
                        if(particleManager.map[x, y].id != ParticleId.Air)
                            particleManager.particleCount++;
                        
                        switch (particleManager.map[x, y].id)
                        {
                            // Empty or static objects don't need any logic

                            case ParticleId.Dirt:
                                // Dirt Update
                                DirtUpdate(position);
                                break;
                            
                            case ParticleId.Grass:
                                // Grass Update
                                GrassUpdate(position);
                                break;

                            case ParticleId.Sand:
                                // Sand Update
                                SandUpdate(position);
                                break;

                            case ParticleId.Water:
                            case ParticleId.Oil:
                                // Water & Oil Update
                                WaterUpdate(position);
                                break;
                            
                            case ParticleId.Acid:
                                // Acid Update
                                AcidUpdate(position);
                                break;
                            
                            case ParticleId.Lava:
                                // Lava Update
                                LavaUpdate(position);
                                break;

                            case ParticleId.Steam:
                                // Steam Update
                                SteamUpdate(position);
                                break;
                            
                            case ParticleId.Smoke:
                            case ParticleId.AcidGas:
                                // Smoke & AcidGas Update
                                SmokeUpdate(position);
                                break;

                            case ParticleId.Fire:
                                // Fire Update
                                FireUpdate(position);
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
            else if(Utilities.RandomChance(50))
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
            {
                particleManager.PlaceParticle(position, ParticleTypes.Grass, true, true);
                float discolouration = particleManager.map[position.x, position.y].discolouration;
                particleManager.map[position.x, position.y].color += new Color(
                    Random.Range(-discolouration, discolouration), Random.Range(-discolouration, discolouration),
                    Random.Range(-discolouration, discolouration));
            }

            if (bottom)
                particleManager.MoveParticle(position, bottomDirection);
        }

        private void GrassUpdate(Vector2Int position)
        {
            Particle topLeftCell = particleManager.GetParticle(position + topLeftDirection);
            Particle topCell = particleManager.GetParticle(position + topDirection);
            Particle topRightCell = particleManager.GetParticle(position + topRightDirection);
            Particle bottomCell = particleManager.GetParticle(position + bottomDirection);

            bool top = topCell.state != ParticleState.Solid;
            bool bottom = bottomCell.state != ParticleState.Solid;

            if (top)
                particleManager.map[position.x, position.y].timer = 50;
            else
            {
                particleManager.map[position.x, position.y].timer -= Utilities.RandomChance(50) ? 1 : 0;
            }
            
            if (particleManager.map[position.x, position.y].timer <= 0)
            {
                particleManager.PlaceParticle(position, ParticleTypes.Dirt, true, true);
            }
            
            if (bottom)
                particleManager.MoveParticle(position, bottomDirection);
            
            // Fire burns grass
            if (topCell.id == ParticleId.Fire || topLeftCell.id == ParticleId.Fire || topRightCell.id == ParticleId.Fire)
                particleManager.map[position.x, position.y].color = new Color(0.07f, 0.33f, 0.04f);
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
                bottomLeft = rand;
                bottomRight = !rand;
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
                left = rand;
                right = !rand;
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
        
        private void AcidUpdate(Vector2Int position)
        {
            Particle thisCell = particleManager.GetParticle(position);
            Particle topCell = particleManager.GetParticle(position + topDirection);
            Particle leftCell = particleManager.GetParticle(position + leftDirection);
            Particle rightCell = particleManager.GetParticle(position + rightDirection);
            Particle bottomCell = particleManager.GetParticle(position + bottomDirection);
            Particle bottomLeftCell = particleManager.GetParticle(position + bottomLeftDirection);
            Particle bottomRightCell = particleManager.GetParticle(position + bottomRightDirection);
            
            bool bottom = bottomCell.id == ParticleId.Air || bottomCell.id == ParticleId.AcidGas;
            bool bottomLeft = bottomLeftCell.id == ParticleId.Air || bottomLeftCell.id == ParticleId.AcidGas;
            bool bottomRight = bottomRightCell.id == ParticleId.Air || bottomRightCell.id == ParticleId.AcidGas;
            bool left = leftCell.id == ParticleId.Air || leftCell.id == ParticleId.AcidGas;
            bool right = rightCell.id == ParticleId.Air || leftCell.id == ParticleId.AcidGas;

            if (topCell.weight > thisCell.weight)
                particleManager.SwapParticle(position, position + topDirection);

            if (!bottom && !bottomLeft && !bottomRight)
            {
                bool rand = Utilities.RandomChance(50);
                left = rand;
                right = !rand;
            }

            const int gasChance = 5;
            bool useGas = Utilities.RandomChance(gasChance);

            if(topCell.state != ParticleState.Gas && topCell.id != ParticleId.Stone && topCell.id != ParticleId.Acid && topCell.id != ParticleId.AcidGas)
            {
                bool placed = particleManager.PlaceParticle(position + topDirection,
                    useGas ? ParticleTypes.AcidGas : ParticleTypes.Air, true);
                    
                if(placed && useGas)
                {
                    float discolouration = particleManager.map[position.x + topDirection.x, position.y + topDirection.y].discolouration;
                    particleManager.map[position.x + topDirection.x, position.y + topDirection.y].color += new Color(
                        Random.Range(-discolouration, discolouration), Random.Range(-discolouration, discolouration),
                        Random.Range(-discolouration, discolouration));
                }
            }            
            if(leftCell.state != ParticleState.Gas && leftCell.id != ParticleId.Stone && leftCell.id != ParticleId.Acid && leftCell.id != ParticleId.AcidGas)
            {
                bool placed = particleManager.PlaceParticle(position + leftDirection,
                    useGas ? ParticleTypes.AcidGas : ParticleTypes.Air, true);
                    
                if(placed && useGas)
                {
                    float discolouration = particleManager.map[position.x + leftDirection.x, position.y + leftDirection.y].discolouration;
                    particleManager.map[position.x + leftDirection.x, position.y + leftDirection.y].color += new Color(
                        Random.Range(-discolouration, discolouration), Random.Range(-discolouration, discolouration),
                        Random.Range(-discolouration, discolouration));
                }
            }            
            if(rightCell.state != ParticleState.Gas && rightCell.id != ParticleId.Stone && rightCell.id != ParticleId.Acid && rightCell.id != ParticleId.AcidGas)
            {
                bool placed = particleManager.PlaceParticle(position + rightDirection,
                    useGas ? ParticleTypes.AcidGas : ParticleTypes.Air, true);
                    
                if(placed && useGas)
                {
                    float discolouration = particleManager.map[position.x + rightDirection.x, position.y + rightDirection.y].discolouration;
                    particleManager.map[position.x + rightDirection.x, position.y + rightDirection.y].color += new Color(
                        Random.Range(-discolouration, discolouration), Random.Range(-discolouration, discolouration),
                        Random.Range(-discolouration, discolouration));
                }
            }            
            if(bottomCell.state != ParticleState.Gas && bottomCell.id != ParticleId.Stone && bottomCell.id != ParticleId.Acid && bottomCell.id != ParticleId.AcidGas)
            {
                bool placed = particleManager.PlaceParticle(position + bottomDirection,
                    useGas ? ParticleTypes.AcidGas : ParticleTypes.Air, true);
                    
                if(placed && useGas)
                {
                    float discolouration = particleManager.map[position.x + bottomDirection.x, position.y + bottomDirection.y].discolouration;
                    particleManager.map[position.x + bottomDirection.x, position.y + bottomDirection.y].color += new Color(
                        Random.Range(-discolouration, discolouration), Random.Range(-discolouration, discolouration),
                        Random.Range(-discolouration, discolouration));
                }
            }
            // Move
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
        
        // Todo: water -> steam
        private void LavaUpdate(Vector2Int position)
        {
            Particle thisCell = particleManager.GetParticle(position);
            Particle topCell = particleManager.GetParticle(position + topDirection);
            Particle leftCell = particleManager.GetParticle(position + leftDirection);
            Particle rightCell = particleManager.GetParticle(position + rightDirection);
            Particle bottomCell = particleManager.GetParticle(position + bottomDirection);
            Particle bottomLeftCell = particleManager.GetParticle(position + bottomLeftDirection);
            Particle bottomRightCell = particleManager.GetParticle(position + bottomRightDirection);
            
            bool bottom = bottomCell.id == ParticleId.Air || bottomCell.id == ParticleId.Fire;
            bool bottomLeft = bottomLeftCell.id == ParticleId.Air || bottomLeftCell.id == ParticleId.Fire;
            bool bottomRight = bottomRightCell.id == ParticleId.Air || bottomRightCell.id == ParticleId.Fire;
            bool left = leftCell.id == ParticleId.Air || leftCell.id == ParticleId.Fire;
            bool right = rightCell.id == ParticleId.Air || leftCell.id == ParticleId.Fire;

            if (topCell.weight > thisCell.weight)
                particleManager.SwapParticle(position, position + topDirection);

            if (!bottom && !bottomLeft && !bottomRight)
            {
                bool rand = Utilities.RandomChance(50);
                left = rand;
                right = !rand;
            }

            const int smokeChance = 20;
            bool useSmoke = Utilities.RandomChance(smokeChance);
            const int steamChance = 50;
            bool useSteam = Utilities.RandomChance(steamChance);
            
            if(topCell.state != ParticleState.Gas && topCell.id != ParticleId.Stone && topCell.id != ParticleId.Lava && topCell.id != ParticleId.Fire)
            {
                if (topCell.flammable)
                    particleManager.PlaceParticle(position + topDirection, ParticleTypes.Fire, true);
                else if(topCell.id == ParticleId.Water && useSteam)
                    particleManager.PlaceParticle(position + topDirection, ParticleTypes.Steam, true);
                else if(topCell.id != ParticleId.Water && useSmoke)
                    particleManager.PlaceParticle(position + topDirection, ParticleTypes.Smoke, true);
            }
            
            if(leftCell.state != ParticleState.Gas && leftCell.id != ParticleId.Stone && leftCell.id != ParticleId.Lava && leftCell.id != ParticleId.Fire)
            {
                if (leftCell.flammable)
                    particleManager.PlaceParticle(position + leftDirection, ParticleTypes.Fire, true);
                else if(leftCell.id == ParticleId.Water && useSteam)
                    particleManager.PlaceParticle(position + leftDirection, ParticleTypes.Steam, true);
                else if(leftCell.id != ParticleId.Water && useSmoke)
                    particleManager.PlaceParticle(position + leftDirection, ParticleTypes.Smoke, true);
            }
            
            if(rightCell.state != ParticleState.Gas && rightCell.id != ParticleId.Stone && rightCell.id != ParticleId.Lava && rightCell.id != ParticleId.Fire)
            {
                if (rightCell.flammable)
                    particleManager.PlaceParticle(position + rightDirection, ParticleTypes.Fire, true);
                else if(rightCell.id == ParticleId.Water && useSteam)
                    particleManager.PlaceParticle(position + rightDirection, ParticleTypes.Steam, true);
                else if(rightCell.id != ParticleId.Water && useSmoke)
                    particleManager.PlaceParticle(position + rightDirection, ParticleTypes.Smoke, true);
            }
            
            if(bottomCell.state != ParticleState.Gas && bottomCell.id != ParticleId.Stone && bottomCell.id != ParticleId.Lava && bottomCell.id != ParticleId.Fire)
            {
                if (bottomCell.flammable)
                    particleManager.PlaceParticle(position + bottomDirection, ParticleTypes.Fire, true);
                else if(bottomCell.id == ParticleId.Water && useSteam)
                    particleManager.PlaceParticle(position + bottomDirection, ParticleTypes.Steam, true);
                else if(bottomCell.id != ParticleId.Water && useSmoke)
                    particleManager.PlaceParticle(position + bottomDirection, ParticleTypes.Smoke, true);
            }

            // Move
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
            Particle thisCell = particleManager.GetParticle(position);
            Particle topLeftCell = particleManager.GetParticle(position + topLeftDirection);
            Particle topCell = particleManager.GetParticle(position + topDirection);
            Particle topRightCell = particleManager.GetParticle(position + topRightDirection);
            Particle rightCell = particleManager.GetParticle(position + rightDirection);
            Particle bottomRightCell = particleManager.GetParticle(position + bottomRightDirection);
            Particle bottomCell = particleManager.GetParticle(position + bottomDirection);
            Particle bottomLeftCell = particleManager.GetParticle(position + bottomLeftDirection);
            Particle leftCell = particleManager.GetParticle(position + leftDirection);
            
            bool top = topCell.id == ParticleId.Air;
            bool topLeft = topLeftCell.id == ParticleId.Air;
            bool topRight = topRightCell.id == ParticleId.Air;
            bool left = leftCell.id == ParticleId.Air;
            bool right = rightCell.id == ParticleId.Air;
            bool bottom = bottomCell.id == ParticleId.Air;

            int nearbySteam = 0;
            
            // Count nearby steam
            if(topLeftCell.id == ParticleId.Steam) nearbySteam++;
            if(topCell.id == ParticleId.Steam) nearbySteam++;
            if(topRightCell.id == ParticleId.Steam) nearbySteam++;
            if(rightCell.id == ParticleId.Steam) nearbySteam++;
            if(bottomRightCell.id == ParticleId.Steam) nearbySteam++;
            if(bottomCell.id == ParticleId.Steam) nearbySteam++;
            if(bottomLeftCell.id == ParticleId.Steam) nearbySteam++;
            if(leftCell.id == ParticleId.Steam) nearbySteam++;

            if (!particleManager.IsWithinMap(position + topDirection)) nearbySteam += 3;
            
            // Change color based on nearby fire
            float discolouration = particleManager.map[position.x, position.y].discolouration;
            switch (nearbySteam)
            {
                case 1:
                case 2:
                case 3:
                    particleManager.map[position.x, position.y].color = SteamColors.oneNearby + new Color(Random.Range(-discolouration, discolouration), Random.Range(-discolouration, discolouration), 0);
                    break;
                
                case 4:
                case 5:
                    particleManager.map[position.x, position.y].color = SteamColors.twoNearby + new Color(Random.Range(-discolouration, discolouration), Random.Range(-discolouration, discolouration), 0);
                    break;
                
                case 6:
                case 7:
                case 8:
                    particleManager.map[position.x, position.y].color = SteamColors.threeNearby + new Color(Random.Range(-discolouration, discolouration), Random.Range(-discolouration, discolouration), 0);
                    break;
            }
            // If there is nothing above the cell, and something below the cell
            if (nearbySteam >= 6 && bottom)
            {
                particleManager.map[position.x, position.y].timer -= Utilities.RandomChance(nearbySteam) ? 1 : 0;
            }
            // else
            //     particleManager.map[position.x, position.y].timer = 5;

            if (particleManager.map[position.x, position.y].timer <= 0)
                particleManager.PlaceParticle(position, ParticleTypes.Water, true, true);
            
            //Makes steam dispersal random.
            if (!top && !topLeft && !topRight)
            {
                bool rand = Utilities.RandomChance(50);
                left = rand;
                right = !rand;
            }

            if (topCell.weight > thisCell.weight)
                particleManager.SwapParticle(position, position + topDirection);
            

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
        
        private void SmokeUpdate(Vector2Int position)
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

            //Makes smoke dispersal random.
            if (!top && !topLeft && !topRight)
            {
                bool rand = Utilities.RandomChance(50);
                left = rand;
                right = !rand;
            }

            if (topCell.weight > thisCell.weight)
                particleManager.SwapParticle(position, position + topDirection);

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

            int nearbyFlames = 0;
            
            // Count nearby fire
            if(topLeftCell.id == ParticleId.Fire) nearbyFlames++;
            if(topCell.id == ParticleId.Fire) nearbyFlames++;
            if(topRightCell.id == ParticleId.Fire) nearbyFlames++;
            if(rightCell.id == ParticleId.Fire) nearbyFlames++;
            if(bottomRightCell.id == ParticleId.Fire) nearbyFlames++;
            if(bottomCell.id == ParticleId.Fire) nearbyFlames++;
            if(bottomLeftCell.id == ParticleId.Fire) nearbyFlames++;
            if(leftCell.id == ParticleId.Fire) nearbyFlames++;
            
            // Change color based on nearby fire
            float discolouration = particleManager.map[position.x, position.y].discolouration;
            switch (nearbyFlames)
            {
                default:
                    particleManager.map[position.x, position.y].color = FireColors.red + new Color(Random.Range(-discolouration, discolouration), Random.Range(-discolouration, discolouration), 0);
                    break;
                
                case 3:
                case 4:
                    particleManager.map[position.x, position.y].color = FireColors.orange + new Color(Random.Range(-discolouration, discolouration), Random.Range(-discolouration, discolouration), 0);
                    break;
                
                case 5:
                case 6:
                    particleManager.map[position.x, position.y].color = FireColors.brightOrange + new Color(Random.Range(-discolouration, discolouration), Random.Range(-discolouration, discolouration), 0);
                    break;
                
                case 7:
                    particleManager.map[position.x, position.y].color = FireColors.yellow + new Color(Random.Range(-discolouration, discolouration), Random.Range(-discolouration, discolouration), 0);
                    break;
                
                case 8:
                    particleManager.map[position.x, position.y].color = FireColors.brightYellow + new Color(Random.Range(-discolouration, discolouration), Random.Range(-discolouration, discolouration), 0);
                    break;
            }
            
            if (top)
                particleManager.MoveParticle(position, topDirection);

            const int smokeChance = 20;
            bool useSmoke = Utilities.RandomChance(smokeChance);
            const int steamChance = 50;
            bool useSteam = Utilities.RandomChance(steamChance);
            
            // Set fire to flammable objects
            if (topLeftFlammable)
            {
                particleManager.PlaceParticle(position + topLeftDirection, ParticleTypes.Fire, true, true);
                particleManager.map[position.x + topLeftDirection.x, position.y + topLeftDirection.y].hasBeenUpdated = true;
                
                // Smoke
                if (particleManager.IsWithinMap(position + bottomRightDirection) && useSmoke)
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
                if (particleManager.IsWithinMap(position + bottomDirection) && useSmoke)
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
                if (particleManager.IsWithinMap(position + bottomLeftDirection) && useSmoke)
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
                if (particleManager.IsWithinMap(position + leftDirection) && useSmoke)

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
                if (particleManager.IsWithinMap(position + topLeftDirection) && useSmoke)
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
                if (particleManager.IsWithinMap(position + topDirection) && useSmoke)
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
                if (particleManager.IsWithinMap(position + topRightDirection) && useSmoke)
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
                if (particleManager.IsWithinMap(position + rightDirection) && useSmoke)
                {
                    particleManager.PlaceParticle(position + rightDirection, ParticleTypes.Smoke, true, true);
                    particleManager.map[position.x + rightDirection.x, position.y + rightDirection.y].hasBeenUpdated = true;
                }
            }
            
            // Fire + Water = steam
            if (topLeftCell.id == ParticleId.Water)
            {
                particleManager.PlaceParticle(position + topLeftDirection, useSteam ? ParticleTypes.Steam : ParticleTypes.Air, true, true);
                particleManager.map[position.x + topLeftDirection.x, position.y + topLeftDirection.y].hasBeenUpdated = true;
            }
            else if (topCell.id == ParticleId.Water)
            {
                particleManager.PlaceParticle(position + topDirection, useSteam ? ParticleTypes.Steam : ParticleTypes.Air, true, true);
                particleManager.map[position.x + topDirection.x, position.y + topDirection.y].hasBeenUpdated = true;
            }
            else if (topRightCell.id == ParticleId.Water)
            {
                particleManager.PlaceParticle(position + topRightDirection, useSteam ? ParticleTypes.Steam : ParticleTypes.Air, true, true);
                particleManager.map[position.x + topRightDirection.x, position.y + topRightDirection.y].hasBeenUpdated = true;
            }
            else if (rightCell.id == ParticleId.Water)
            {
                particleManager.PlaceParticle(position + rightDirection, useSteam ? ParticleTypes.Steam : ParticleTypes.Air, true, true);
                particleManager.map[position.x + rightDirection.x, position.y + rightDirection.y].hasBeenUpdated = true;
            }
            else if (bottomRightCell.id == ParticleId.Water)
            {
                particleManager.PlaceParticle(position + bottomRightDirection, useSteam ? ParticleTypes.Steam : ParticleTypes.Air, true, true);
                particleManager.map[position.x + bottomRightDirection.x, position.y + bottomRightDirection.y].hasBeenUpdated = true;
            }
            else if (bottomCell.id == ParticleId.Water)
            {
                particleManager.PlaceParticle(position + bottomDirection, useSteam ? ParticleTypes.Steam : ParticleTypes.Air, true, true);
                particleManager.map[position.x + bottomDirection.x, position.y + bottomDirection.y].hasBeenUpdated = true;
            }
            else if (bottomLeftCell.id == ParticleId.Water)
            {
                particleManager.PlaceParticle(position + bottomLeftDirection, useSteam ? ParticleTypes.Steam : ParticleTypes.Air, true, true);
                particleManager.map[position.x + bottomLeftDirection.x, position.y + bottomLeftDirection.y].hasBeenUpdated = true;
            }
            else if (leftCell.id == ParticleId.Water)
            {
                particleManager.PlaceParticle(position + leftDirection, useSteam ? ParticleTypes.Steam : ParticleTypes.Air, true, true);
                particleManager.map[position.x + leftDirection.x, position.y + leftDirection.y].hasBeenUpdated = true;
            }
        }
    }   
}