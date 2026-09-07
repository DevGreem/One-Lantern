
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;
#nullable enable

public abstract partial class Registry<[MustBeVariant] T> : Resource, IRegistry<T> where T: Resource
{

	[Signal]
	public delegate void ReadyEventHandler();

	public abstract string Id { get; protected set; } 

	[Export]
	public string[] RecordsPaths { get; private set; } = [];

	[Export]
	public bool RecursiveSearch { get; private set; } = false;

	public bool IsReady { get; private set; } = false;

	public Dictionary<string, T> Entries { get; protected set; } = new();

	protected virtual Dictionary<string, T> InspectorEntries { get => Entries; set => Entries = value; }

	public Registry()
	{
		_ = LoadPaths();
		GD.Print($"{nameof(Registry)}: Successfully loaded, data starts as = {Entries}");
	}

	public bool Register(string id, T value)
	{
		if (Entries.ContainsKey(id))
			return false;
		
		Entries[id] = value;
		return true;
	}

	public bool Unregister(string id) => Entries.Remove(id);

	public bool TryGet(string id, out T? value) => Entries.TryGetValue(id, out value);

	public T Get(string id) => Entries[id];

	public bool Contains(string id) => Entries.ContainsKey(id);

	protected async Task LoadPaths()
	{
		System.Collections.Generic.List<Task> tasks = new();

		foreach (string path in RecordsPaths)
		{
			tasks.Add(LoadRecords(path));
		}

		await Task.WhenAll(tasks);

		IsReady = true;
	}

	protected async Task LoadRecords(string path)
	{

		System.Collections.Generic.List<Task> tasks = new();
		
		if (RecursiveSearch)
		{
			foreach (string dir in DirAccess.GetDirectoriesAt(path))
			{
				tasks.Add(LoadRecords(dir));
			}
		}
		
		foreach (string file in DirAccess.GetFilesAt(path))
		{
			if (!FileUtils.IsResource(file))
				continue;
			
			T record = ResourceLoader.Load<T>(file);

			if (record is not T)
				continue;
			
			Register(
				Path.GetFileName(file),
				record
			);
			GD.Print($"{nameof(Registry)}: Loaded record {record} in registry \"{Id}\"");
		}

		await Task.WhenAll(tasks);
	}
}