using Editor;
using Sandbox;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Todo;

public sealed partial class TodoWidget : Widget
{
	private static readonly string[] IgnoredFolders = {
		"obj",
		"properties"
	};


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
			//lines = lines.CollapseSpacesAndPreserveLines();
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

		Instance.SaveAndRefresh();
	}

	private static string GetComments( string lines )
	{
		Regex commentRegex = new( "(?<=\\/\\/).*", RegexOptions.Multiline | RegexOptions.IgnoreCase );
		MatchCollection results = commentRegex.Matches( lines );

		StringBuilder commentString = new();

		foreach ( Match match in results )
		{
			commentString.AppendLine( match.Value.Trim() );
		}

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
