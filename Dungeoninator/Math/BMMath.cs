using System.Numerics;
using Dungeoninator.DataStructures;

namespace Dungeoninator.Math;

public static class BmMath
{
    // public static bool LineRectIntersect(Line line, Rect rect)
    // {
    //     
    // }

    public static Func<T, List<Line>> LineList<T>(Func<T, Func<int, Vector2>> list, Func<T, int> count, bool closed) => target => LineList(list(target), count(target), closed);
    public static List<Line> LineList(Func<int, Vector2> list, int count, bool closed)
    {
        var res = new List<Line>();
        for (var i = 1; i < count; i++)
        {
            res.Add(new Line(list(i - 1), list(i)));
        }
        if (closed) res.Add(new Line(list(count - 1), list(0)));
        return res;
    }
    
    public static bool SAT(List<Line> shape0, List<Line> shape1)
    {
        var points0 = getPoints(shape0);
        var points1 = getPoints(shape1);

        foreach (var axis in shape0)
        {
            var (min0, max0) = getMinMax(axis, points0);
            var (min1, max1) = getMinMax(axis, points1);

            if (max0 < min1 || max1 < min0) return false;
        }
        
        foreach (var axis in shape1)
        {
            var (min0, max0) = getMinMax(axis, points0);
            var (min1, max1) = getMinMax(axis, points1);

            if (max0 < min1 || max1 < min0) return false;
        }

        return true;
        
        Vector2[] getPoints(List<Line> shape)
        {
            var points = new Vector2[shape.Count * 2];
            for (var i = 0; i < shape.Count; i++)
            {
                points[2 * i] = shape[i].start;
                points[2 * i + 1] = shape[i].end;
            }
            return points;
        }

        (float, float) getMinMax(Line line, Vector2[] points)
        {
            var min = float.MaxValue;
            var max = float.MinValue;
            var normal = line.Normal;

            foreach (var p in points)
            {
                var dot = Vector2.Dot(normal, p - line.start);
                if (dot < min) min = dot;
                if (dot > max) max = dot;
            }
            
            return (min, max);
        }
    }

    public static float Clamp(float v, float min, float max)
    {
        if (v < min) return min;
        if (v > max) return max;
        return v;
    }

    public static float Clamp01(float v) => Clamp(v, 0.0f, 1.0f);
}