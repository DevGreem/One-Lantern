using Godot;

[GlobalClass, Icon("res://addons/at-icons/node/thumbtack.svg")]
public partial class Record: Record<Resource>
{
	[Export]
	public override Resource Value => base.Value;
}