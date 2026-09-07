using Godot;
#nullable enable

public partial interface IInteractComponent<TActor, TAreas> where TActor: Node where TAreas: IInteractionArea
{
	
	public TActor? Actor { get; }

	public bool CanInteract { get; set; }

	public bool CanInteractThroughWalls { get; set; }

	public abstract TAreas? FocusedInteractable { get; set; }

	public abstract bool Interact();
}