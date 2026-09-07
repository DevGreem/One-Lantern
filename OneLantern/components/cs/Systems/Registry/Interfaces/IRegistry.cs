
using Godot;

public partial interface IRegistry: IUnique
{	
	public bool Unregister(string id);

	public bool Unregister(RecordId id) => Unregister(id.ToString());

	public bool Contains(string id);

	public bool Contains(RecordId id) => Contains(id.ToString());

}