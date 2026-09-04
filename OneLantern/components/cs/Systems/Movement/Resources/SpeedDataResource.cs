using Godot;

[GlobalClass]
public partial class SpeedDataResource : Resource, ISmoothSpeed<float>
{

	[Signal]
	public delegate void SpeedChangedEventHandler();

	[Signal]
	public delegate void AccelerationChangedEventHandler();

	[Signal]
	public delegate void DecelerationChangedEventHandler();

	private float _speed = 0.0f;
	
	[Export]
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

	[Export]
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

	[Export]
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

	public Vector2 LimitVelocity<V>(Vector2 velocity) 
	{
		return velocity.LimitLength(Speed);
	}
}