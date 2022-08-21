using UnityEngine;

namespace Murgn
{
    public struct ParticleTypes
    {
        public static readonly Particle Null = new()
        {
            id = ParticleId.Null,
            lifetime = -1,
            color = Color.black,
            weight = 0,
            dispersalChance = 0
        };
        
        public static readonly Particle Air = new()
        {
            id = ParticleId.Air,
            lifetime = -1,
            color = Color.white,
            weight = 0,
            dispersalChance = 0
        };
        
        public static readonly Particle Block = new()
        {
            id = ParticleId.Block,
            lifetime = -1,
            color = Color.grey,
            weight = 0,
            dispersalChance = 0
        };
        
        public static readonly Particle Dirt = new()
        {
            id = ParticleId.Dirt,
            lifetime = -1,
            color = new Color(0.38f, 0.267f, 0.137f),
            weight = 3,
            dispersalChance = 0.5f
        };
        
        public static readonly Particle Sand = new()
        {
            id = ParticleId.Sand,
            lifetime = -1,
            color = Color.yellow,
            weight = 2
            ,
            dispersalChance = 0.7f
        };
        
        public static readonly Particle Water = new()
        {
            id = ParticleId.Water,
            lifetime = -1,
            color = Color.blue,
            weight = 1
            ,
            dispersalChance = 0.9f
        };
        
        public static readonly Particle Steam = new()
        {
            id = ParticleId.Steam,
            lifetime = -1,
            color = Color.cyan,
            weight = 0
            ,
            dispersalChance = 0.9f
        };
    }
}