using System.Numerics;
using System.Runtime.InteropServices;

namespace Dungeoninator
{
    public struct Rect
    {
        public float xMin;
        public float xMax;
        public float yMin;
        public float yMax;

        public Rect(float xMin, float xMax, float yMin, float yMax)
        {
            this.xMin = xMin;
            this.xMax = xMax;
            this.yMin = yMin;
            this.yMax = yMax;
        }

        public Vector2 Center
        {
            get => new Vector2(xMin + xMax, yMin + yMax) * 0.5f;
            set
            {
                var size = Size;

                xMin = value.X - size.X / 2.0f;
                xMax = value.X + size.X / 2.0f;
                yMin = value.Y - size.Y / 2.0f;
                yMax = value.Y + size.Y / 2.0f;
            }
        }

        public Vector2 Size
        {
            get => new Vector2(xMax - xMin, yMax - yMin);
            set
            {
                var center = Center;

                xMin = center.X - value.X / 2.0f;
                xMax = center.X + value.X / 2.0f;
                yMin = center.Y - value.Y / 2.0f;
                yMax = center.Y + value.Y / 2.0f;
            }
        }

        public bool Overlaps(Rect other, float padding = 0.0f)
        {
            if (xMax - other.xMin + padding > 0.0f && other.xMax - xMin + padding > 0.0f)
            {
                if (yMax - other.yMin + padding > 0.0f && other.yMax - yMin + padding > 0.0f)
                {
                    return true;
                }
            }
            return false;
        }

        public Vector2 GetDepenetrateVector(Rect other, float padding = 0.0f)
        {
            if (!Overlaps(other, padding)) return Vector2.Zero;

            var overlaps = new float[]
            {
                xMax - other.xMin + padding,
                other.xMax - xMin + padding,

                yMax - other.yMin + padding,
                other.yMax - yMin + padding,
            };

            var results = new Vector2[]
            {
                new(-1.0f, 0.0f),
                new(1.0f, 0.0f),
                new(0.0f, -1.0f),
                new(0.0f, 1.0f),
            };

            var best = 0;
            for (var i = 1; i < overlaps.Length; i++)
            {
                if (overlaps[i] <= 0.0f) continue;
                if (overlaps[best] <= 0.0f)
                {
                    best = i;
                    continue;
                }

                if (overlaps[best] < overlaps[i]) continue;
                best = i;
            }

            if (overlaps[best] <= 0.0f) return Vector2.Zero;
            return results[best] * overlaps[best];
        }
    }
}