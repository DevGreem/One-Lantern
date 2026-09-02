using Godot;

[GlobalClass]
public partial class DebugNode : Node
{
	[Export]
	private Node target;

	[Export]
	private bool onlyWorksWithOwner;

	public override void _Ready()
	{
		if (!OS.IsDebugBuild())
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