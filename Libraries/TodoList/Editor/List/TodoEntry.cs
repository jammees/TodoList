namespace Todo.List;

internal class TodoEntry
{
	public string Message { get; set; } = "None";

	public string Group { get; set; } = "Default";

	public bool IsDone { get; set; } = false;
}
