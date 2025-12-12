using Editor;
using System.Collections.Generic;
using System.Text.Json;
using Todo.List;

namespace Todo;

public sealed partial class TodoDock : Widget
{
	public void Import()
	{
		string defaultPath = Editor.FileSystem.Root.GetFullPath( "" );
		string filePath = EditorUtility.OpenFileDialog( "Import Todo Entries", "txt", defaultPath );

		if ( string.IsNullOrWhiteSpace( filePath ) )
		{
			Log.Error( "An invalid or empty path had been provided!" );
			return;
		}

		string json = System.IO.File.ReadAllText( filePath );

		Cookies.Datas = JsonSerializer.Deserialize<List<ManualEntry>>( json );

		Instance.SaveAndRefresh();
	}
}
