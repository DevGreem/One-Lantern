using System.Collections.Generic;
using System.Data.Common;
using Godot;
#nullable enable

[GlobalClass]
public partial class RegistryManager : Node
{
	private readonly Dictionary<string, IRegistry> _registries = new();

	public Registry GetRegistry(string id) => (Registry)_registries[id];

	public T GetRegistry<T>(string id) where T: IRegistry => (T)_registries[id];

	public T GetRegistry<T>(RecordId id) where T: IRegistry => GetRegistry<T>(id.ToString());

	public void AddRegistry<T>(Registry<T> registry)
	{
		_registries.Add(registry.Id.ToString(), registry);
	}

	public bool RemoveRegistry(string id) => _registries.Remove(id);

	public bool RemoveRegistry(RecordId id) => RemoveRegistry(id.ToString());

	public bool RemoveRegistry(IRegistry registry) => RemoveRegistry(registry.Id);
}