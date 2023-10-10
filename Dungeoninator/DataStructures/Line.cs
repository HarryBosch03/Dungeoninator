using System.Numerics;
using Dungeoninator.Math;

namespace Dungeoninator.DataStructures;

public struct Line
{
    public Vector2 start;
    public Vector2 end;

    public Line(Vector2 start, Vector2 end)
    {
        this.start = start;
        this.end = end;
    }
    
    public Vector2 Lerp(float t) => start + (end - start) * BmMath.Clamp01(t);
    
    public Vector2 Tangent => Vector2.Normalize(end - start);
    public Vector2 Normal
    {
        get
        {
            var t = Tangent;
            return new Vector2(t.Y, -t.X);
        }
    }
}