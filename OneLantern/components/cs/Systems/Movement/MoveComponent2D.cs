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

	protected override void AddAceleration(double delta)
	{
		Target.Velocity += Direction*Acceleration*(float)delta;
	}

	protected override void CapSpeed()
	{
		Target.Velocity.Min(Speed);
	}

	protected override void MoveAndSlideTarget()
	{
		Target.MoveAndSlide();
	}

	public override Vector2 GetTargetVelocity() => Target.Velocity;
}
