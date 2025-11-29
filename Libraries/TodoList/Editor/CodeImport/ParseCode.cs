using Sandbox;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Todo.List;

namespace Todo.CodeImport;

internal static class ParseCode
{
	internal static void ProcessFiles( string root, string extension, EnumerationOptions options, ref Dictionary<string, List<CodeEntry>> results )
	{
		string[] paths = System.IO.Directory.GetFiles( root, extension, options );

		foreach ( string path in paths )
		{
			if ( IgnoredFolders.IsIgnored( path ) )
				continue;

			string fileName = new FileInfo( path ).Name;

			List<int> lineLenghts = CodeUtility.GetLineLengths( path );

			string sourceText = FileUtility.GetFileContents( path );
			string lines = GetComments( sourceText, out MatchCollection lineMatches );

			foreach ( TodoCodeStyle style in TodoDock.Instance.CodeStyles )
			{
				string[] entries = ScanFor( lines, style.CodeWord, out MatchCollection stubEntries );

				for ( int i = 0; i < entries.Length; i++ )
				{
					results.GetOrCreate( fileName ).Add( new()
					{
						SourceFile = FileUtility.GetRelativePath( path ),
						Message = entries[i],
						SourceLine = CodeUtility.GetSourceLine( sourceText, stubEntries[i].Value, lineMatches, lineLenghts ),
						Style = style
					} );
				}
			}

			foreach ( var item in results.Values )
			{
				item.Sort( ( x, y ) => x.SourceLine.CompareTo( y.SourceLine ) );
			}
		}
	}

	private static string GetComments( string lines, out MatchCollection matches )
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

	private static string GetRegexTerminator()
	{
		StringBuilder builder = new();
		int stylesCount = TodoDock.Instance.CodeStyles.Count;

		for ( int i = 0; i < stylesCount; i++ )
		{
			TodoCodeStyle style = TodoDock.Instance.CodeStyles[i];

			builder.Append( style.CodeWord );
			if ( i + 1 < stylesCount )
				builder.Append("|");
		}

		return builder.ToString();
	}

	private static string[] ScanFor( string lines, string searchString, out MatchCollection stubResults )
	{
		Regex searchRegex = new( $"(?<=^{searchString}).*(?:\\n(?!\\s*$|{GetRegexTerminator()}).*)*", RegexOptions.Multiline | RegexOptions.IgnoreCase );
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
}
