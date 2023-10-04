namespace Dungeoninator
{
    public class SpacialArranger
    {
        public float padding = 1.0f;

        public void Separate(List<Rect> rooms)
        {
            bool overlap;

            do
            {
                overlap = false;
                for (var i = 0; i < rooms.Count; i++)
                {
                    for (var j = i + 1; j < rooms.Count; j++)
                    {
                        var a = rooms[i];
                        var b = rooms[j];

                        if (!a.Overlaps(b, padding * 0.999f)) continue;
                        overlap = true;

                        var offset = a.GetDepenetrateVector(b, padding);
                        a.Center += offset * 0.5f;
                        b.Center -= offset * 0.5f;

                        rooms[i] = a;
                        rooms[j] = b;
                    }
                }
            }
            while (overlap);
        }
    }
}