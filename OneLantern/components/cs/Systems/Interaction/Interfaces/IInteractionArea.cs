using System;
using System.ComponentModel;
using Godot;

public partial interface IInteractionArea
{

	[Signal]
	public delegate void InteractedEventHandler();

	[Signal]
	public delegate void FocusedEventHandler();

	[Signal]
	public delegate void UnfocusedEventHandler();

	public bool Active { get; set; }

	public abstract void Interact(Node component);

	public abstract void Focus();

	public abstract void Unfocus();
}