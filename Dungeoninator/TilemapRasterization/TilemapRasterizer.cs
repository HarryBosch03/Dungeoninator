using Dungeoninator.DataStructures;

namespace Dungeoninator.TilemapRasterization;

public class TilemapRasterizer
{
    public int wallTile;
    
    public DungeonTilemapData Rasterize(DungeonGraph graph)
    {
        var data = new DungeonTilemapData(graph);

        foreach (var room in graph.rooms)
        {
            var xMin = (int)MathF.Ceiling(room.rect.xMin);
            var yMin = (int)MathF.Ceiling(room.rect.yMin);
            var xMax = (int)MathF.Floor(room.rect.xMax);
            var yMax = (int)MathF.Floor(room.rect.yMax);

            if (xMin > xMax) continue;
            if (yMin > yMax) continue;

            for (var x = xMin; x <= xMax; x++)
            {
                data.SetTile(wallTile, x, yMin);
                data.SetTile(wallTile, x, yMax);
            }

            for (var y = yMin + 1; y < yMax; y++)
            {
                data.SetTile(wallTile, xMin, y);
                data.SetTile(wallTile, xMax, y);
            }
        }
        
        return data;
    }
}