using System;
using Godot;

/// <summary>
/// Only for internal use, don't for possible mods
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public partial class AddRecordAttribute: Attribute
{
	public string RegistryId { get; }

	public AddRecordAttribute(string registryId)
	{
		RegistryId = registryId;
	}
}