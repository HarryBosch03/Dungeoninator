using Dungeoninator.DataStructures;

namespace Dungeoninator.TilemapRasterization;

public class DungeonTilemapData
{
    public DungeonGraph graph;
    public List<Tile> tiles = new();

    public DungeonTilemapData(DungeonGraph graph)
    {
        this.graph = graph;
    }
    
    public int? GetTile(int x, int y) => GetTile(At(x, y));
    public int? GetTile(int x, int y, int z) => GetTile(At(x, y, z));
    public int? GetTile(Predicate<Tile> predicate)
    {
        foreach (var tile in tiles)
        {
            if (predicate(tile)) return tile.index;
        }
        return null;
    }

    public DungeonTilemapData SetTile(int index, int x, int y) => SetTile(index, x, y, 0, At(x, y));
    public DungeonTilemapData SetTile(int index, int x, int y, int z) => SetTile(index, x, y, z, At(x, y, z));
    public DungeonTilemapData SetTile(int index, int x, int y, int z, Predicate<Tile> predicate)
    {
        var existing = tiles.FindIndex(predicate);
        if (existing == -1)
        {
            tiles.Add(new Tile(index, x, y));
        }
        else
        {
            tiles[existing] = new Tile(index, x, y);
        }
        return this;
    }

    private static Predicate<Tile> At(int x, int y) => tile => tile.x == x && tile.y == y;
    private static Predicate<Tile> At(int x, int y, int z) => tile => tile.x == x && tile.y == y && tile.z == z;

    public class Tile
    {
        public int index;
        public int x, y, z;

        public Tile(int index, int x, int y, int z = 0)
        {
            this.index = index;
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}