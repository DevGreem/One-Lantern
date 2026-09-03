using Godot;

public static partial class Vector2Extensions
{
	public static void Add(this Vector2 vector, float value)
	{
		vector.X += value;
		vector.Y += value;
	}
}