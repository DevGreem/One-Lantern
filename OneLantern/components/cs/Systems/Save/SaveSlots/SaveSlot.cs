using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
#nullable enable

[GlobalClass]
public partial class SaveSlot : Resource, ISaveable
{

	[Export]
	public string Name { get; private set; } = "";

	[Export]
	public Resource SaveData { get; set; } = new();

	public bool IsBinary => ResourcePath.EndsWith(".res");
	
	public SaveSlot()
	{
		Callable.From(() => GD.Print($"{nameof(SaveSlot)}: Save slot \"{Name}\" loaded!")).CallDeferred();
	}

	public SaveSlot(string name)
	{
		Name = name;
		GD.Print($"{nameof(SaveSlot)}: Save slot \"{name}\" loaded!");
	}

	public void Save()
	{
		string path = GetSlotPath();
		
		ResourceSaver.Save(this, GetSlotPath());
	}

	public static SaveSlot? Open(string name)
	{

		string path;

		if (ResourceLoader.Exists(GetSlotPath(name, true)))
		{
			path = GetSlotPath(name, true);
		}
		else if (ResourceLoader.Exists(GetSlotPath(name, false)))
		{
			path = GetSlotPath(name, false);
		}
		else
		{
			return null;
		}

		
		SaveSlot slot = ResourceLoader.Load<SaveSlot>(path);

		return slot;
	}
	

	public static SaveSlot AddSlot(string name, bool saveBinary = true)
	{
		return AddSlot(name, new Resource(), saveBinary);
	}

	public static SaveSlot AddSlot(string name, Resource saveData, bool saveBinary = true)
	{
		SaveSlot slot = new(name)
		{
			SaveData = saveData
		};

		ResourceSaver.Save(slot, GetSlotPath(name, saveBinary));

		return slot;
	}

	public bool EraseSlot()
	{
		return EraseSlot(this.Name);
	}

	public static bool EraseSlot(string name)
	{
		string path = GetSlotPath(name);

		if (!FileAccess.FileExists(path))
			return false;
		
		Error result = DirAccess.RemoveAbsolute(path);
		
		if (result != Error.Ok)
			return false;
		
		return true;
	}

	public static bool Exists(string name)
	{
		bool withBinary = FileAccess.FileExists(GetSlotPath(name, true));
		bool notBinary = FileAccess.FileExists(GetSlotPath(name, false)); 

		return notBinary || withBinary;
	}

	public string GetSlotPath()
	{
		return GetSlotPath(Name, IsBinary);
	}

	protected static string GetSlotPath(string name, bool isBinary = true)
	{

		string resourcePath = SaveManager.Instance.SavePath;
		string extension = isBinary ? ".res" : ".tres";

		if (!resourcePath.EndsWith("/"))
		{
			resourcePath += "/";
		}

		return resourcePath + name + extension;
	}

	protected static string GetExtension(bool isBinary)
	{
		return isBinary ? ".res" : ".tres";
	}
}
