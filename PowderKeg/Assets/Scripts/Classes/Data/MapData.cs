namespace Murgn.Data
{
    [System.Serializable] 
    public class MapData
    {
        public string mapName;
        public Particle[,] map;

        public MapData(string mapName, Particle[,] map)
        {
            this.mapName = mapName;
            this.map = map;
        }
    }

    
}