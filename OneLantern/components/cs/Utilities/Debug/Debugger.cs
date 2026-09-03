using Godot;

public partial class Debugger : RefCounted
{
	
	public void Log<T>(T executer, params string[] messages)
	{
		GD.Print($"{nameof(executer)}: {messages.Join("\n")}");
	}
}