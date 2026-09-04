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

	[Export]
	public override Vector2 Direction { get; set; } = Vector2.Zero;

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
				Target.Velocity = Target.Velocity.MoveToward(Vector2.Zero, Deceleration*floatDelta);
			}
			else
			{
				Target.Velocity = Target.Velocity.MoveToward(Direction * Speed, Acceleration*floatDelta);
			}
		}

		CapSpeed();
		Target.MoveAndSlide();
	}



	protected override void CapSpeed()
	{
		Target.Velocity = Target.Velocity.LimitLength(Speed);
	}

	public override Vector2 GetTargetVelocity() => Target.Velocity;
}
