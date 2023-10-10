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

    private DungeonGraph graph;
    private TileMap tilemap;
    private bool draw;
    private float drawScale = 16.0f;

    private int max = 0;
    private Random rng = new();

    private Array<Label> labels = new();

    public override void _Ready()
    {
        tilemap = GetNode<TileMap>("TileMap");
        Regenerate();
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Regenerate")) Regenerate();

        var draw = Input.IsActionPressed("OverlayGraph");
        if (draw != this.draw)
        {
            this.draw = draw;
            QueueRedraw();
        }
    }

    public override void _Draw()
    {
        if (!draw) return;
        Material = new CanvasItemMaterial();

        foreach (var e in graph.rooms)
        {
            DrawRect(new Rect2(new Vector2(e.rect.xMin, e.rect.yMin) * drawScale, e.rect.Size.Gd() * drawScale), Colors.Yellow);
        }

        foreach (var e in graph.connections)
        {
            if (e.edges.Count < 2)
            {
                var a = graph.rooms[e.room0].rect.Center.Gd();
                var b = graph.rooms[e.room1].rect.Center.Gd();
                DrawLine(a * drawScale, b * drawScale, Colors.Cyan, 5.0f, true);
            }
            else
            {
                for (var i = 0; i < e.edges.Count - 1; i++)
                {
                    var a = e.edges[i].Gd();
                    var b = e.edges[i + 1].Gd();
                    DrawLine(a * drawScale, b * drawScale, Colors.Cyan, 5.0f, true);
                }
            }
        }
    }

    public void Regenerate()
    {
        graph = new DungeonGraph().Arm
        (
            new Room("Entrance").WidthHeight(6, 6),
            new Room("CombatRoom.0").WidthHeight(14, 5, 8, 12),
            new Room("CombatRoom.1").WidthHeight(35, -2, 14, 14),
            new Room("ChestRoom").WidthHeight(55, -8, 6, 6),
            new Room("BossRoom").WidthHeight(45, 25, 16, 24),
            new Room("Exit").WidthHeight(30, 25, 6, 6)
        );

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

        QueueRedraw();
    }

    private float Random(float max = 1.0f) => Random(0.0f, max);
    private float Random(float min, float max) => (float)rng.NextDouble() * (max - min) + min;
    private float Variate(float center = 1.0f, float variation = 0.0f) => center + ((float)rng.NextDouble() - 0.5f) * variation;
}