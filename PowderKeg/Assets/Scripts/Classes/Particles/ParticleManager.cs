using System;
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

        public Vector2 offset;
        
        [Header("Logic")]
        [SerializeField] private float updateSpeed;
        [SerializeField] private bool paused;
        public int particleCount;
        private float updateTimer;

        private new void Awake()
        {
            base.Awake();
            map = new Particle[width, height];
            offset = new Vector2(-width / 2.0f, -height / 2.0f);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    map[x, y] = ParticleTypes.Air;
                }
            }
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
                    map[position.x, position.y].color = map[position.x, position.y].color + (changeColor ? new Color(Random.Range(-discolouration, discolouration), Random.Range(-discolouration, discolouration), Random.Range(-discolouration, discolouration)) : Color.black);
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

        public Particle GetParticle(Vector2Int position)
        {
            if(IsWithinMap(position))
                return map[position.x, position.y];

            return ParticleTypes.Null;
        }
        
        public bool DestroyParticle(Vector2Int position) => PlaceParticle(position, ParticleTypes.Air, true);
        
        public bool IsWithinMap(Vector2Int position) => 0 <= position.x && position.x <= width - 1 
                                                     && 0 <= position.y && position.y <= height - 1;
    }
}