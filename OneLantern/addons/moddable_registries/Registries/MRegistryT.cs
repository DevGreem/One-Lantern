using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Godot;
using GDColl = Godot.Collections;
#nullable enable


[Tool]
public partial class MRegistry<[MustBeVariant] T> : Resource, IMRegistry<T> where T: Resource
{

	[Signal]
	public delegate void ReadyEventHandler();

	[Export]
	public string Id { get; protected set; } = "";

	[ExportGroup("Auto Record Inspection Configuration")]

	[Export]
	public string[] RecordsPaths { get; private set; } = [];

	[Export]
	public bool RecursiveSearch { get; private set; } = false;

	public bool IsReady { get; private set; } = false;

	public GDColl.Dictionary<string, T> Entries { get; protected set; } = new();

	protected virtual GDColl.Dictionary<string, T> InspectorEntries { get => Entries; set => Entries = value; }

	public MRegistry()
	{
		Entries = InspectorEntries;
	}

	public static implicit operator MRegistry<T>(MRegistry other)
	{
		MRegistry<T> resource = new()
		{
			Id = other.Id,
			RecordsPaths = other.RecordsPaths,
			RecursiveSearch = other.RecursiveSearch,
			Entries = (other.Entries as GDColl.Dictionary<string, T>)!
		};
		
		return resource;
	}

	public bool Query(string id, MRQueryType queryType, T value)
	{
		if (queryType == MRQueryType.ADD)
			return Register(id, value);
		
		if (queryType == MRQueryType.EDIT)
			return EditRecord(id, value);
		
		if (queryType == MRQueryType.REPLACE)
			return ReplaceRecord(id, value);
		
		return false;
	}

	public bool Register(string id, T value)
	{
		if (Entries.ContainsKey(id))
			return false;
		
		Entries[id] = value;
		return true;
	}

	public bool EditRecord(string id, T newValue)
	{
		T? record;

		bool status = TryGet(id, out record);
		
		if (!status)
			return false;

		foreach (var property in typeof(T).GetProperties())
		{
			if (!property.CanRead || !property.CanWrite)
				continue;
			
			property.SetValue(record, property.GetValue(newValue));
		}

		return true;
	}

	public bool ReplaceRecord(string id, T newValue)
	{

		var status = Entries.ContainsKey(id);
		
		Entries[id] = newValue;

		return !status;
	}

	public bool Unregister(string id) => Entries.Remove(id);

	public bool TryGet(string id, out T? value) => Entries.TryGetValue(id, out value);

	public T Get(string id) => Entries[id];

	public ReadOnlyCollection<T> GetIf(Func<T, bool> query)
	{
		List<T> values = [];
		
		foreach (var record in Entries)
		{
			if (query(record.Value))
				values.Add(record.Value);
		}

		return values.AsReadOnly();
	}

	public bool Contains(string id) => Entries.ContainsKey(id);

	public async Task Load()
	{
		await Task.Run(LoadPaths);

		IsReady = true;
		EmitSignalReady();
		GD.Print($"{nameof(MRegistry)}: Successfully loaded, data starts as = {Entries}");
	}

	private async Task LoadPaths()
	{
		List<Task> tasks = new();

		foreach (string path in RecordsPaths)
		{
			var dirAccess = DirAccess.Open(path);

			if (dirAccess is null)
			{
				GD.PrintErr($"{nameof(MRegistry)}: The path \"{path}\" don't exists");
				continue;
			}

			tasks.Add(LoadRecords(path));
		}

		await Task.WhenAll(tasks);
	}

	private async Task LoadRecords(string path)
	{

		List<Task> tasks = new();
		
		if (RecursiveSearch)
		{
			foreach (string dir in DirAccess.GetDirectoriesAt(path))
			{
				tasks.Add(LoadRecords(path.PathJoin(dir)));
			}
		}
		
		foreach (string file in DirAccess.GetFilesAt(path))
		{

			string filePath = path.PathJoin(file);

			if (!ResourceLoader.Exists(filePath))
				continue;
			
			LoadRecord(filePath);
		}

		await Task.WhenAll(tasks);

		
	}

	protected virtual void LoadRecord(string file)
	{
		Resource resource = ResourceLoader.Load(file);

		if (resource is not Record<T> record)
			return;
		
		Query(record.Id, record.queryType, record.Value);;
		GD.Print($"{nameof(MRegistry)}: Loaded record {record.Id} in registry \"{Id}\"");
	}

	public override void _ValidateProperty(GDColl.Dictionary property)
	{
		
		if (property["name"].AsString() == nameof(RecordsPaths))
			return;
		
		property["hint_string"] = $"{(int)Variant.Type.String}/{(int)PropertyHint.Dir}:";
	}
}