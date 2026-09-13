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
		Type valueType = record.Value.GetType();

		foreach (var resource in _searchTypes)
		{
			if (valueType.IsAssignableFrom(resource.GetType()))
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

	// public override void _ValidateProperty(Dictionary property)
	// {
	// 	base._ValidateProperty(property);

	// 	if (property["name"].AsString() != nameof(_searchTypes))
	// 		return;
		
	// 	property["type"] = (int)Variant.Type.Array;
	// 	property["hint"] = (int)PropertyHint.TypeString;
	// 	property["hint_string"] = $"{(int)Variant.Type.Object}/{(int)PropertyHint.ResourceType}:Resource";
	// }
}