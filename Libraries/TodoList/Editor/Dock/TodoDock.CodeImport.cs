using Editor;
using Sandbox;
using System.Collections.Generic;
using System.IO;
using Todo.CodeImport;
using Todo.List;

namespace Todo;

internal sealed partial class TodoDock : Widget
{
	private Dictionary<string, List<CodeEntry>> ImportFromCode()
	{
		string codeFolderPath = Project.Current.GetCodePath();

		EnumerationOptions options = new()
		{
			RecurseSubdirectories = true,
			IgnoreInaccessible = true,
		};

		Dictionary<string, List<CodeEntry>> results = new();

		ParseCode.ProcessFiles( codeFolderPath, "*.cs", options, ref results );
		ParseCode.ProcessFiles( codeFolderPath, "*.razor", options, ref results );

		return results;
	}
}
