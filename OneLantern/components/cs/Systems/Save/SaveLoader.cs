using Godot;
using Godot.Collections;
using System.Linq;

[GlobalClass]
public partial class SaveLoader : Node, ISaveable
{

	[Signal]
	public delegate void LoadedEventHandler();

	[Signal]
	public delegate void SavedEventHandler();

	[Export]
	public TargetObjectNodePath TargetNode { get; set; }

	[Export]
	public TargetObjectResource SlotResource { get; set; }

	public override void _Ready()
	{
		if (Engine.IsEditorHint())
			return;
		
		SaveManager.Instance.Loaders.AddLast(this);
	}

	public override void _ExitTree()
	{
		if (Engine.IsEditorHint())
			return;
		
		SaveManager.Instance.Loaders.Remove(this);
	}

	public async void Load()
	{
		GD.Print($"{nameof(SaveLoader)}: Loading save data...");

		if (SaveManager.Instance.CurrentSaveSlot is null)
		{
			await ToSignal(SaveManager.Instance, SaveManager.SignalName.SlotChanged);
		}

		Variant value = SaveManager.Instance.CurrentSaveSlot.SaveData.Get(SlotResource.Property);

		GD.Print($"{nameof(SaveLoader)}: Loading value = \"{value}\" for property = \"{SlotResource.Property}\"");

		GetNode(TargetNode.Target).Set(TargetNode.Property, value);

		GD.Print($"{nameof(SaveLoader)}: Save data loaded!");
		EmitSignalLoaded();
	}
	
	public void Save()
	{
		var slot = SaveManager.Instance.CurrentSaveSlot;

		Variant value = GetNode(TargetNode.Target).Get(TargetNode.Property);
		

		GD.Print($"{nameof(SaveLoader)}: Saving property \"{TargetNode.Property}\" with value \"{value}\" to data \"{SlotResource.Property}\"");

		SaveManager.Instance.CurrentSaveSlot.SaveData.Set(
			SlotResource.Property,
			value
		);

		EmitSignalSaved();
	}
}
