using Godot;

[GlobalClass, Icon("res://addons/at-icons/node/code.svg")]
[Tool]
public partial class DebugNode : Node, IActivable
{
	[Export]
	private Node target;

	[Export]
	private bool onlyWorksWithOwner = true;

	[Export]
	public bool Active { get; set; } = true;

	public override void _Ready()
	{
		if (!OS.IsDebugBuild() || !Active)
		{
			target.QueueFree();
		}

		if (onlyWorksWithOwner && IsInstanceValid(target))
		{
			if (target.Owner != GetTree().CurrentScene)
			{
				target.QueueFree();
			}
		}
	}
}