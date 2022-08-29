using UnityEngine;

namespace Murgn
{
    public struct ParticleTypes
    {
        public static readonly Particle Null = new()
        {
            id = ParticleId.Null,
            state = ParticleState.Solid,
            lifetime = -1,
            color = Color.black,
            // active = true,
            weight = 0,
            dispersalChance = 0
        };
        
        public static readonly Particle Air = new()
        {
            id = ParticleId.Air,
            state = ParticleState.Gas,
            lifetime = -1,
            color = Color.black,
            // active = true,
            weight = 0,
            dispersalChance = 0
        };
        
        public static readonly Particle Stone = new()
        {
            id = ParticleId.Stone,
            state = ParticleState.Solid,
            lifetime = -1,
            color = Color.grey,
            // active = true,
            weight = 0,
            dispersalChance = 0,
            discolouration = 0.005f
        };
        
        public static readonly Particle Dirt = new()
        {
            id = ParticleId.Dirt,
            state = ParticleState.Solid,
            lifetime = -1,
            color = new Color(0.47f, 0.29f, 0.2f),
            // active = true,
            weight = 3,
            dispersalChance = 0.5f,
            timer = 100,
            discolouration = 0.01f
        };
        
        public static readonly Particle Grass = new()
        {
            id = ParticleId.Grass,
            state = ParticleState.Solid,
            lifetime = -1,
            color = new Color(0.32f, 0.89f, 0.18f),
            // active = true,
            weight = 3,
            dispersalChance = 0.5f,
            timer = 50,
            discolouration = 0.03f
        };
        
        public static readonly Particle Sand = new()
        {
            id = ParticleId.Sand,
            state = ParticleState.Solid,
            lifetime = -1,
            color = new Color(0.98f, 0.76f, 0.17f),
            // active = true,
            weight = 2,
            dispersalChance = 0.7f,
            discolouration = 0.03f
            
        };
        
        public static readonly Particle Water = new()
        {
            id = ParticleId.Water,
            state = ParticleState.Liquid,
            lifetime = -1,
            color = new Color(0.1f, 0.33f, 0.88f),
            // active = true,
            weight = 1,
            dispersalChance = 0.9f,
            discolouration = 0.015f
        };
        
        public static readonly Particle Steam = new()
        {
            id = ParticleId.Steam,
            state = ParticleState.Gas,
            lifetime = -1,
            color = new Color(0.78f, 0.84f, 0.86f),
            // active = true,
            weight = 0,
            dispersalChance = 0.9f,
            discolouration = 0.01f
        };
        
        public static readonly Particle Fire = new()
        {
            id = ParticleId.Fire,
            state = ParticleState.Plasma,
            lifetime = 10,
            color = Color.red,
            // active = true,
            weight = 0,
            dispersalChance = 0.925f,
            discolouration = 0.01f
        };
        
        public static readonly Particle Wood = new()
        {
            id = ParticleId.Wood,
            state = ParticleState.Solid,
            lifetime = -1,
            color = new Color(0.58f, 0.39f, 0.2f),
            // active = true,
            weight = 0,
            dispersalChance = 0,
            discolouration = 0.005f,
            flammable = true
        };
    }
}