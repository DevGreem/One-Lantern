
using System.Threading.Tasks;
using Godot;

public partial interface IMRegistry: IUnique
{	
	public bool Unregister(string id);

	public bool Unregister(RecordId id) => Unregister(id.ToString());

	public bool Contains(string id);

	public bool Contains(RecordId id) => Contains(id.ToString());

	public Task Load();

}