using System.Collections.Generic;
using System.IO;
using static Todo.CodeImport.ParseCode;

namespace Todo.CodeImport;

internal static class CodeUtility
{
	// dear god
	internal static int GetSourceLine( string desiredStub, CommentMatch[] matches )
	{
		foreach ( CommentMatch match in matches )
		{
			if ( string.IsNullOrEmpty( match.CommentStub ) )
				continue;

			if ( desiredStub.Contains( match.CommentStub ) is false )
				continue;

			return match.Line;
		}

		Log.Error( $"Failed to find line for: {desiredStub}!" );

		return 0;
	}

	internal static int GetSourceLine( int targetIndex, List<int> lineLenghts )
	{
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
	internal static List<int> GetLineOffsets( FileInfo file )
	{
		List<int> lineLengths = new();

		using StreamReader reader = file.OpenText();

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
}
