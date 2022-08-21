using UnityEngine;

namespace Murgn
{
    public struct Particle
    {
        public ParticleId id;
        public float lifetime;
        public Color color;
        public bool hasBeenUpdated;
        public float weight;
        public float dispersalChance;
    }
}