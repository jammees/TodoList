using System.Collections.Generic;

namespace Todo.List;

internal class EntryGroup
{
	public string Group { get; set; } = "Default";

	public List<TodoEntry> Datas { get; set; }
	public bool IsOpen { get; set; } = true;
}
