using Godot;
using System;

public abstract partial class MoveComponent<[MustBeVariant] VectorType> : Node, IActivable
{

	// [Signal]
	// public delegate void MaxSpeedChangedEventHandler(float newValue);

	[Signal]
	public delegate void SpeedChangedEventHandler(float newValue);

	[Signal]
	public delegate void AccelerationChangedEventHandler(float newValue);

	[Signal]
	public delegate void DecelerationChangedEventHandler(float newValue);

	[Export]
	public bool Active { get; set; } = true;

	[Export]
	public SpeedDataResource SpeedData { get; private set; } = new();

	[Export]
	public bool canMove = true;

	[Export]
	public bool canChangeDirection = true;

	public virtual VectorType Direction { get; set; }

	public abstract VectorType GetTargetVelocity();

	protected abstract void CapSpeed();

}
