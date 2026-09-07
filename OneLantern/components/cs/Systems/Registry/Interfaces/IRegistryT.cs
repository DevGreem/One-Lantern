
using System.Collections.Generic;
#nullable enable

public partial interface IRegistry<T> : IRegistry
{
	public IReadOnlyDictionary<string, T> Entries { get; }

	public bool Register(string id, T value);

	public bool Register(RegistryId id, T value) => Register(id.ToString(), value);

	public bool TryGet(string id, out T? value);

	public bool TryGet(RegistryId id, out T? value) => TryGet(id.ToString(), out value);

	public T Get(string id);

	public T Get(RegistryId id) => Get(id.ToString());
} 