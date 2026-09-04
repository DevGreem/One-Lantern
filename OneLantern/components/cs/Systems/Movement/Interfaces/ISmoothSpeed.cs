

using System.Numerics;

public partial interface ISmoothSpeed<T> where T: INumber<T>
{
	
	public T Speed { get; }

	public T Acceleration { get; }

	public T Deceleration { get; }
}