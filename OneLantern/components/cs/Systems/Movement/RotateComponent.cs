using Godot;
using Godot.Collections;
#nullable enable

[GlobalClass]
public partial class RotateComponent : Node, IActivable
{
	[Export]
	public bool Active { get; set; } = true;

	[Export]
	public Node2D? target;

	[Export]
	public float rotationOffsetDegrees = 0.0f;

	private bool _instantRotation = true;

	[Export]
	public bool InstantRotation
	{
		get => _instantRotation;
		set
		{
			if (_instantRotation == value)
				return;
			
			_instantRotation = value;
			NotifyPropertyListChanged();
		}
	}

	[Export]
	public double rotationSpeed = 1.0;

	public Vector2 direction = Vector2.Zero;

	public override void _Process(double delta)
	{
		if (target is null || !Active)
			return;

		if (InstantRotation)
		{
			target.LookAt(direction);
		}
		else
		{
			double targetRotation = direction.Angle();

			target.Rotation = (float)Mathf.MoveToward(
				target.Rotation,
				targetRotation,
				rotationSpeed * delta
			);
		}

		target.RotationDegrees += rotationOffsetDegrees;
		GD.Print(nameof(RotateToMouseNode2D), ": ", target.RotationDegrees);
	}

	public override void _ValidateProperty(Dictionary property)
	{
		
		if (property["name"].AsString() == nameof(rotationSpeed))
		{
			if (!InstantRotation)
				return;
			
			property["usage"] = (int)PropertyUsageFlags.NoEditor;
		}
	}
}