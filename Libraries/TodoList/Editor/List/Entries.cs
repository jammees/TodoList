namespace Todo.List;

internal sealed class TodoEntry
{
	public string Message { get; set; } = "None";

	public string Group { get; set; } = "Default";

	public bool IsDone { get; set; } = false;
}

internal struct CodeEntry
{
	public string Message = "";

	public string SourceFile = "";
	public int SourceLine = 0;

	public TodoCodeStyle Style;

	public CodeEntry()
	{
	}
}
