using System.Collections.Generic;
using Godot;

[GlobalClass, Icon("res://addons/at-icons/node2d/out_of_bounds.svg")]
public partial class HitboxComponent2D: Area2D, IHitboxComponent<float>
{
	
	[Signal]
	public delegate void DamageDealedEventHandler(float damage);

	[Export]
	public Node Actor { get; set; }

	[Export]
	public bool CanDamageActor { get; set; } = false;

	[Export]
	public float Damage { get; set; } = 0f;

	[Export]
	public bool ActivateInmunity { get; set; } = true;

	[Export]
	public bool IgnoreInmunity { get; set; } = false;

	protected LinkedList<IHurtboxComponent<float>> EnteredHurtboxes { get; private set; }= [];

	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
		AreaExited += OnAreaExited;
	}

	public override void _Process(double delta)
	{
		foreach (var hurtbox in EnteredHurtboxes)
		{
			DealDamage(hurtbox);
		}
	}

	protected virtual void OnAreaEntered(Area2D area)
	{
		
		if (area is not IHurtboxComponent<float>)
			return;
		
		IHurtboxComponent<float> hurtbox = (IHurtboxComponent<float>)area;

		if (hurtbox.Actor == this.Actor && !CanDamageActor)
			return;
		
		EnteredHurtboxes.AddLast(hurtbox);
	}

	protected virtual void OnAreaExited(Area2D area)
	{
		if (area is not IHurtboxComponent<float>)
			return;
		
		var hurtbox = (IHurtboxComponent<float>)area;

		EnteredHurtboxes.Remove(hurtbox);
	}

	protected void DealDamage(IHurtboxComponent<float> hurtbox)
	{
		bool hitted = hurtbox.ReceiveDamage(Damage, ActivateInmunity, IgnoreInmunity);

		if (hitted)
		{
			EmitSignalDamageDealed(Damage);
		}
	}
}