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

    public DungeonGraph Add(params Connection[] connections)
    {
        this.connections.AddRange(connections);
        return this;
    }
}