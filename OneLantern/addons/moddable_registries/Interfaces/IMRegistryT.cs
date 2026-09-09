
#nullable enable

using Godot;
using Godot.Collections;

public partial interface IMRegistry<[MustBeVariant] T> : IMRegistry
{
	public Dictionary<string, T> Entries { get; }

	public bool Register(string id, T value);

	public bool Register(RecordId id, T value) => Register(id.ToString(), value);

	public bool TryGet(string id, out T? value);

	public bool TryGet(RecordId id, out T? value) => TryGet(id.ToString(), out value);

	public T Get(string id);

	public T Get(RecordId id) => Get(id.ToString());
} 