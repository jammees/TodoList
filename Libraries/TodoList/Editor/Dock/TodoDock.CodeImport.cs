using Editor;
using System.Collections.Generic;
using Todo.CodeImport;
using Todo.List;

namespace Todo;

internal sealed partial class TodoDock : Widget
{
	private Dictionary<string, List<CodeEntry>> ImportFromCode()
	{
		return ParseCode.ProcessFiles( FileUtility.GetAllFiles( new() { ".cs", ".razor" } ) );
	}
}
