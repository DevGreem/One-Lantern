
using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public partial class SetSceneAttribute : Attribute
{
	public string ScenePath { get; private set; }

	public SetSceneAttribute(string scenePath)
	{
		ScenePath = scenePath;
	}
}