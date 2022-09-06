using System.Collections.Generic;
using System.Linq;

namespace Murgn
{
    public static class ParticleLookup
    {
        public static readonly Dictionary<ParticleId, Particle> IdToParticle
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
    }
}