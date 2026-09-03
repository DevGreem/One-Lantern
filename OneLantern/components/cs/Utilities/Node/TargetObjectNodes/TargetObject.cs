using Godot;
using Godot.Collections;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
#nullable enable

[GlobalClass]
[Tool]
public partial class TargetObject : AbstractTargetObject<GodotObject?>
{

	[Export]
	public override GodotObject? Target
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

	public T? GetTarget<T>() where T: GodotObject => (T?)Target;

	public void SetPropertyValue(Variant value)
	{
		if (Target is null)
			return;

		GD.Print($"{nameof(TargetObject)}: Setting \"{Property}\" value to \"{value}\"");
		Target.Set(Property, value);
	}

	public Variant GetPropertyValue()
	{
		if (Target is null)
			return default;
		
		Variant value = Target.Get(Property);

		GD.Print($"{nameof(TargetObject)}: GETTED RAW VALUE = \"{value}\"");
		GD.Print($"{nameof(TargetObject)}: TYPE = {value.VariantType}");

		return value;
	}

	public T? GetPropertyValue<[MustBeVariant] T>() => GetPropertyValue().As<T>();

	protected override void ReloadTargetProperties()
	{
		if (!Engine.IsEditorHint())
			return;

		if (!IsInstanceValid(_target))
		{
			_propertyNames = [];
		}
		else
		{
			_propertyNames = _target!.GetPropertyList().Select(x => x["name"].AsString()).ToArray();
		}

		NotifyPropertyListChanged();
	}
}
