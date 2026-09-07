using Godot;
using System;

[GlobalClass, Icon("res://addons/at-icons/node/arrow_cross.svg")]
public partial class MoveComponent2D : MoveComponent<Vector2>, ITarget<CharacterBody2D>
{
	[Export]
	public CharacterBody2D Target
	{
		get;
		protected set;
	}

	private Vector2 _direction = Vector2.Zero;

	[Export]
	public override Vector2 Direction
	{
		get => _direction;
		set
		{
			_direction = value.LimitLength(1f);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!Active)
			return;
		
		if (!canMove)
		{
			Target.Velocity = Vector2.Zero;
			Target.MoveAndSlide();
			return;
		}

		float floatDelta = (float)delta;

		if (canChangeDirection)
		{
			if (Direction == Vector2.Zero)
			{
				Target.Velocity = Target.Velocity.MoveToward(Vector2.Zero, SpeedData.Deceleration*floatDelta);
			}
			else
			{
				Target.Velocity = Target.Velocity.MoveToward(Direction * SpeedData.Speed, SpeedData.Acceleration*floatDelta);
			}
		}

		GD.Print($"{nameof(MoveComponent2D)}: Target.Velocity before CapSpeed = {Target.Velocity}");
		CapSpeed();

		GD.Print($"{nameof(MoveComponent2D)}: Target.Velocity after CapSpeed = {Target.Velocity}");
		Target.MoveAndSlide();
	}



	protected override void CapSpeed()
	{
		Target.Velocity = Target.Velocity.LimitLength(SpeedData.Speed);
	}

	public override Vector2 GetTargetVelocity() => Target.Velocity;
}
