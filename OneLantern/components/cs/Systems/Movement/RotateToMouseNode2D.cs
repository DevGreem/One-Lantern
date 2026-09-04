using Godot;
#nullable enable

[GlobalClass]
[Tool]
public partial class RotateToMouseNode2D : RotateComponent
{
	public override void _Process(double delta)
	{

		if (target is null || !Active || Engine.IsEditorHint())
			return;
		
		AngleDirection = target.GlobalPosition.DirectionTo(target.GetGlobalMousePosition());
		base._Process(delta);
	}
}