
using System;
using System.Reflection;
using Godot;
using Godot.Collections;
#nullable enable

[GlobalClass]
public partial class EntitiesRegistry: MRegistry<PackedScene>
{

	[Export]
	protected override Dictionary<string, PackedScene> InspectorEntries { get => base.InspectorEntries; set => base.InspectorEntries = value; }

	// protected override void LoadRecords()
	// {
		
	// 	foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
	// 	{
	// 		foreach (var type in assembly.GetTypes())
	// 		{
	// 			foreach (var attribute in type.GetCustomAttributes<AddRecordAttribute>())
	// 			{
	// 				if (attribute.RegistryId != Id)
	// 					continue;
					
	// 				SetSceneAttribute? sceneAttribute = type.GetCustomAttribute<SetSceneAttribute>();

	// 				// if (sceneAttribute is null)
	// 				// 	Register();
	// 			}
	// 		}
	// 	}
	// }
}