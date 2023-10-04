using System;
using System.Numerics;

namespace DungeoninatorDev;
public static class Extensions
{
    public static Godot.Vector2 Gd(this System.Numerics.Vector2 v) => new Godot.Vector2(v.X, v.Y);
    public static System.Numerics.Vector2 Sn(this Godot.Vector2 v) => new System.Numerics.Vector2(v.X, v.Y);
}
