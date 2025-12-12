using Editor;
using Sandbox;
using System.Collections.Generic;
using System.Text.Json;
using Todo.List;

namespace Todo;

internal sealed partial class TodoDock : Widget
{
	internal void Import()
	{
		string defaultPath = Editor.FileSystem.Root.GetFullPath( "" );
		string filePath = EditorUtility.OpenFileDialog( "Import Todo Entries", "txt", defaultPath );

		if ( string.IsNullOrEmpty( filePath ) )
		{
			Log.Error( "An invalid or empty path had been provided!" );
			return;
		}

		string json = System.IO.File.ReadAllText( filePath );

		Cookies.Datas = JsonSerializer.Deserialize<List<TodoEntry>>( json );

		Instance.SaveAndRefresh();
	}
}
