using Godot;
#nullable enable

[GlobalClass]
public partial class FollowMouseNode2D : Node
{
	public Node2D? target;

	public float rotationOffsetDegrees = 0.0f;

	public override void _Process(double delta)
	{
		if (target is null)
			return;
		
		target.Rotation = target.GetGlobalMousePosition().AngleTo(target.GlobalPosition);
		target.RotationDegrees += rotationOffsetDegrees;
	}
}