using Godot;

public partial class Record<T>: Resource, IUnique where T: Resource
{
	[Export]
	public string Id { get; private set; }

	public virtual T Value { get; protected set; }

	[Export]
	public MRQueryType queryType = MRQueryType.ADD;
	
}