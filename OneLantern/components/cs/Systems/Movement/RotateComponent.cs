using System;
using System.Linq;
using Godot;
using Godot.Collections;
#nullable enable

[GlobalClass]
[Tool]
public partial class RotateComponent : Node, IActivable, ISmoothSpeed<float>
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
	public float Speed { get; set; } = 1.0f;

	[Export]
	public float Acceleration { get; set; } = 1.0f;

	[Export]
	public float Deceleration { get; set; } = 1.0f;

	protected float _rotationVelocity = 0.0f;

	public Vector2 direction = Vector2.Zero;

	public override void _Process(double delta)
	{
		if (target is null || !Active || Engine.IsEditorHint())
			return;

		float targetRotation = direction.Angle() + Mathf.DegToRad(rotationOffsetDegrees);

		if (InstantRotation)
		{
			target.Rotation = targetRotation;
			_rotationVelocity = 0.0f;;
		}
		else
		{
			float difference = Mathf.AngleDifference(
				target.Rotation,
				targetRotation
			);

			if (Mathf.Abs(difference) < 0.001f)
			{
				target.Rotation = targetRotation;
				_rotationVelocity = 0.0f;
				return;
			}

			float desiredVelocity = Mathf.Sign(difference) * Speed;

			_rotationVelocity = Mathf.MoveToward(
				_rotationVelocity,
				desiredVelocity,
				Acceleration * (float)delta
			);

			float rotationAmount = _rotationVelocity * (float)delta;

			if (Mathf.Abs(rotationAmount) >= Mathf.Abs(difference))
			{
				target.Rotation = targetRotation;
				_rotationVelocity = 0.0f;
			}
			else {
				target.Rotation += rotationAmount;
			}
		}
	}

	public override void _ValidateProperty(Dictionary property)
	{
		
		string[] props = ["Speed", "Acceleration", "Deceleration"];

		if (props.Contains(property["name"].AsString()))
		{
			if (!InstantRotation)
				return;
			
			property["usage"] = (int)PropertyUsageFlags.NoEditor;
		}
	}
}