using Godot;

public partial interface ITarget<T>
{
	[Signal]
	public delegate void TargetChangedEventHandler();

	[Export]
	public T Target { get; }
}