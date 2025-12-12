using Todo.List;

namespace Todo.Search;

public struct AbstractEntry
{
	public object Entry;

	public string Message;
	public string Group;

	public CodeEntry CodeEntry => (CodeEntry)Entry;

	public ManualEntry ManualEntry => (ManualEntry)Entry;

	public bool IsCode => Entry is CodeEntry;
}
