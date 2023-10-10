using System;
using Dungeoninator.Math;
using Godot;

namespace DungeoninatorDev.Scripts;

public partial class SatTest : Node2D
{
	private Polygon2D shape0, shape1;

	private Vector2 lastMousePos;

	public override void _Ready()
	{
		shape0 = GetNode<Polygon2D>("Shape0");
		shape1 = GetNode<Polygon2D>("Shape1");
	}

	public override void _Process(double delta)
	{
		var mousePos = GetGlobalMousePosition();
		var mouseDelta = mousePos - lastMousePos;

		if (Input.IsMouseButtonPressed(MouseButton.Left))
		{
			shape0.GlobalPosition += mouseDelta;
		}
		if (Input.IsMouseButtonPressed(MouseButton.Right))
		{
			shape1.GlobalPosition += mouseDelta;
		}

		var toLines = BmMath.LineList<Polygon2D>(e => i => e.ToGlobal(e.Polygon[i]).Sn(), e => e.Polygon.Length, true);
		var overlaps = BmMath.SAT(toLines(shape0), toLines(shape1));
		
		Both(shape => shape.Color = overlaps ? Colors.Red : Colors.Green);

		lastMousePos = mousePos;
		QueueRedraw();
	}

	private void Both(Action<Polygon2D> callback)
	{
		callback(shape0);
		callback(shape1);
	}
}