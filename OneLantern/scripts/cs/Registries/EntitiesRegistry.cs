
using System;
using System.Reflection;
using Godot;
#nullable enable

[GlobalClass]
[AddRegistry]
public partial class EntitiesRegistry: Registry<PackedScene>
{
	public override string Id { get; protected set; } = "entities";

	protected override void LoadRecords()
	{
		
		foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
		{
			foreach (var type in assembly.GetTypes())
			{
				foreach (var attribute in type.GetCustomAttributes<AddRecordAttribute>())
				{
					if (attribute.RegistryId != Id)
						continue;
					
					SetSceneAttribute? sceneAttribute = type.GetCustomAttribute<SetSceneAttribute>();

					// if (sceneAttribute is null)
					// 	Register();
				}
			}
		}
	}
}