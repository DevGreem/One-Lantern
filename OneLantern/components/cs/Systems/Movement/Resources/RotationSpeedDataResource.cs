using Godot;

[GlobalClass]
public partial class RotationSpeedDataResource : Resource, ISmoothSpeed<float>
{

	public const string ROTATION_DEGRESS_HINT = "-360,360,0.1,or_greater,or_less,radians_as_degress,suffix:degress";

	[Signal]
	public delegate void SpeedChangedEventHandler();

	[Signal]
	public delegate void AccelerationChangedEventHandler();

	[Signal]
	public delegate void DecelerationChangedEventHandler();

	private float _speed = 0.0f;
	
	[Export(PropertyHint.Range, ROTATION_DEGRESS_HINT)]
	public float Speed
	{
		get => _speed;
		set
		{
			if (_speed == value)
				return;
			
			_speed = value;
			EmitSignalSpeedChanged();
		}
	}

	private float _acceleration = 0.0f;

	[Export(PropertyHint.Range, ROTATION_DEGRESS_HINT)]
	public float Acceleration
	{
		get => _acceleration;
		set
		{
			if (_acceleration == value)
				return;
			
			_acceleration = value;
			EmitSignalAccelerationChanged();
		}
	}

	private float _deceleration = 0.0f;

	[Export(PropertyHint.Range, ROTATION_DEGRESS_HINT)]
	public float Deceleration
	{
		get => _deceleration;
		set
		{
			if (_deceleration == value)
				return;
			
			_deceleration = value;
			EmitSignalDecelerationChanged();
		}
	}

	public RotationSpeedDataResource DegToRad()
	{
		RotationSpeedDataResource resource = new()
		{
			Speed = Mathf.DegToRad(this.Speed),
			Acceleration = Mathf.DegToRad(this.Acceleration),
			Deceleration = Mathf.DegToRad(this.Deceleration)	
		};

		return resource;
	}

	public RotationSpeedDataResource RadToDeg()
	{
		RotationSpeedDataResource resource = new()
		{
			Speed = Mathf.RadToDeg(this.Speed),
			Acceleration = Mathf.RadToDeg(this.Acceleration),
			Deceleration = Mathf.RadToDeg(this.Deceleration)
		};

		return resource;
	}
}