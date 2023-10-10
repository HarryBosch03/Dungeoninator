using System.Numerics;

namespace Dungeoninator.DataStructures;

public class Connection
{
    public int room0;
    public int room1;
    public List<Vector2> edges = new();

    public Connection(int room0, int room1, params Vector2[] edges)
    {
        this.room0 = room0;
        this.room1 = room1;
        this.edges.AddRange(edges);
    }

    public void StyleConnection(DungeonGraph graph)
    {
        var room0 = graph.rooms[this.room0];
        var room1 = graph.rooms[this.room1];

        var direction = room1.rect.Center - room0.rect.Center;
        if (MathF.Abs(direction.X) > MathF.Abs(direction.Y))
        {
            direction = new Vector2(MathF.Sign(direction.X), 0.0f);
        }
        else
        {
            direction = new Vector2(0.0f, MathF.Sign(direction.Y));
        }
    }
}