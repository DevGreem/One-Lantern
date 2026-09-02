using Godot;
using Godot.Collections;
using System;
using System.Linq;

[GlobalClass]
[Tool]
public partial class SlotLoader : Node
{

	[Signal]
	public delegate void LoadedEventHandler();
	
	[Export]
	public string SlotName { get; protected set; }

	private bool _addIfNotExists = false;

	[Export]
	public bool AddIfNotExists
	{
		get => _addIfNotExists;
		protected set
		{
			_addIfNotExists = value;
			NotifyPropertyListChanged();
		}
	}

	[Export]
	private bool _saveAsBinary = true;

	[Export]
	private Resource _saveData = new();

	public void Load()
	{

		if (Engine.IsEditorHint())
		{
			return;
		}

		SaveSlot slot = null;

		if (AddIfNotExists && !SaveSlot.Exists(SlotName))
		{
			slot = SaveSlot.AddSlot(SlotName, _saveData, _saveAsBinary);
		}

		if (slot is null)
		{
			slot = SaveSlot.Open(SlotName);
		}

		SaveManager.Instance.CurrentSaveSlot = slot;
		EmitSignalLoaded();
	}

	public override void _ValidateProperty(Dictionary property)
	{

		string[] props = ["_saveAsBinary", "_saveData"];
		
		if (props.Contains(property["name"].AsString()))
		{
			
			if (_addIfNotExists)
				return;
			
			property["usage"] = (int)PropertyUsageFlags.NoEditor;
		}
	}
}
