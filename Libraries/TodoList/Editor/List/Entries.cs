namespace Todo.List;

internal sealed class TodoEntry
{
	public string Message { get; set; } = "None";

	public string Group { get; set; } = "Default";

	public bool IsDone { get; set; } = false;
}

internal struct CodeEntry
{
	public string SourceFile = "";
	public string Message = "";
	public int SourceLine = 0;

	public CodeEntry()
	{
	}
}
