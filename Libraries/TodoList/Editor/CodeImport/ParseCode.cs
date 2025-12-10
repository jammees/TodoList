using Sandbox;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Todo.List;

namespace Todo.CodeImport;

internal static class ParseCode
{
	internal static Dictionary<string, List<CodeEntry>> ProcessFiles( FileInfo[] files )
	{
		Dictionary<string, List<CodeEntry>> results = new();

		foreach ( FileInfo file in files )
		{
			List<int> lineOffsets = CodeUtility.GetLineOffsets( file );

			string sourceText = FileUtility.GetFileContents( file );
			string lines = GetComments( file, out List<Match> lineMatches );

			foreach ( TodoCodeWord style in TodoDock.Instance.Cookies.CodeWords )
			{
				string[] entries = ScanFor( lines, style.CodeWord, out MatchCollection stubEntries );

				for ( int i = 0; i < entries.Length; i++ )
				{
					results.GetOrCreate( file.Name ).Add( new()
					{
						SourceFile = FileUtility.GetRelativePath( file.FullName ),
						Message = entries[i],
						SourceLine = CodeUtility.GetSourceLine( sourceText, stubEntries[i].Value, lineMatches, lineOffsets ),
						Style = style
					} );
				}
			}

			foreach ( var item in results.Values )
			{
				item.Sort( ( x, y ) => x.SourceLine.CompareTo( y.SourceLine ) );
			}
		}

		return results;
	}

	private static string GetComments( FileInfo file, out List<Match> matches )
	{
		using StreamReader reader = file.OpenText();

		List<Match> foundMatches = new();
		StringBuilder commentString = new();
		bool lastLineWasComment = false;

		while ( reader.EndOfStream is false )
		{
			string line = reader.ReadLine();

			if ( line.Contains( "//" ) is false )
			{
				if ( lastLineWasComment )
				{
					lastLineWasComment = false;
					commentString.AppendLine( "" );
				}
				continue;
			}

			lastLineWasComment = true;

			Regex commentRegex = new( "(?<=\\/\\/).*$", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace );
			Match commentMatch = commentRegex.Match( line );

			if ( commentMatch.Success is false )
			{
				lastLineWasComment = false;
				continue;
			}

			foundMatches.Add( commentMatch );

			commentString.AppendLine( commentMatch.Value.Trim() );
		}

		matches = foundMatches;

		Log.Info(commentString.ToString());

		return commentString.ToString();
	}

	private static string GetRegexTerminator()
	{
		StringBuilder builder = new();
		int stylesCount = TodoDock.Instance.Cookies.CodeWords.Count;

		for ( int i = 0; i < stylesCount; i++ )
		{
			TodoCodeWord style = TodoDock.Instance.Cookies.CodeWords[i];

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
