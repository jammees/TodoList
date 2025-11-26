using Editor;
using Sandbox;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Todo.List;
using static Sandbox.Resources.ResourceGenerator;

namespace Todo;

public sealed partial class TodoWidget : Widget
{
	private static readonly string[] IgnoredFolders = {
		"obj",
		"properties"
	};

	[Menu( "Editor", "Todo List/Import Entries" )]
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

		Instance.TriggerSave();
	}

	[Menu( "Editor", "Todo List/Import from Code" )]
	internal static void ImportFromCodePrompt()
	{
		Dialog.AskConfirm( ImportFromCode, "This will overwrite all of the entries!", "Are you sure you want to import from code?" );
	}

	internal static void ImportFromCode()
	{
		if ( Instance.IsValid() is false )
			return;

		string codeFolderPath = Project.Current.GetCodePath();

		EnumerationOptions options = new()
		{
			RecurseSubdirectories = true,
			IgnoreInaccessible = true,
		};

		Instance.Datas.Clear();

		ProcessFiles( codeFolderPath, "*.cs", options );
		ProcessFiles( codeFolderPath, "*.razor", options );
	}

	private static void ProcessFiles( string root, string extension, EnumerationOptions options )
	{
		string[] paths = System.IO.Directory.GetFiles( root, extension, options );

		foreach ( string path in paths )
		{
			if ( IsInIgnoredFolder( path ) )
				continue;

			string fileName = new FileInfo( path ).Name;

			string lines = System.IO.File.ReadAllText( path );
			lines = lines.CollapseSpacesAndPreserveLines();
			lines = GetComments( lines );

			foreach ( var message in ScanFor( lines, "TODO:" ) )
			{
				Instance.Datas.Add( new()
				{
					Group = fileName,
					Message = message
				} );
			}
		}

		Instance.TriggerSave();
	}

	private static string GetComments( string lines )
	{
		Regex commentRegex = new( "(?<=^\\/\\/).*", RegexOptions.Multiline | RegexOptions.IgnoreCase );
		MatchCollection results = commentRegex.Matches( lines );

		StringBuilder commentString = new();

		foreach ( Match match in results )
		{
			commentString.AppendLine( match.Value.Trim() );
		};

		return commentString.ToString();
	}

	private static string[] ScanFor( string lines, string searchString )
	{
		Regex searchRegex = new( $"(?<=^{searchString}).*(?:\\n(?!\\s*$|{searchString}).*)*", RegexOptions.Multiline | RegexOptions.IgnoreCase );
		MatchCollection results = searchRegex.Matches( lines );

		string[] formattedResults = new string[results.Count];

		for ( int i = 0; i < formattedResults.Length; i++ )
		{
			formattedResults[i] = results[i].Value.CollapseWhiteSpace();
		}

		return formattedResults;
	}

	private static bool IsInIgnoredFolder( string path )
	{
		foreach ( var ignoredPath in IgnoredFolders )
		{
			if ( path.ToLower().Contains( ignoredPath.ToLower() ) is false )
				continue;

			return true;
		}

		return false;
	}
}
