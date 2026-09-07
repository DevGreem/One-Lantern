using Godot;

public static partial class FileAccessExtensions
{
	
	public static bool IsResource(this FileAccess obj, string path)
	{
		return path.EndsWith(".tres") || path.EndsWith(".res");
	}
}