using Godot;
using Godot.Collections;

[GlobalClass]
public partial class MRegistry : MRegistry<Resource>
{

	[Export]
	protected override Dictionary<string, Resource> InspectorEntries { get => base.InspectorEntries; set => base.InspectorEntries = value; }
}