using Godot;

public partial class InteractionArea2D: Area2D, IInteractionArea
{
	
	[Signal]
	public delegate void InteractedEventHandler();

	[Signal]
	public delegate void FocusedEventHandler();

	[Signal]
	public delegate void UnfocusedEventHandler();

	[Export]
	public bool Active { get; set; } = true;

	public void Interact(Node node)
	{
		if (!Active)
			return;
		
		EmitSignalInteracted();
	}

	public void Focus()
	{
		if (!Active)
			return;
		
		EmitSignalFocused();
	}

	public void Unfocus()
	{
		if (!Active)
			return;
		
		EmitSignalUnfocused();
	}
}