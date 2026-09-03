using Godot;
using Godot.Collections;

[GlobalClass]
[Tool]
public partial class VectorMoveInputComponent2D : VectorMoveInputComponent, ITarget<MoveComponent2D>
{
	
	public MoveComponent2D Target { get; protected set; }

	[Export(PropertyHint.NodeType, "MoveComponent2D")]
	private Node InspectorTarget { get; set; }

	public override void _Ready()
	{
		if (Engine.IsEditorHint())
			return;
		
		Target = (MoveComponent2D)InspectorTarget;
	}

	public override void _Input(InputEvent @event)
	{
		
		if (Engine.IsEditorHint() || !Active)
			return;

		var axis = GetAxis();
		Target.Direction = axis;

		if (axis != Vector2.Zero)
		{
			EmitSignalInputDetected();
		}
	}

	// public override void _ValidateProperty(Dictionary property)
	// {
	// 	base._ValidateProperty(property);

	// 	if (property["name"].AsString() == "InspectorTarget")
	// 	{
	// 		property["hint"] = PropertyHint.NodeType
	// 	}
	// }
}