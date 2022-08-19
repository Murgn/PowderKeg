using System;
using Murgn.Utilities;
using UnityEngine;

namespace Murgn
{
    public class ParticleManager : Singleton<ParticleManager>
    {
        [Header("Map")]
        public byte[,] map;
        
        public int width;
        public int height;

        public Vector2 offset;
        
        [Header("Logic")]
        [SerializeField] private float updateSpeed;
        private float updateTimer;

        private new void Awake()
        {
            base.Awake();
            map = new byte[width, height];
            offset = new Vector2(-width / 2.0f, -height / 2.0f);
        }

        private void Update()
        {
            if (updateTimer <= Time.time)
            {
                EventManager.Update?.Invoke();
                updateTimer = updateSpeed + Time.time;
            }
        }

        public bool PlaceParticle(Vector2Int position, ParticleId particleId, bool unsafeMode = false)
        {
            // Checks if within map borders
            if (IsWithinMap(position))
            {
                // If cell is empty place particle
                if (map[position.x, position.y] == 0 || unsafeMode)
                {
                    map[position.x, position.y] = (byte)particleId;
                    //Debug.Log($"Placed particle {particleId} at X: {position.x}, Y: {position.y}");
                    return true;
                }
            }
            return false;
        }

        public bool MoveParticle(Vector2Int position, Vector2Int direction)
        {
            ParticleId particle = GetParticle(position);
            bool placed = PlaceParticle(position + direction, particle);
            PlaceParticle(position, ParticleId.Air, placed);

            return placed;
        }

        public ParticleId GetParticle(Vector2Int position)
        {
            if(IsWithinMap(position))
                return (ParticleId)map[position.x, position.y];

            return ParticleId.Null;
        }
        
        public bool IsWithinMap(Vector2Int position) => 0 <= position.x && position.x <= width - 1 
                                                     && 0 <= position.y && position.y <= height -1;
    }
}