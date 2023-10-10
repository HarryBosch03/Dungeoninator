using System.Numerics;

namespace Dungeoninator.DataStructures;

public class DungeonGraph
{
    public List<Room> rooms = new();
    public List<Connection> connections = new();

    public DungeonGraph Add(params Room[] rooms)
    {
        this.rooms.AddRange(rooms);
        return this;
    }

    public DungeonGraph Arm(params Room[] rooms)
    {
        var offset = this.rooms.Count;
        Add(rooms);
        for (var i = 0; i < rooms.Length - 1; i++)
        {
            connections.Add(new Connection(offset + i, offset + i + 1));
        }
        return this;
    }

    public DungeonGraph Add(params Connection[] connections)
    {
        this.connections.AddRange(connections);
        return this;
    }
}