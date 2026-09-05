
using System.Numerics;
using Godot;

public partial interface IHitboxComponent<T> where T: INumber<T>
{

	[Signal]
	public delegate void DamageDealedEventHandler(T cantity);

	public Node Actor { get; }

	public bool CanDamageActor { get; }

	public T Damage { get; set; }

	public bool IgnoreInmunity { get; set; }

	public bool ActivateInmunity { get; set; }

}