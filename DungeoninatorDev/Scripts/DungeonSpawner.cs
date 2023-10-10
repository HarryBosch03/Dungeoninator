using System;
using Dungeoninator;
using Dungeoninator.DataStructures;
using Dungeoninator.TilemapRasterization;
using Godot;
using Godot.Collections;

namespace DungeoninatorDev.Scripts;

public partial class DungeonSpawner : Node2D
{
    [Export] public LabelSettings labelSettings;
    
    private TileMap tilemap;

    private int max = 0;
    private Random rng = new();

    private bool wasSpacePressed;
    private Array<Label> labels = new();
    
    public override void _Ready()
    {
        tilemap = GetNode<TileMap>("TileMap");
        Regenerate();
    }

    public override void _Process(double delta)
    {
        var spacePressed = Input.IsKeyPressed(Key.Space); 
        if (spacePressed && !wasSpacePressed)
        {
            Regenerate();
        }
        wasSpacePressed = spacePressed;
    }

    public void Regenerate()
    {
        var graph = new DungeonGraph();
        graph.Add(new Room("Entrance").WidthHeight(6, 6));
        graph.Add(new Room("CombatRoom.0").WidthHeight(8, 12));
        graph.Add(new Room("CombatRoom.1").WidthHeight(14, 14));
        graph.Add(new Room("ChestRoom").WidthHeight(6, 6));
        graph.Add(new Room("BossRoom").WidthHeight(16, 24));
        graph.Add(new Room("Exit").WidthHeight(6, 6));

        var spacialArranger = new SpacialArranger();
        spacialArranger.separationMode = SpacialArranger.SeparationMode.CenterDifference;
        spacialArranger.Separate(graph);
        var data = new TilemapRasterizer().Rasterize(graph);

        var cells = new Array<Vector2I>();
        foreach (var e in data.tiles)
        {
            cells.Add(new Vector2I(e.x, e.y));   
        }
        tilemap.Clear();
        tilemap.SetCellsTerrainConnect(0, cells, 0, 0);

        while (labels.Count < graph.rooms.Count)
        {
            var newLabel = new Label();

            newLabel.LabelSettings = labelSettings;
            
            labels.Add(newLabel);
            AddChild(newLabel);
        }
        while (graph.rooms.Count < labels.Count)
        {
            var oldLabel = labels[^1];
            labels.Remove(oldLabel);
            RemoveChild(oldLabel);
            oldLabel.QueueFree();
        }

        for (var i = 0; i < graph.rooms.Count; i++)
        {
            var room = graph.rooms[i];
            var label = labels[i];
            label.Text = $"{i + 1}. {room.identifier}";
            label.GlobalPosition = room.rect.Center.Gd() * 16.0f;
            label.RotationDegrees = 45.0f;
            label.PivotOffset = new Vector2(0.0f, label.Size.Y / 2.0f);
        }
    }

    private float Random(float max = 1.0f) => Random(0.0f, max);
    private float Random(float min, float max) => (float)rng.NextDouble() * (max - min) + min;
    private float Variate(float center = 1.0f, float variation = 0.0f) => center + ((float)rng.NextDouble() - 0.5f) * variation;
}