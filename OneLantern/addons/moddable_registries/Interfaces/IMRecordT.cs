using Godot;

public partial interface IMRecord<T>: IMRecord where T: Resource
{
	public T Value { get; }

}