
using System.Numerics;
using Godot;

public partial interface IHitboxComponent<T> where T: INumber<T>
{

	[Signal]
	public delegate void DamageDealedEventHandler(T cantity);

}