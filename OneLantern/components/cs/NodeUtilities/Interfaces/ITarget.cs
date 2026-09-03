using Godot;

public partial interface ITarget<T>
{
	[Export]
	public T Target { get; }
}