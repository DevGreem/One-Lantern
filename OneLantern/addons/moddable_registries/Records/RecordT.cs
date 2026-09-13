using System.IO;
using Godot;
using Godot.Collections;

[Tool]
public partial class Record<T>: Resource, IMRecord<T> where T: Resource
{
	[Export]
	public string Id { get; private set; }

	public virtual T Value { get; protected set; }

	[Export]
	public MRQueryType QueryType { get; private set; } = MRQueryType.ADD;

	private bool _useDirName = true;

	[Export]
	private bool UseDirName
	{
		get => _useDirName;
		set
		{
			if (_useDirName == value)
				return;
			
			_useDirName = value;
			NotifyPropertyListChanged();

			if (_useDirName)
				Id = GetContainerDirName();
		}
	}

	private string GetContainerDirName()
	{
		string dir = Path.GetDirectoryName(ResourcePath);

		string dirName = Path.GetFileName(dir);

		return dirName;
	}

	public override void _ValidateProperty(Dictionary property)
	{
		
		if (property["name"].AsStringName() == PropertyName.Id)
		{
			if (_useDirName)
			{
				property["usage"] = (int)(property["usage"].As<PropertyUsageFlags>() | PropertyUsageFlags.ReadOnly);
			}
		}
	}
	
}