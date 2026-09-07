using System.Collections.Generic;
using Godot;
#nullable enable

public abstract partial class Registry<T> : Resource, IRegistry<T>
{

	public abstract RecordId Id { get; protected set; }

	private readonly Dictionary<string, T> _entries = new();

	public IReadOnlyDictionary<string, T> Entries => _entries;

	public bool Register(string id, T value)
	{
		if (_entries.ContainsKey(id))
			return false;
		
		_entries[id] = value;
		return true;
	}

	public bool Unregister(string id) => _entries.Remove(id);

	public bool TryGet(string id, out T? value) => _entries.TryGetValue(id, out value);

	public T Get(string id) => _entries[id];

	public bool Contains(string id) => _entries.ContainsKey(id);
}