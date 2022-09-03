using System.Collections.Generic;
using Murgn.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Murgn
{
    public class ParticleManager : Singleton<ParticleManager>
    {
        [Header("Map")]
        public Particle[,] map;
        
        public int width;
        public int height;

        public Vector2 mapOffset;
        
        [Header("Logic")]
        [SerializeField] private float updateSpeed;
        public bool paused;
        public int particleCount;
        public int maxParticleCount;
        private float updateTimer;

        public readonly Dictionary<ParticleId, Particle> particleIdLookup
            = new()
            {
                { ParticleId.Null, ParticleTypes.Null },
                { ParticleId.Air, ParticleTypes.Air },
                { ParticleId.Stone, ParticleTypes.Stone },
                { ParticleId.Dirt, ParticleTypes.Dirt },
                { ParticleId.Grass, ParticleTypes.Grass },
                { ParticleId.Sand, ParticleTypes.Sand },
                { ParticleId.Water, ParticleTypes.Water },
                { ParticleId.Steam, ParticleTypes.Steam },
                { ParticleId.Fire, ParticleTypes.Fire },
                { ParticleId.Wood, ParticleTypes.Wood },
                { ParticleId.Oil, ParticleTypes.Oil },
                { ParticleId.Smoke, ParticleTypes.Smoke },
                { ParticleId.Acid, ParticleTypes.Acid },
                { ParticleId.Glass, ParticleTypes.Glass },
                { ParticleId.Lava, ParticleTypes.Lava },
                { ParticleId.AcidGas, ParticleTypes.AcidGas },
                { ParticleId.Clone, ParticleTypes.Clone },
                { ParticleId.Output, ParticleTypes.Output },
                { ParticleId.Void, ParticleTypes.Void },
                { ParticleId.Gunpowder, ParticleTypes.Gunpowder },
                { ParticleId.Grenade, ParticleTypes.Grenade },
                { ParticleId.Bomb, ParticleTypes.Bomb },
                { ParticleId.Seed, ParticleTypes.Seed },
                { ParticleId.Plant, ParticleTypes.Plant },
            };

        private new void Awake()
        {
            base.Awake();
            map = new Particle[width, height];
            mapOffset = new Vector2(-width / 2.0f, -height / 2.0f);
            
            // Disable debug updater
            UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    map[x, y] = ParticleTypes.Air;
                }
            }

            maxParticleCount = width * height;
        }

        private void Update()
        {
            if (updateTimer <= Time.time)
            {
                particleCount = 0;
                if(!paused) EventManager.Update?.Invoke();
                EventManager.Render?.Invoke();
                
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        map[x, y].hasBeenUpdated = false;
                    }
                }

                updateTimer = updateSpeed + Time.time;
            }

            if (Keyboard.current.spaceKey.wasPressedThisFrame) paused = !paused;
        }

        public bool PlaceParticle(Vector2Int position, Particle particle, bool unsafeMode = false, bool changeColor = false)
        {
            // Checks if within map borders
            if (IsWithinMap(position))
            {
                // If cell is empty place particle
                if (map[position.x, position.y].id == ParticleId.Air || unsafeMode)
                {
                    map[position.x, position.y] = particle;
                    float discolouration = map[position.x, position.y].discolouration;
                    map[position.x, position.y].color += changeColor ? new Color(Random.Range(-discolouration, discolouration), Random.Range(-discolouration, discolouration), Random.Range(-discolouration, discolouration)) : Color.black;
                    return true;
                }
            }
            return false;
        }

        public bool MoveParticle(Vector2Int position, Vector2Int direction)
        {
            Particle particle = GetParticle(position);
            bool placed = PlaceParticle(position + direction, particle);
            PlaceParticle(position, ParticleTypes.Air, placed);

            return placed;
        }
        
        public bool SwapParticle(Vector2Int position, Vector2Int otherPosition)
        {
            Particle particle = GetParticle(position);
            Particle otherParticle = GetParticle(otherPosition);
            PlaceParticle(position, ParticleTypes.Air, true);
            PlaceParticle(otherPosition, ParticleTypes.Air, true);
            
            PlaceParticle(position, otherParticle, true);
            return PlaceParticle(otherPosition, particle, true);
        }

        public Particle GetParticle(Vector2Int position) => IsWithinMap(position) ? map[position.x, position.y] : ParticleTypes.Null;
        
        public bool DestroyParticle(Vector2Int position) => PlaceParticle(position, ParticleTypes.Air, true);
        
        public bool IsWithinMap(Vector2Int position) => 0 <= position.x && position.x <= width - 1 
                                                     && 0 <= position.y && position.y <= height - 1;
    }
}