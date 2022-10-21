using Murgn.OdinSerializer;

namespace Murgn.Data
{
    [System.Serializable] 
    public class MapData
    {
        public string mapName;
        [OdinSerialize]
        public Particle[,] map;
        public MapSize mapSize;

        public MapData(string mapName, Particle[,] map, MapSize mapSize)
        {
            this.mapName = mapName;
            this.map = map;
            this.mapSize = mapSize;
        }
    }

    
}