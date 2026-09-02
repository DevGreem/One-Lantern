using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Data.Common;

[Tool]
public partial class SaveManager : Node, ISaveable
{

	[Signal]
	public delegate void SlotChangedEventHandler();

	public static SaveManager Instance { get; private set; }

	private const string _savePathSetting = "save_system/save_path";
	private const string _autoSaveSetting = "save_system/auto_save";
	private const string _saveTimeSetting = "save_system/save_time";

	public string SavePath => ProjectSettings.GetSetting(_savePathSetting).AsString();

	public bool AutoSaveActivated => ProjectSettings.GetSetting(_autoSaveSetting).AsBool();

	public double SaveTime => ProjectSettings.GetSetting(_saveTimeSetting).AsDouble();

	protected double _currentSaveTime = -1.0; 

	private SaveSlot _currentSaveSlot;

	[Export]
	public SaveSlot CurrentSaveSlot
	{
		get => _currentSaveSlot;
		set
		{
			_currentSaveSlot = value;
			EmitSignalSlotChanged();
		}
	}

	public LinkedList<SaveLoader> Loaders = new();

	public override void _Ready()
	{
		if (Instance is not null)
		{
			QueueFree();
			return;
		}

		Instance = this;

		if (!DirAccess.DirExistsAbsolute(SavePath))
		{
			DirAccess.MakeDirAbsolute(SavePath);
		}

		_currentSaveTime = SaveTime;
	}

	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint())
		{
			return;
		}

		if (!AutoSaveActivated)
			return;

		if (_currentSaveTime > 0.0)
		{
			_currentSaveTime -= delta;
		}
		else
		{
			_currentSaveTime = SaveTime;
			Save();
		}
	}

	public void Save()
	{
		GD.Print($"{nameof(SaveManager)}: Saving all...");
		
		foreach (var loader in Loaders)
		{
			loader.Save();
		}

		if (CurrentSaveSlot is not null)
		{
			CurrentSaveSlot.Save();
		}

		GD.Print($"{nameof(SaveManager)}: Save finished");
	}

	public SaveSlot GetSaveSlot()
	{
		return _currentSaveSlot;
	}

	public SaveSlot<T> GetSaveSlot<T>() where T: Resource
	{
		return (SaveSlot<T>)_currentSaveSlot;
	}

	public void OpenSlot(string name)
	{
		CurrentSaveSlot = SaveSlot.Open(name);
	}

	public override void _EnterTree()
	{
		
		if (!Engine.IsEditorHint())
			return;
		
		SetupSettings();
	}

	public override void _ExitTree()
	{
		if (!Engine.IsEditorHint())
			return;
		
		RemoveSettings();
	}

	private void SetupSettings()
	{
		
		if (!ProjectSettings.HasSetting(_savePathSetting))
		{
			ProjectSettings.SetSetting(_savePathSetting, "");
			ProjectSettings.AddPropertyInfo(new Dictionary()
			{
				{ "name", _savePathSetting },
				{ "type", (int)Variant.Type.String },
				{ "hint", (int)PropertyHint.FilePath }
			});
		}

		if (!ProjectSettings.HasSetting(_autoSaveSetting))
		{
			ProjectSettings.SetSetting(_autoSaveSetting, false);
			ProjectSettings.AddPropertyInfo(new Dictionary()
			{
				{ "name", _autoSaveSetting },
				{ "type", (int)Variant.Type.Bool }
			});
		}

		if (!ProjectSettings.HasSetting(_saveTimeSetting))
		{
			ProjectSettings.SetSetting(_saveTimeSetting, -1.0);
			ProjectSettings.AddPropertyInfo(new Dictionary()
			{
				{ "name", _saveTimeSetting },
				{ "type", (int)Variant.Type.Float },
				{ "hint", (int)PropertyHint.Range },
				{ "hint_string", $"-1.0,{float.MaxValue}"}
			});
		}
	}

	private void RemoveSettings()
	{
		if (ProjectSettings.HasSetting(_savePathSetting))
		{
			ProjectSettings.Clear(_savePathSetting);
		}

		if (ProjectSettings.HasSetting(_autoSaveSetting))
		{
			ProjectSettings.Clear(_autoSaveSetting);
		}

		if (ProjectSettings.HasSetting(_saveTimeSetting))
		{
			ProjectSettings.Clear(_saveTimeSetting);
		}
	}
}
