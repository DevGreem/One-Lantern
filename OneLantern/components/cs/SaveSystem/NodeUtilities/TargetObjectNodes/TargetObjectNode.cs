using Godot;
using Godot.Collections;
using System;

[GlobalClass]
[Tool]
public partial class TargetObjectNode : TargetObject
{
	public override void _ValidateProperty(Dictionary property)
	{
		if (property["name"].AsString() == nameof(Target))
		{
			property["hint"] = (int)PropertyHint.NodeType;
			property["hint_string"] = "Node";

			return;
		}

		base._ValidateProperty(property);
	}
}
