using Sandbox;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Todo.List;

namespace Todo.CodeImport;

internal static class ParseCode
{
	internal struct CommentMatch
	{
		public string CommentStub;
		public int Line;
	}

	internal static Dictionary<string, List<CodeEntry>> ProcessFiles( FileInfo[] files )
	{
		Dictionary<string, List<CodeEntry>> results = new();

		foreach ( FileInfo file in files )
		{
			string sourceText = FileUtility.GetFileContents( file );
			string lines = GetComments( file, out CommentMatch[] lineMatches );

			foreach ( TodoCodeWord style in TodoDock.Instance.Cookies.CodeWords )
			{
				string[] entries = ScanFor( lines, style.CodeWord );

				for ( int i = 0; i < entries.Length; i++ )
				{
					results.GetOrCreate( file.Name ).Add( new()
					{
						SourceFile = FileUtility.GetRelativePath( file.FullName ),
						Message = entries[i],
						SourceLine = CodeUtility.GetSourceLine( entries[i], lineMatches ),
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

	private static string GetComments( FileInfo file, out CommentMatch[] lineMatches )
	{
		StringBuilder commentString = new();

		bool lastWasComment = false;

		using StreamReader reader = file.OpenText();

		List<CommentMatch> matches = new();
		string lineContent = "";
		int lineIndex = 0;

		// all of this just to add empty lines
		// between comments that are not next to
		// each other
		while ( reader.EndOfStream is false )
		{
			lineContent = reader.ReadLine();
			lineIndex += 1;

			if ( lineContent.Contains( "//" ) is false )
			{
				if ( lastWasComment )
				{
					lastWasComment = false;
					commentString.AppendLine( "" );
				}
				continue;
			}

			lastWasComment = true;

			Regex commentRegex = new( "(?<=\\/\\/).*$", RegexOptions.Singleline | RegexOptions.IgnoreCase );
			Match commentMatch = commentRegex.Match( lineContent );

			if ( commentMatch.Success is false )
			{
				lastWasComment = false;
				continue;
			}

			Regex stubSearchRegex = new( $"(?<={GetRegexTerminator()}).*", RegexOptions.Singleline | RegexOptions.IgnoreCase );

			matches.Add( new()
			{
				Line = lineIndex,
				CommentStub = stubSearchRegex.Match( lineContent ).Value.Trim()
			} );

			commentString.AppendLine( commentMatch.Value.Trim() );
		}

		lineMatches = matches.ToArray();

		return commentString.ToString();
	}

	private static string GetRegexTerminator()
	{
		StringBuilder builder = new();

		string[] codeWords = TodoDock.Instance.Cookies.CodeWords.Select( x => x.CodeWord ).ToArray();
		builder.AppendJoin( "|", codeWords );

		return builder.ToString();
	}

	private static string[] ScanFor( string lines, string searchString )
	{
		Regex searchRegex = new( $"(?<=^{searchString}).*(?:\\n(?!\\s*$|{GetRegexTerminator()}).*)*", RegexOptions.Multiline | RegexOptions.IgnoreCase );
		MatchCollection results = searchRegex.Matches( lines );

		return results.Select( x => x.Value.CollapseWhiteSpace() ).ToArray();
	}
}
