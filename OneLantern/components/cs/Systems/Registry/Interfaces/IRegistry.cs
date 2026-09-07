
using Godot;

public partial interface IRegistry
{

	public abstract RecordId Id { get; }
	
	public bool Unregister(string id);

	public bool Unregister(RecordId id) => Unregister(id.ToString());

	public bool Contains(string id);

	public bool Contains(RecordId id) => Contains(id.ToString());

}