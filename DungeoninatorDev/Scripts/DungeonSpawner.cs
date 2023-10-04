using Godot;
using Dungeoninator;
using System.Collections.Generic;

namespace DungeoninatorDev.Scripts;

public partial class DungeonSpawner : Node2D
{
    private List<Rect> rectangles = new();
    private List<int> connections = new();

    private int max = 0;

    public override void _Ready()
    {
        
    }

    public override void _Process(double delta)
    {
        QueueRedraw();

        var previous = max;

        if (Input.IsKeyPressed(Key.Left)) max--;
        if (Input.IsKeyPressed(Key.Right)) max++;
        if (max < 0) max = 0;

        if (max != previous || Input.IsKeyPressed(Key.Space))
        {
            connections.Clear();
            var connectionGenerator = new ConnectionGenerator();
            connectionGenerator.GenerateConnections(connections, i => rectangles[i].Center, rectangles.Count, GD.Print, max);
        }

        if (Input.IsKeyPressed(Key.Space))
        {
            RandomRects(4);
            var spacialArrangement = new SpacialArranger();
            spacialArrangement.padding = 10.0f;

            spacialArrangement.Separate(rectangles);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        //var r = rectangles[0];
        //r.Center = GetGlobalMousePosition().Sn();
        //rectangles[0]= r;
    }

    public override void _Draw()
    {
        //    var a = new Vector2(5, 3);
        //    var b = new Vector2(7, -2);
        //    var c = new Vector2(-3, 5);
        //    var d = GetGlobalMousePosition();
        //
        //    if (!ConnectionGenerator.IsDelaunay(a.Sn(), b.Sn(), c.Sn(), d.Sn(), out var center))
        //    {
        //     a = d;
        //    }
        //    
        //    DrawArc(center.Gd(), (center.Gd() - a).Length(), 0.0f, 360.0f, 128, new Color(1.0f, 1.0f, 1.0f, 0.1f));
        //    
        //    DrawLine(a, b, Colors.White);
        //    DrawLine(b, c, Colors.White);
        //    DrawLine(c, a, Colors.White);
        // DrawCircle(d, 0.1f, Colors.Red);

        foreach (var r in rectangles)
        {
            DrawRect(new Rect2((r.Center - r.Size * 0.5f).Gd(), r.Size.Gd()), Colors.White, false);
        }

        for (var i = 0; i < connections.Count / 2; i++)
        {
            var a = rectangles[connections[2 * i]].Center.Gd();
            var b = rectangles[connections[2 * i + 1]].Center.Gd();

            DrawLine(a, b, Colors.Red);
        }
    }

    public void RandomRects(int count)
    {
        var rng = new System.Random();

        rectangles.Clear();
        for (var i = 0; i < count; i++)
        {
            var r = new Rect();
            r.Size = new System.Numerics.Vector2(rng.Next(2, 5) * 20.0f, rng.Next(2, 5) * 20.0f);
            rectangles.Add(r);
        }
    }
}