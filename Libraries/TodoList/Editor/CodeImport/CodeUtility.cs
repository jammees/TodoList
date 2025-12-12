using static Todo.CodeImport.ParseCode;

namespace Todo.CodeImport;

public static class CodeUtility
{
	// dear god
	public static int GetSourceLine( string desiredStub, CommentMatch[] matches )
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
