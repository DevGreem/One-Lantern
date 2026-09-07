using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
#nullable enable

[Tool]
public partial class RegistryManager : Node
{
	public static RegistryManager Instance { get; private set; } = default!;

	private readonly Dictionary<string, IRegistry> _registries = new();

	public bool IsLoaded { get; private set; } = false;

	public RegistryManager()
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

		_ = LoadProject();
	}

	/// <summary>
	/// Function for gdscript
	/// </summary>
	/// <param name="id">ID of the Registry</param>
	/// <returns>Registry</returns>
	public Registry GetRegistry(string id) => (Registry)_registries[id];

	public T GetRegistry<T>(string id) where T: IRegistry => (T)_registries[id];

	public T GetRegistry<T>(RecordId id) where T: IRegistry => GetRegistry<T>(id.ToString());

	public bool AddRegistry(IRegistry registry)
	{
		if (_registries.ContainsKey(registry.Id.ToString()))
			return false;
		
		_registries.Add(registry.Id.ToString(), registry);
		return true;
	}

	private bool AddRegistry(Type type)
	{
		IRegistry registry = (IRegistry)Activator.CreateInstance(type)!;
		return AddRegistry(registry);
	}

	public bool RemoveRegistry(string id) => _registries.Remove(id);

	public bool RemoveRegistry(RecordId id) => RemoveRegistry(id.ToString());

	public bool RemoveRegistry(IRegistry registry) => RemoveRegistry(registry.Id);

	private async Task LoadProject()
	{
		var assemblies = AppDomain.CurrentDomain.GetAssemblies();
		
		await Task.Run(() =>
		{
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				LoadProjectRegistries(assembly);
			}
		});
	}

	private void LoadProjectRegistries(Assembly assembly)
	{
		foreach (var type in assembly.GetTypes())
		{
			foreach (var attribute in type.GetCustomAttributes<AddRegistryAttribute>())
			{
				if (!typeof(IRegistry).IsAssignableFrom(type))
					continue;
				
				var result = AddRegistry(type);

				if (result)
					GD.Print($"{nameof(RegistryManager)}: Successfully added registry \"{type.FullName}\"");
				else
					GD.Print($"{nameof(RegistryManager)}: Error on add registry \"{type.FullName}\"");
			}
		}
	}
}