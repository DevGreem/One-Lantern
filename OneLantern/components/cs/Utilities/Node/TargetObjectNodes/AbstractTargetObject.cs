using Godot;
using Godot.Collections;
using System;

[Tool]
public abstract partial class AbstractTargetObject<[MustBeVariant] T> : Resource, ITargetObject<T>
{

	protected T _target;

	protected string[] _propertyNames = [];

	public virtual T Target
	{
		get => _target;
		set
		{
			if (_target.Equals(value))
				return;

			_target = value;
			CallDeferred(nameof(ReloadTargetProperties));
		}
	}

	[Export]
	public StringName Property { get; set; } = "";

	protected abstract void ReloadTargetProperties();

	public override void _ValidateProperty(Dictionary property)
	{
		if (property["name"].AsStringName() != nameof(Property))
			return;
		
		property["hint"] = (int)PropertyHint.EnumSuggestion;
		property["hint_string"] = _propertyNames.Join(",");
	}
}