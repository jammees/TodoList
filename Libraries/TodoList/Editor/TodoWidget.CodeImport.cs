using Editor;
using Sandbox;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Todo.List;

namespace Todo;

public sealed partial class TodoWidget : Widget
{
	private readonly string[] IgnoredFolders = {
		"obj",
		"properties"
	};

	private Dictionary<string, List<CodeEntry>> ImportFromCode()
	{
		if ( Instance.IsValid() is false )
			return default;

		string codeFolderPath = Project.Current.GetCodePath();

		EnumerationOptions options = new()
		{
			RecurseSubdirectories = true,
			IgnoreInaccessible = true,
		};

		Dictionary<string, List<CodeEntry>> results = new();

		ProcessFiles( codeFolderPath, "*.cs", options, ref results );
		ProcessFiles( codeFolderPath, "*.razor", options, ref results );

		return results;
	}

	private void ProcessFiles( string root, string extension, EnumerationOptions options, ref Dictionary<string, List<CodeEntry>> results )
	{
		string[] paths = System.IO.Directory.GetFiles( root, extension, options );

		foreach ( string path in paths )
		{
			if ( IsInIgnoredFolder( path ) )
				continue;

			string fileName = new FileInfo( path ).Name;

			List<int> lineLenghts = GetLineLengths( path );

			// the story behind why I read all lines
			// just to combine them is because for some reason
			// regex is unable to find the comments if the file is using
			// CR but LF and CRLF are fine... what?
			string[] stubLines = System.IO.File.ReadAllLines(path);
			string sourceText = CombineLineStubs( stubLines );

			string lines = GetComments( sourceText, out MatchCollection lineMatches );

			string[] entries = ScanFor( lines, "TODO:", out MatchCollection stubEntries );

			for ( int i = 0; i < entries.Length; i++ )
			{
				results.GetOrCreate( fileName ).Add( new()
				{
					SourceFile = GetRelativePath( path ),
					Message = entries[i],
					SourceLine = GetSourceLine( sourceText, stubEntries[i].Value, lineMatches, lineLenghts )
				} );
			}
		}
	}

	private string CombineLineStubs( string[] stubs )
	{
		StringBuilder builder = new();

		foreach ( var line in stubs )
		{
			builder.AppendLine( line );
		}

		return builder.ToString();
	}

	private string GetRelativePath( string absolutePath )
	{
		string codeFolderPath = Project.Current.GetCodePath();
		string truncatedPath = absolutePath.Remove( 0, codeFolderPath.Length );
		truncatedPath = truncatedPath.Substring( 1, truncatedPath.Length - 1 );

		return "./" + truncatedPath;
	}

	private int GetSourceLine( string source, string stubTarget, MatchCollection comments, List<int> lineLenghts )
	{
		int targetIndex = comments.First( x => x.Value.Trim().Contains( stubTarget.Trim() ) ).Index;

		for ( int i = 0; i < lineLenghts.Count; i++ )
		{
			int lastLenght = i > 0 ? lineLenghts[i - 1] : 0;
			int currentLenght = lineLenghts[i];

			if ( (lastLenght < targetIndex && targetIndex < currentLenght) is false )
				continue;

			return i + 1;
		}

		return 0;
	}

	// oh my god
	// I'm not sure which is worse, this, or my regex that I don't even
	// remember what it does anymore
	private List<int> GetLineLengths( string path )
	{
		List<int> lineLengths = new();

		using FileStream file = System.IO.File.OpenRead( path );
		using StreamReader reader = new( file );

		int globalSum = 0;
		while ( reader.EndOfStream is false )
		{
			int character = reader.Read();

			if ( character == '\n' )
			{
				globalSum += 1;
				lineLengths.Add( globalSum );
			}
			else if ( character == '\r' )
			{
				if ( reader.Peek() == '\n' )
				{
					reader.Read();
					globalSum += 2;
					lineLengths.Add( globalSum );
				}
				else
				{
					globalSum += 1;
					lineLengths.Add( globalSum );
				}
			}
			else
			{
				globalSum += 1;
			}
		}

		return lineLengths;
	}

	private string GetComments( string lines, out MatchCollection matches )
	{
		Regex commentRegex = new( "(?<=\\/\\/).*$", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace );
		MatchCollection results = commentRegex.Matches( lines );

		StringBuilder commentString = new();

		foreach ( Match match in results )
		{
			commentString.AppendLine( match.Value.Trim() );
		}

		matches = results;

		return commentString.ToString();
	}

	private string[] ScanFor( string lines, string searchString, out MatchCollection stubResults )
	{
		Regex searchRegex = new( $"(?<=^{searchString}).*(?:\\n(?!\\s*$|{searchString}).*)*", RegexOptions.Multiline | RegexOptions.IgnoreCase );
		MatchCollection results = searchRegex.Matches( lines );

		Regex stubSearchRegex = new( $"(?<=^{searchString}).*", RegexOptions.Multiline | RegexOptions.IgnoreCase );
		stubResults = stubSearchRegex.Matches( lines );

		string[] formattedResults = new string[results.Count];

		for ( int i = 0; i < formattedResults.Length; i++ )
		{
			formattedResults[i] = results[i].Value.CollapseWhiteSpace();
		}

		return formattedResults;
	}

	private bool IsInIgnoredFolder( string path )
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
