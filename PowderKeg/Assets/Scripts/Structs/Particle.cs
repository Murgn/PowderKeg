using UnityEngine;

namespace Murgn
{
    public struct Particle
    {
        public ParticleId id;
        public ParticleState state;
        public int lifetime;
        public Color color;
        public bool hasBeenUpdated;
        public float weight;
        public float dispersalChance;
        public int timer;
        public float discolouration;
        public bool flammable;
        public ParticleId extraId;
        public bool extraBool;
    }
}