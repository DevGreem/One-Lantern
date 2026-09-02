using Godot;
using System;
using System.Data.Common;
using System.Runtime.CompilerServices;
#nullable enable

public partial class SaveSlot<T> : ISaveable where T: Resource
{

	private readonly SaveSlot _slot;

	public string Name => _slot.Name;

	public bool IsBinary => _slot.IsBinary;

	public T SaveData
	{
		get => (T)_slot.SaveData;
		set => _slot.SaveData = value;
	}

	public SaveSlot(SaveSlot slot)
	{
		_slot = slot;
	}

	public static implicit operator SaveSlot<T>(SaveSlot generalSlot)
	{
		SaveSlot<T> slot = new SaveSlot<T>(generalSlot);
		return slot;
	}

	public void Save()
	{
		_slot.Save();
	}
}
