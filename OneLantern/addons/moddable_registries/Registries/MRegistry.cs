using System;
using System.IO;
using Godot;
using Godot.Collections;
#nullable enable

[GlobalClass]
[Tool]
public partial class MRegistry : MRegistry<Resource>
{

	[Export]
	protected override Dictionary<string, Resource> InspectorEntries { get => base.InspectorEntries; set => base.InspectorEntries = value; }

	[Export]
	private Resource[] _searchTypes = [];

	public bool TryGet<T>(string id, out T? value) where T: Resource => TryGet(id, out value);

	public T Get<T>(string id) where T: Resource => (T)Get(id);

	protected override void LoadRecord(string file)
	{
		Resource loadedResource = ResourceLoader.Load(file);

		if (loadedResource is not Record record)
			return;

		bool correctType = false;
		Type recordType = record.GetType();

		foreach (Resource resource in _searchTypes)
		{
			if (recordType.IsAssignableFrom(resource.GetType()))
			{
				correctType = true;
				break;
			}
		}

		if (!correctType)
			return;
		
		Query(record.Id, record.QueryType, record.Value);
		GD.Print($"{nameof(MRegistry)}: Loaded record {file} in registry {this.Id} with value = {record.Value}");
	}
}