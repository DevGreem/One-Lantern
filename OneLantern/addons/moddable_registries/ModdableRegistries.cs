#if TOOLS
using Godot;
using Godot.Collections;
using System;

[Tool]
public partial class ModdableRegistries : EditorPlugin
{

	public const string GROUP = "RegistryConfiguration";
	public const string REGISTRIES_PATH_CONFIG = GROUP + "/RegistriesPath";

	public static string RegistriesPath => ProjectSettings.GetSetting(REGISTRIES_PATH_CONFIG, "").AsString();

	public override void _EnterTree()
	{
		SetupConfigs();
	}

	public override void _ExitTree()
	{
		RemoveConfigs();
	}

	private void SetupConfigs()
	{
		
		if (!ProjectSettings.HasSetting(REGISTRIES_PATH_CONFIG))
		{
			ProjectSettings.SetSetting(REGISTRIES_PATH_CONFIG, "");
			ProjectSettings.AddPropertyInfo(new Dictionary()
			{
				{ "name", REGISTRIES_PATH_CONFIG },
				{ "type", (int)Variant.Type.String },
				{ "hint", (int)PropertyHint.Dir }
			});
		}
	}

	private void RemoveConfigs()
	{
		if (ProjectSettings.HasSetting(REGISTRIES_PATH_CONFIG))
		{
			ProjectSettings.Clear(REGISTRIES_PATH_CONFIG);
		}
	}
}
#endif
