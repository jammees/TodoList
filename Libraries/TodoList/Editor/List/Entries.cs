namespace Todo.List;

internal class Entry
{
	public string Message { get; set; } = "None";

	public string Group { get; set; } = "Default";
}

internal sealed class CodeEntry: Entry
{
	public string SourceFile { get; set; } = "";

	public int SourceLine { get; set; } = 0;
}

internal sealed class TodoEntry: Entry
{
	public bool IsDone { get; set; } = false;
}
