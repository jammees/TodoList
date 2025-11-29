using Editor;
using Sandbox;
using System.IO;
using System.Text.Json;

namespace Todo;

public sealed partial class TodoWidget : Widget
{
	internal static void Export()
	{
		if ( Instance.IsValid() is false )
			return;

		string defaultPath = Editor.FileSystem.Root.GetFullPath( "" );
		string filePath = EditorUtility.SaveFileDialog( "Export Todo Entries", "txt", defaultPath + "\\TodoEntries.txt" );

		if ( string.IsNullOrEmpty( filePath ) )
		{
			EditorUtility.DisplayDialog( "Invalid path", "An invalid or empty path had been provided!" );
			return;
		}

		string json = JsonSerializer.Serialize( Instance.Datas );

		StreamWriter file = System.IO.File.CreateText( filePath );
		file.Write( json );
		file.Close();
	}
}
