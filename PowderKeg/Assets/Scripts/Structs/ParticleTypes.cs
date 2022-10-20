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
            weight = 0,
            dispersalChance = 0
        };
        
        public static readonly Particle Air = new()
        {
            id = ParticleId.Air,
            state = ParticleState.Gas,
            lifetime = -1,
            color = Color.white,
            weight = 0,
            dispersalChance = 0
        };
        
        public static readonly Particle Stone = new()
        {
            id = ParticleId.Stone,
            state = ParticleState.Solid,
            lifetime = -1,
            color = Color.grey,
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
            weight = 2,
            dispersalChance = 0.7f,
            discolouration = 0.03f
        };
        
        public static readonly Particle Glass = new()
        {
            id = ParticleId.Glass,
            state = ParticleState.Solid,
            lifetime = -1,
            color = new Color(0.98f, 0.76f, 0.17f),
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
            weight = 1,
            dispersalChance = 0.9f,
            discolouration = 0.015f
        };
        
        public static readonly Particle Steam = new()
        {
            id = ParticleId.Steam,
            state = ParticleState.Gas,
            lifetime = -1,
            color = new Color(0.82f, 0.88f, 0.9f),
            weight = 0.1f,
            dispersalChance = 0.9f,
            discolouration = 0.01f,
            timer = 15,
        };
        
        public static readonly Particle Fire = new()
        {
            id = ParticleId.Fire,
            state = ParticleState.Plasma,
            lifetime = 10,
            color = new Color(1f, 0.08f, 0.08f),
            weight = 0.2f,
            dispersalChance = 0.925f,
            discolouration = 0.03f
        };
        
        public static readonly Particle Wood = new()
        {
            id = ParticleId.Wood,
            state = ParticleState.Solid,
            lifetime = -1,
            color = new Color(0.58f, 0.39f, 0.2f),
            weight = 0,
            dispersalChance = 0,
            discolouration = 0.005f,
            flammable = true
        };
        
        public static readonly Particle Oil = new()
        {
            id = ParticleId.Oil,
            state = ParticleState.Liquid,
            lifetime = -1,
            color = new Color(0.22f, 0.0f, 0.22f),
            weight = 0.5f,
            dispersalChance = 0.9f,
            discolouration = 0.01f,
            flammable = true
        };
        
        public static readonly Particle Smoke = new()
        {
            id = ParticleId.Smoke,
            state = ParticleState.Gas,
            lifetime = -1,
            color = new Color(0.18f, 0.2f, 0.2f),
            weight = 0,
            dispersalChance = 0.9f,
            discolouration = 0.01f
        };
        
        public static readonly Particle Acid = new()
        {
            id = ParticleId.Acid,
            state = ParticleState.Liquid,
            lifetime = -1,
            color = new Color(0.12f, 0.86f, 0.18f),
            weight = 99f,
            dispersalChance = 0.8f,
            discolouration = 0.02f,
        };
        
        public static readonly Particle AcidGas = new()
        {
            id = ParticleId.AcidGas,
            state = ParticleState.Gas,
            lifetime = -1,
            color = new Color(0.54f, 0.86f, 0.61f),
            weight = 0.05f,
            dispersalChance = 0.9f,
            discolouration = 0.01f
        };

        public static readonly Particle Lava = new()
        {
            id = ParticleId.Lava,
            state = ParticleState.Liquid,
            lifetime = -1,
            color = new Color(0.98f, 0.42f, 0.11f),
            weight = 99f,
            dispersalChance = 0.7f,
            discolouration = 0.02f,
        };
        
        public static readonly Particle Clone = new()
        {
            id = ParticleId.Clone,
            state = ParticleState.Solid,
            lifetime = -1,
            color = new Color(0.86f, 0.82f, 0.6f),
            weight = 0,
            dispersalChance = 0,
            discolouration = 0.005f
        };
        
        public static readonly Particle Output = new()
        {
            id = ParticleId.Output,
            state = ParticleState.Solid,
            lifetime = -1,
            color = new Color(0.48f, 0.48f, 0.42f),
            weight = 0,
            dispersalChance = 0,
            discolouration = 0.005f
        };
        
        public static readonly Particle Void = new()
        {
            id = ParticleId.Void,
            state = ParticleState.Solid,
            lifetime = -1,
            color = new Color(0.11f, 0.11f, 0.11f),
            weight = 0,
            dispersalChance = 0,
            discolouration = 0.005f
        };
        
        public static readonly Particle Gunpowder = new()
        {
            id = ParticleId.Gunpowder,
            state = ParticleState.Solid,
            lifetime = -1,
            color = new Color(0.18f, 0.18f, 0.18f),
            weight = 2,
            dispersalChance = 0.7f,
            discolouration = 0.02f,
            flammable = true,
        };
        
        public static readonly Particle Grenade = new()
        {
            id = ParticleId.Grenade,
            state = ParticleState.Solid,
            lifetime = -1,
            color = new Color(0.1f, 0.3f, 0.24f),
            weight = 1.5f,
            dispersalChance = 0.999f,
            discolouration = 0.01f,
        };
        
        public static readonly Particle Bomb = new()
        {
            id = ParticleId.Bomb,
            state = ParticleState.Solid,
            lifetime = -1,
            color = new Color(0.35f, 0.4f, 0.25f),
            weight = 1.5f,
            dispersalChance = 0.999f,
            discolouration = 0.01f,
        };
        
        public static readonly Particle Seed = new()
        {
            id = ParticleId.Seed,
            state = ParticleState.Solid,
            lifetime = -1,
            color = new Color(0.94f, 0.88f, 0.71f),
            weight = 3,
            dispersalChance = 0.98f,
            timer = 5,
            discolouration = 0.01f
        };
        
        public static readonly Particle Plant = new()
        {
            id = ParticleId.Plant,
            state = ParticleState.Solid,
            lifetime = -1,
            color = new Color(0.12f, 0.88f, 0.1f),
            weight = 0,
            dispersalChance = 0.9f,
            timer = 5,
            discolouration = 0.02f,
            flammable = true
        };
    }
}