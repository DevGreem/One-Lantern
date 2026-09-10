using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
#nullable enable

[Tool]
public partial class MRegistryManager : Node
{

	[Signal]
	public delegate void ReadyEventHandler();

	public static MRegistryManager Instance { get; private set; } = default!;

	private readonly Dictionary<string, IMRegistry> _registries = new();

	public bool IsReady { get; private set; } = false;

	public MRegistryManager()
	{
		if (IsInstanceValid(Instance))
		{
			QueueFree();
			return;
		}

		Instance = this;
	}

	public override void _Ready()
	{
		if (Engine.IsEditorHint())
			return;

		LoadProject();
	}

	/// <summary>
	/// Function for gdscript
	/// </summary>
	/// <param name="id">ID of the Registry</param>
	/// <returns>Registry</returns>
	public MRegistry GetRegistry(string id) => (MRegistry)_registries[id];

	public T GetRegistry<T>(string id) where T: IMRegistry => (T)_registries[id];

	public T GetRegistry<T>(RecordId id) where T: IMRegistry => GetRegistry<T>(id.ToString());

	public bool AddRegistry(IMRegistry registry)
	{
		if (_registries.ContainsKey(registry.Id))
			return false;
		
		_registries.Add(registry.Id, registry);
		return true;
	}

	public bool RemoveRegistry(string id) => _registries.Remove(id);

	public bool RemoveRegistry(RecordId id) => RemoveRegistry(id.ToString());

	public bool RemoveRegistry(IMRegistry registry) => RemoveRegistry(registry.Id);

	private void LoadProject()
	{
		
		GD.Print($"{nameof(MRegistryManager)}: Loading project...");
		LoadProjectRegistries();

		IsReady = true;
		EmitSignalReady();
	}

	private void LoadProjectRegistries()
	{

		var files = DirAccess.GetFilesAt(ModdableRegistries.RegistriesPath);

		foreach (string file in files)
		{
			if (!FileUtils.IsResource(file))
				continue;
			
			var registry = ResourceLoader.Load<MRegistry>(file);

			if (registry is not IMRegistry)
				continue;
			
			AddRegistry(registry);
			_ = registry.Load();
		}
	}
}