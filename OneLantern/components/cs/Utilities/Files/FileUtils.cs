
using System.IO;
using Godot;

public static class FileUtils
{
	public static bool IsResource(string path) => path.EndsWith(".tres") || path.EndsWith(".res");
}