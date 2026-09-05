using Godot;

[GlobalClass, Icon("res://addons/at-icons/node/heart.svg")]
[Tool]
public partial class HealthComponent : HealthComponent<float>
{

	[Export]
	public override float MaxHealth { get => base.MaxHealth; set => base.MaxHealth = value; }

	[Export]
	public override float Health { get => base.Health; set => base.Health = value; }
}