using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Godot;
#nullable enable

[GlobalClass, Icon("res://addons/at-icons/node/tap.svg")]
[Tool]
public partial class InteractComponent2D: InteractComponent<Node2D, InteractionArea2D, Area2D>
{

	[Signal]
	public delegate void InteractionAddedEventHandler(InteractionArea2D area);

	[Signal]
	public delegate void InteractionRemovedEventHandler(InteractionArea2D area);
	
	[Signal]
	public delegate void InteractedEventHandler();

	[Signal]
	public delegate void FocusedInteractableChangedEventHandler(InteractionArea2D newArea);

	[Export]
	public override Node2D? Actor { get; protected set; }

	[Export]
	public override Area2D InteractArea { get => base.InteractArea; protected set => base.InteractArea = value; }

	[Export]
	public override InteractionArea2D? FocusedInteractable { get => base.FocusedInteractable; set => base.FocusedInteractable = value; }

	[Export(PropertyHint.Layers2DPhysics)]
	public uint blockLayers = 0;

	public override void _Ready()
	{
		if (Engine.IsEditorHint())
			return;

		InteractArea.AreaEntered += OnAreaEntered;
		InteractArea.AreaExited += OnAreaExited;
	}

	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint())
			return;
		
		if (!CanInteract)
			return;
		
		foreach (var area in detectedAreas)
		{
			
			if (!IsInstanceValid(area.Key))
			{
				UnregisterArea(area.Key);
				continue;
			}

			if (!area.Key.Active)
			{
				detectedAreas[area.Key] = false;

				if (area.Key == FocusedInteractable)
					FocusedInteractable = null;
				
				continue;
			}

			if (CanInteractThroughWalls)
			{
				detectedAreas[area.Key] = true;
				continue;
			}

			if (IsInstanceValid(area.Key))
			{
				detectedAreas[area.Key] = Actor.CanReach(area.Key, [InteractArea.GetRid()], blockLayers);
			}
			else
			{
				detectedAreas[area.Key] = false;
			}
		}

		FocusedInteractable = GetCloserInteractable();
	}

	public override bool Interact()
	{
		var result = base.Interact();

		if (result)
			EmitSignalInteracted();

		return result;
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area is not InteractionArea2D)
			return;
		
		RegisterArea((area as InteractionArea2D)!);
	}

	private void OnAreaExited(Area2D area)
	{
		if (area is not InteractionArea2D)
			return;
		
		UnregisterArea((area as InteractionArea2D)!);
	}

	protected override bool RegisterArea(InteractionArea2D area)
	{
		if (detectedAreas.ContainsKey(area))
			return false;
		
		detectedAreas[area] = false;
		EmitSignalInteractionAdded(area);

		return true;
	}

	protected override bool UnregisterArea(InteractionArea2D area)
	{
		if (!detectedAreas.ContainsKey(area))
			return false;
		
		if (area == FocusedInteractable)
			FocusedInteractable = null;
		
		bool status = detectedAreas.Remove(area);

		if (status)
			EmitSignalInteractionRemoved(area);
		
		return status;
	}

	protected override InteractionArea2D? GetCloserInteractable()
	{
		
		if (detectedAreas.Count == 0 || !IsInstanceValid(Actor))
			return null;
		
		InteractionArea2D? bestArea = null;
		float bestDistance = Mathf.Inf;

		foreach (var area in detectedAreas)
		{
			if (!area.Value)
				continue;

			float distance = Actor.GlobalPosition.DistanceTo(area.Key.GlobalPosition);
			
			if (distance < bestDistance)
			{
				bestDistance = distance;
				bestArea = area.Key;
			}
		}
		
		return bestArea;
	}
}