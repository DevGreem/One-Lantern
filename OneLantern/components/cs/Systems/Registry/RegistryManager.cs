using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Godot;
#nullable enable

[GlobalClass]
public partial class RegistryManager : Node
{
	public static RegistryManager Instance { get; private set; } = default!;

	private readonly Dictionary<string, IRegistry> _registries = new();

	public RegistryManager()
	{
		if (IsInstanceValid(Instance))
		{
			QueueFree();
			return;
		}

		Instance = this;
	}

	/// <summary>
	/// Function for gdscript
	/// </summary>
	/// <param name="id">ID of the Registry</param>
	/// <returns>Registry</returns>
	public Registry GetRegistry(string id) => (Registry)_registries[id];

	public T GetRegistry<T>(string id) where T: IRegistry => (T)_registries[id];

	public T GetRegistry<T>(RecordId id) where T: IRegistry => GetRegistry<T>(id.ToString());

	public bool AddRegistry(IRegistry registry)
	{
		if (_registries.ContainsKey(registry.Id.ToString()))
			return false;
		
		_registries.Add(registry.Id.ToString(), registry);
		return true;
	}

	public bool RemoveRegistry(string id) => _registries.Remove(id);

	public bool RemoveRegistry(RecordId id) => RemoveRegistry(id.ToString());

	public bool RemoveRegistry(IRegistry registry) => RemoveRegistry(registry.Id);
}