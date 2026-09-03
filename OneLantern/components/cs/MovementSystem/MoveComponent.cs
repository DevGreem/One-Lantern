using Godot;
using System;

public abstract partial class MoveComponent<[MustBeVariant] VectorType> : Node 
{

	// [Signal]
	// public delegate void MaxSpeedChangedEventHandler(float newValue);

	[Signal]
	public delegate void SpeedChangedEventHandler(float newValue);

	[Signal]
	public delegate void AccelerationChangedEventHandler(float newValue);

	// private float _maxSpeed = float.MaxValue;

	// [Export]
	// public float MaxSpeed
	// {
	// 	get => _maxSpeed;
	// 	set
	// 	{
	// 		if (MaxSpeed == value)
	// 			return;
			
	// 		_maxSpeed = value;
	// 		EmitSignalMaxSpeedChanged(MaxSpeed);
	// 	}
	// }

	private float _speed = 0.0f;

	[Export]
	public float Speed
	{
		get => _speed;
		set
		{
			if (Speed == value)
				return;
			
			_speed = value;
			EmitSignalSpeedChanged(Speed);
		}
	}

	private float _acceleration = 0.0f;

	[Export]
	public float Acceleration
	{
		get => _acceleration;
		set
		{
			if (Acceleration == value)
				return;
			
			_acceleration = value;
			EmitSignalAccelerationChanged(Acceleration);
		}
	}

	[Export]
	public bool canMove = true;

	[Export]
	public bool canChangeDirection = true;

	public virtual VectorType Direction { get; set; }

	public override void _PhysicsProcess(double delta)
	{
		AddAceleration(delta);
		CapSpeed();
		MoveAndSlideTarget();
	}

	public abstract VectorType GetTargetVelocity();

	protected abstract void AddAceleration(double delta);

	protected abstract void CapSpeed();

	protected abstract void MoveAndSlideTarget();
}
