using System.Numerics;

namespace Dungeoninator.DataStructures;

public class Room
{
    public string identifier;
    public Rect rect;

    public Room(string identifier)
    {
        this.identifier = identifier;
    }
    
    public Room MinMax(int xMin, int xMax, int yMin, int yMax)
    {
        rect.xMin = xMin;
        rect.xMax = xMax;
        rect.yMin = yMin;
        rect.yMax = yMax;

        return this;
    }

    public Room WidthHeight(float width, float height) => WidthHeight(0.0f, 0.0f, width, height);
    public Room WidthHeight(float xCenter, float yCenter, float width, float height)
    {
        rect.Size = new Vector2(width, height);
        rect.Center = new Vector2(xCenter, yCenter);
        
        return this;
    }
}