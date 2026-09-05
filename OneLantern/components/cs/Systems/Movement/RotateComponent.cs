using System;
using System.Linq;
using Godot;
using Godot.Collections;
#nullable enable

[GlobalClass, Icon("res://addons/at-icons/node/arrows_clockwise.svg")]
[Tool]
public partial class RotateComponent : Node, IActivable
{
	[Export]
	public bool Active { get; set; } = true;

	[Export]
	public Node2D? target;

	public float RotationAngle { get; set; } = 0.0f;

	[Export(PropertyHint.Range, "-180,180,0.1,suffix:degress")]
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
	public RotationSpeedDataResource RotationSpeedData { get; private set; } = new();

	[Export]
	private Resource InspectorRotationSpeedData { get; set; } = new RotationSpeedDataResource();

	protected float _rotationVelocity = 0.0f;

	public override void _Ready()
	{
		if (Engine.IsEditorHint())
			return;

		RotationSpeedData = ((RotationSpeedDataResource)InspectorRotationSpeedData).DegToRad();
	}

	public override void _Process(double delta)
	{
		if (target is null || !Active || Engine.IsEditorHint())
			return;

		float targetRotation = RotationAngle + Mathf.DegToRad(rotationOffsetDegrees);

		if (InstantRotation)
		{
			target.Rotation = targetRotation;
			_rotationVelocity = 0f;
			return;
		}

		// Future logic implementation for smooth rotation
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
		
		string[] props = [nameof(InspectorRotationSpeedData)];

		if (props.Contains(property["name"].AsString()))
		{
			if (InstantRotation)
			{
				property["usage"] = (int)PropertyUsageFlags.NoEditor;
			}
			else
			{
				property["hint"] = (int)PropertyHint.ResourceType;
				property["hint_string"] = nameof(RotationSpeedDataResource);
			}
			
			//GD.Print(property["hint"], "\n", property["hint_string"]);
		}

		//GD.Print(property);
	}
}