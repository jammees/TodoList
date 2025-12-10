using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Todo.CodeImport;

internal static class CodeUtility
{
	internal static int GetSourceLine( string source, string stubTarget, MatchCollection comments, List<int> lineLenghts )
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
	internal static List<int> GetLineLengths( FileInfo file )
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
