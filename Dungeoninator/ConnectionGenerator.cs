using System.Numerics;

namespace Dungeoninator;

public class ConnectionGenerator
{
    public void GenerateConnections(List<int> list, Func<int, Vector2> points, int count, Action<string> log, int max = 100000)
    {
        for (var i = 2; i < count; i++)
        {
            list.Add(i - 2);
            list.Add(i - 1);

            list.Add(i - 1);
            list.Add(i);

            list.Add(i);
            list.Add(i - 2);
        }

        for (var i = 0; i < max; i++)
        {
            if (Test(ref list, points, count))
            {
                log(i.ToString());
                break;
            }
        }
        log("Failed");
    }

    private bool Test(ref List<int> list, Func<int, Vector2> points, int count)
    {
        var dirty = false;
        for (var i = 0; i < list.Count / 3; i++)
        {
            var t0 = points(list[3 * i + 0]);
            var t1 = points(list[3 * i + 1]);
            var t2 = points(list[3 * i + 2]);

            for (var j = 0; j < count; j++)
            {
                if (list[3 * i + 0] == j) continue;
                if (list[3 * i + 1] == j) continue;
                if (list[3 * i + 2] == j) continue;

                var other = points(j);
                if (IsDelaunay(t0, t1, t2, other, out _)) continue;
                dirty = true;                
                
                list[3 * i] = j;
            }
        }

        return !dirty;
    }

    public static bool IsDelaunay(Vector2 t0, Vector2 t1, Vector2 t2, Vector2 other, out Vector2 center)
    {
        var a = t0.X;
        var b = t0.Y;

        var c = t1.X;
        var d = t1.Y;

        var g = t2.X;
        var h = t2.Y;
        
        var cx = ((h - d) * (a * a + b * b) + (b - h) * (c * c + d * d) + (d - b) * (g * g + h * h)) / (2 * (a * (h - d) + c * (b - h) + g * (d - b)));
        var cy = ((g - c) * (b * b + a * a) + (a - g) * (d * d + c * c) + (c - a) * (h * h + g * g)) / (2 * (b * (g - c) + d * (a - g) + h * (c - a)));

        center = new Vector2(cx, cy);
        var r = (t0 - center).Length();

        return (other - center).Length() > r;
    }
}