
public readonly record struct RecordId(string Namespace, string name)
{
	public override string ToString() => $"{Namespace}:{name}";
}