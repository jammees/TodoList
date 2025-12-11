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
			if ( match.MatchSuccess is false )
				continue;

			if ( desiredStub.Contains( match.CommentStub ) is false )
				continue;

			return match.Line;
		}

		Log.Error( $"Failed to find line for: {desiredStub}!" );

		return 0;
	}
}
