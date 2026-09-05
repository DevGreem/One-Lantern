using System;
using System.Linq;
using Godot;
using Godot.Collections;
#nullable enable

[GlobalClass]
[Tool]
public partial class RotateComponent : Node, IActivable
{
	[Export]
	public bool Active { get; set; } = true;

	[Export]
	public Node2D? target;

	public float RotationAngle { get; set; } = 0.0f;

	[Export(PropertyHint.Range, "-180,180,0.1")]
	private float RotationDegressAngle
	{
		get => Mathf.RadToDeg(RotationAngle);
		set
		{
			RotationAngle = Mathf.DegToRad(value);
		}
	}

	[Export]
	public Vector2 AngleDirection
	{
		get => Vector2.FromAngle(RotationAngle);
		set
		{
			RotationAngle = value.Angle();
		}
	}

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

	/// <summary>
	/// In Degress
	/// </summary>
	[Export]
	public SpeedDataResource RotationSpeedData { get; private set; } = new();

	protected float _rotationVelocity = 0.0f;

	public override void _Process(double delta)
	{
		if (target is null || !Active || Engine.IsEditorHint())
			return;

		float targetRotation = RotationAngle + Mathf.DegToRad(rotationOffsetDegrees);

		if (InstantRotation)
		{
			target.Rotation = targetRotation;
			_rotationVelocity = 0.0f;
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

			float decelerationDistance = 0f;
			if (RotationSpeedData.Deceleration > 0f)
			{
				decelerationDistance = Mathf.Pow(_rotationVelocity, 2) / (2f * Mathf.DegToRad(RotationSpeedData.Deceleration));
			}

			if (Math.Abs(difference) <= decelerationDistance)
			{
				ApplyVelocity(0f, Mathf.DegToRad(RotationSpeedData.Deceleration * (float)delta));
			}
			else
			{
				float desiredVelocity = Mathf.Sign(difference) * Mathf.DegToRad(RotationSpeedData.Speed);

				ApplyVelocity(desiredVelocity, Mathf.DegToRad(RotationSpeedData.Acceleration) * (float)delta);
			}

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

	protected void ApplyVelocity(float desiredVelocity, float delta)
	{
		_rotationVelocity = Mathf.MoveToward(
			_rotationVelocity,
			desiredVelocity,
			delta
		);
	}

	public override void _ValidateProperty(Dictionary property)
	{
		
		string[] props = [nameof(RotationSpeedData)];

		if (props.Contains(property["name"].AsString()))
		{
			if (!InstantRotation)
				return;
			
			property["usage"] = (int)PropertyUsageFlags.NoEditor;
		}
	}
}