using Todo.CodeImport;

namespace Todo.List;

public sealed class TodoEntry
{
	public string Message { get; set; } = "None";

	public string Group { get; set; } = "Default";

	public bool IsDone { get; set; } = false;
}

public sealed class ManualEntry
{
	public string Message { get; set; } = "None";

	public string Group { get; set; } = "Default";

	public bool IsDone { get; set; } = false;
}

public struct CodeEntry
{
	public string Message = "";

	public string SourceFile = "";
	public int SourceLine = 0;

	public TodoCodeWord Style;

	public CodeEntry()
	{
	}
}
