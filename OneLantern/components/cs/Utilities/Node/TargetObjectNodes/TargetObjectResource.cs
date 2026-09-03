using Godot;
using Godot.Collections;
using System;

[GlobalClass]
[Tool]
public partial class TargetObjectResource : TargetObject
{
	
	public override void _ValidateProperty(Dictionary property)
	{
		if (property["name"].AsString() == nameof(Target))
		{
			property["hint"] = (int)PropertyHint.ResourceType;
			property["hint_string"] = "Resource";
		}

		base._ValidateProperty(property);
	}
}
