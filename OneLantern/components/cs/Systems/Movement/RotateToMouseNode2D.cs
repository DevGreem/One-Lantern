using Godot;
#nullable enable

[GlobalClass]
public partial class RotateToMouseNode2D : Node
{
	[Export]
	public Node2D? target;

	[Export]
	public float rotationOffsetDegrees = 0.0f;

	public override void _Process(double delta)
	{
		if (target is null)
			return;

		target.LookAt(target.GetGlobalMousePosition());
		target.RotationDegrees += rotationOffsetDegrees;
		GD.Print(nameof(RotateToMouseNode2D), ": ", target.RotationDegrees);
	}
}