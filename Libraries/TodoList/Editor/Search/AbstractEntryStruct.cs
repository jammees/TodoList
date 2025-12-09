using Todo.List;

namespace Todo.Search;

internal struct AbstractEntry
{
	public object Entry;

	public string Message;
	public string Group;

	public CodeEntry CodeEntry => (CodeEntry)Entry;

	public TodoEntry TodoEntry => (TodoEntry)Entry;

	public bool IsCode => Entry is CodeEntry;
}
