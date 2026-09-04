using Godot;
#nullable enable

[GlobalClass]
public partial class RotateToMouseNode2D : Node, IActivable
{
	[Export]
	public bool Active { get; set; } = true;

	[Export]
	public Node2D? target;

	[Export]
	public float rotationOffsetDegrees = 0.0f;

	[Export]
	public bool instantRotation = true;

	[Export]
	public double rotationSpeed = 1.0f;

	public override void _Process(double delta)
	{
		if (target is null || !Active)
			return;

		target.LookAt(target.GetGlobalMousePosition());
		target.RotationDegrees += rotationOffsetDegrees;
		GD.Print(nameof(RotateToMouseNode2D), ": ", target.RotationDegrees);
	}
}