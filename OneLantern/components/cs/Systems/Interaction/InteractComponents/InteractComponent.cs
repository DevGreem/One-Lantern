using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
#nullable enable

public abstract partial class InteractComponent<TActor, T, TArea> : Node, IInteractComponent<TActor, T> where TActor: Node where T: IInteractionArea where TArea: CanvasItem
{
	
	public abstract TActor? Actor { get; protected set; }

	private TArea _interactArea = default!;

	public virtual TArea InteractArea
	{
		get => _interactArea;
		protected set
		{
			_interactArea = value;
			UpdateConfigurationWarnings();
		}
	}

	[Export]
	public bool CanInteract { get; set; } = true;

	[Export]
	public bool CanInteractThroughWalls { get; set; }= false;

	protected Dictionary<T, bool> detectedAreas = [];

	private T? _focusedInteractable;

	public virtual T? FocusedInteractable
	{
		get => _focusedInteractable;
		set
		{
			
			if (_focusedInteractable is not null)
			{
				
				if (_focusedInteractable.Equals(value))
					return;
				
				_focusedInteractable.Unfocus();
			}

			_focusedInteractable = value;
			
			if (_focusedInteractable is not null)
				_focusedInteractable.Focus();
		}
	}

	public override abstract void _Ready();

	public override abstract void _Process(double delta);

	protected abstract bool RegisterArea(T area);

	protected abstract bool UnregisterArea(T area);

	public virtual bool Interact()
	{
		if (!CanInteract || FocusedInteractable is null)
			return false;
		
		FocusedInteractable.Interact(this);

		return true;
	}

	protected abstract T? GetCloserInteractable();

	public override string[] _GetConfigurationWarnings()
	{
		
		string[] warnings = [];

		if (InteractArea is null)
			warnings.Append("You must assign an Area!");
		
		return warnings;
	}
}
