using System;
using System.Collections.Generic;
using System.Linq;
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

		if (Engine.IsEditorHint())
			return;

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
		
		GD.Print($"{nameof(MRegistryManager)}: Loading project [color=red]...[/color]");
		LoadProjectRegistries();

		IsReady = true;
		EmitSignalReady();
		GD.PrintRich($"{nameof(MRegistryManager)}: Project [color=green]loaded[/color]!");
	}

	private void LoadProjectRegistries()
	{

		var files = DirAccess.GetFilesAt(ModdableRegistries.RegistriesPath);

		foreach (string file in files)
		{
			string path = ModdableRegistries.RegistriesPath.PathJoin(file);
			
			if (!ResourceLoader.Exists(path))
				continue;
			
			var resource = ResourceLoader.Load<MRegistry>(path);

			if (resource is not IMRegistry)
			{
				GD.PushWarning($"{nameof(MRegistryManager)}: Loaded registry is not a {nameof(IMRegistry)}");
				continue;
			}

			if (_registries.ContainsKey(resource.Id))
			{
				GD.Print($"{nameof(MRegistryManager)}: Registry {resource.Id} already added.");
				continue;
			}
			
			AddRegistry(resource);
			_ = resource.Load();
			GD.Print($"{nameof(MRegistryManager)}: Registry {file} loaded!");
		}
	}
}