using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial interface ITargetObject<T>
{
	
	[Export]
	public T Target { get; }

	[Export]
	public StringName Property { get; }
}
