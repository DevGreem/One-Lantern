using Godot;

public partial interface IMRecord: IUnique
{
	public MRQueryType QueryType { get; }
}