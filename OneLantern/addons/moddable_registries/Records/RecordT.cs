using Godot;

public partial class Record<T>: Resource, IMRecord<T> where T: Resource
{
	[Export]
	public string Id { get; private set; }

	public virtual T Value { get; protected set; }

	[Export]
	public MRQueryType QueryType { get; private set; } = MRQueryType.ADD;
	
}