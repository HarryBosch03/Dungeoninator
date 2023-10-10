using System.Numerics;
using Dungeoninator.DataStructures;

namespace Dungeoninator
{
    public class SpacialArranger
    {
        public float padding = 1.0f;
        public float retentionFactor = 0.0f;
        public SeparationMode separationMode = SeparationMode.Default;

        private DungeonGraph graph;

        public void Separate(DungeonGraph graph)
        {
            this.graph = graph;

            do
            {
                DepenetrateRooms(graph);
            }
            while (!IsValid());
        }

        private void DepenetrateRooms(DungeonGraph graph)
        {
            for (var i = 0; i < graph.rooms.Count; i++)
            {
                for (var j = i + 1; j < graph.rooms.Count; j++)
                {
                    var a = graph.rooms[i].rect;
                    var b = graph.rooms[j].rect;

                    if (!a.Overlaps(b, padding * 0.999f)) continue;

                    Separate(separationMode, ref a, ref b);

                    graph.rooms[i].rect = a;
                    graph.rooms[j].rect = b;
                }
            }
        }

        // public bool AreRoomsNeighbouring(int a, int b)
        // {
        //     var r0 = graph.rooms[a];
        //     var r1 = graph.rooms[b];
        // }
        
        public bool IsValid()
        {
            for (var i = 0; i < graph.rooms.Count; i++)
            for (var j = i + 1; j < graph.rooms.Count; j++)
            {
                var a = graph.rooms[i].rect;
                var b = graph.rooms[j].rect;

                if (!a.Overlaps(b, padding * 0.999f)) continue;
                return false;
            }

            return true;
        }

        public void Separate(SeparationMode mode, ref Rect a, ref Rect b)
        {
            switch (mode)
            {
                case SeparationMode.Default:
                {
                    var offset = a.GetDepenetrateVector(b, padding);
                    a.Center += offset * 0.5f;
                    b.Center -= offset * 0.5f;
                    break;
                }
                case SeparationMode.CenterDifference:
                {
                    var vector = a.Center - b.Center;
                    if (vector.Length() < float.Epsilon)
                    {
                        Separate(SeparationMode.Default, ref a, ref b);
                        return;
                    }

                    var offset = Vector2.Normalize(vector);

                    var xDiff0 = b.xMax - a.xMin;
                    var xDiff1 = a.xMax - b.xMin;

                    var yDiff0 = b.yMax - a.yMin;
                    var yDiff1 = a.yMax - b.yMin;

                    var xDiff = MathF.Min(xDiff0, xDiff1);
                    var yDiff = MathF.Min(yDiff0, yDiff1);

                    var scalar = MathF.Max(MathF.Abs(offset.X * xDiff), MathF.Abs(offset.Y * yDiff)) + padding;
                    offset *= scalar;

                    a.Center += offset * 0.5f;
                    b.Center -= offset * 0.5f;
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        public enum SeparationMode
        {
            Default,
            CenterDifference,
        }
    }
}