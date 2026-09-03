using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial interface ITargetObject<T>: ITarget<T>
{

	[Export]
	public StringName Property { get; }
}
