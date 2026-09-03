using System.ComponentModel;
using System.Linq;
using Godot;
using Godot.Collections;

[GlobalClass]
[Tool]
public partial class VectorMoveInputComponent : InputComponent
{

	[Export]
	public StringName negativeX { get; protected set; }
	
	[Export]
	public StringName positiveX { get; protected set; }

	[Export]
	public StringName negativeY { get; protected set; }

	[Export]
	public StringName positiveY { get; protected set; }

	public virtual Vector2 GetAxis()
	{
		return Input.GetVector(negativeX, positiveX, negativeY, positiveY);;
	}

	public override void _ValidateProperty(Dictionary property)
	{
		
		string[] props = [nameof(negativeX), nameof(positiveX), nameof(negativeY), nameof(positiveY)];

		if (props.Contains(property["name"].AsString()))
		{
			property["hint"] = (int)PropertyHint.EnumSuggestion;

			Array<StringName> actions = InputMap.GetActions();

			property["hint_string"] = string.Join(",", actions);
		}
	}
}