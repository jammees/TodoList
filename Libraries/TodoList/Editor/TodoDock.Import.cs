using Editor;
using Sandbox;
using System.Collections.Generic;
using System.Text.Json;
using Todo.List;

namespace Todo;

public sealed partial class TodoDock : Widget
{
	internal static void Import()
	{
		if ( Instance.IsValid() is false )
			return;

		string defaultPath = Editor.FileSystem.Root.GetFullPath( "" );
		string filePath = EditorUtility.OpenFileDialog( "Import Todo Entries", "txt", defaultPath );

		if ( string.IsNullOrEmpty( filePath ) )
		{
			EditorUtility.DisplayDialog( "Invalid path", "An invalid or empty path had been provided!" );
			return;
		}

		string json = System.IO.File.ReadAllText( filePath );

		Instance.Datas = JsonSerializer.Deserialize<List<TodoEntry>>( json );

		Instance.SaveAndRefresh();
	}
}
