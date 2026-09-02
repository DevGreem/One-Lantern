using Godot;
using Godot.Collections;
using System;
using System.Linq;
#nullable enable

[GlobalClass]
[Tool]
public partial class TargetObjectNodePath : AbstractTargetObject<NodePath?>
{

	[Export]
	public override NodePath? Target 
	{
		get => _target;
		set
		{
			if (_target == value)
				return;
			
			_target = value;
			CallDeferred(nameof(ReloadTargetProperties));
		} 
	}

	protected override void ReloadTargetProperties()
	{

		if (!Engine.IsEditorHint())
			return;
		
		Node scene = EditorInterface.Singleton.GetEditedSceneRoot();

		if (scene is null)
		{
			_propertyNames = [];
			NotifyPropertyListChanged();
			return;
		}

		Node targetNode = scene.GetNodeOrNull(Target);
		
		if (targetNode is null || !IsInstanceValid(targetNode))
		{
			_propertyNames = [];
		}
		else
		{
			_propertyNames = targetNode!.GetPropertyList().Select(x => x["name"].AsString()).ToArray();
		}

		NotifyPropertyListChanged();
	}

	public override void _ValidateProperty(Dictionary property)
	{
		base._ValidateProperty(property);
	}
}
