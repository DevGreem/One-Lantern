using Godot;

[GlobalClass, Icon("res://addons/at-icons/node/keyboard.svg")]
public abstract partial class InputComponent : Node, IActivable
{

	[Signal]
	public delegate void InputDetectedEventHandler();

	[Export]
	public bool Active { get; set; } = true;

}