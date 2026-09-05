using System.Numerics;
using Godot;

public partial class HealthComponent<T>: Node where T: INumber<T>
{

	[Signal]
	public delegate void DiedEventHandler();

	[Signal]
	public delegate void MaxHealthChangedEventHandler();
	
	[Signal]
	public delegate void HealthChangedEventHandler();

	[Export]
	public bool canChangeMaxHealth = true;

	[Export]
	public bool canChangeHealth = true;

	private T _maxHealth = default;

	public virtual T MaxHealth
	{
		get => _maxHealth;
		set
		{
			if (_maxHealth == value || !canChangeMaxHealth)
				return;
			
			_maxHealth = value;
			EmitSignalMaxHealthChanged();

			if (_maxHealth < Health)
				Health = _maxHealth;
		}
	}

	private T _health = default;

	public virtual T Health
	{
		get => _health;
		set
		{
			if (_health == value || !canChangeHealth)
				return;
			
			if (value > MaxHealth)
				value = MaxHealth;

			_health = value;
			EmitSignalHealthChanged();

			if (!Engine.IsEditorHint())
				VerifyDie();
		}
	}

	private void VerifyDie()
	{
		if (Health > T.Zero)
			return;
		
		EmitSignalDied();
	}
}