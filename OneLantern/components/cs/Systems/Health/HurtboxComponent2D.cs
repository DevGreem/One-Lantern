using Godot;
using System;

[GlobalClass, Icon("res://addons/at-icons/node2d/area.svg")]
public partial class HurtboxComponent2D: Area2D, IHurtboxComponent<float>
{

	[Signal]
	public delegate void DamageReceivedEventHandler(float cantity);
	
	[Export]
	public HealthComponent HealthNode { get; set; }

	[Export]
	public float InmunityTime { get; set; } = -1.0f;

	[Export]
	public bool Invincible { get; set; } = false;

	[Export]
	public bool EmitDamageOnInvincible { get; set; } = false;

	protected float inmunityRemaining = -1.0f;

	public bool ReceiveDamage(float cantity, bool activateInmunity, bool ignoreInmunity)
	{
		if (!ignoreInmunity && inmunityRemaining > 0f)
			return false;
		
		if (Invincible)
		{
			
			if (EmitDamageOnInvincible)
				EmitSignalDamageReceived(cantity);
			
			return false;
		}

		HealthNode.Health -= cantity;
		EmitSignalDamageReceived(cantity);

		if (activateInmunity)
			inmunityRemaining = InmunityTime;
		
		return true;
	}
}
