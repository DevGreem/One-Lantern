using Godot;
using System;
using System.Numerics;

public partial interface IHurtboxComponent<T> where T: INumber<T>
{
	
	[Signal]
	public delegate void DamageReceivedEventHandler(T cantity);

	public HealthComponent HealthNode { get; }

	public float InmunityTime { get; }

	public bool Invincible { get; set; }

	public bool EmitDamageOnInvincible { get; }

	public bool ReceiveDamage(T cantity) => ReceiveDamage(cantity, false, false);

	public bool ReceiveDamage(T cantity, bool activateInmunity) => ReceiveDamage(cantity, activateInmunity, false);

	public abstract bool ReceiveDamage(T cantity, bool activateInmunity, bool ignoreInmunity);
}
