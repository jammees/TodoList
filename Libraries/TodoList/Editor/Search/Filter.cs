using Sandbox;
using System.Collections.Generic;

namespace Todo.Search;

internal static class Filter
{
	internal static bool FilterEntry( AbstractEntry entry, ref int validSearches )
	{
		if ( TodoDock.Instance.IsSearching is false )
		{
			validSearches++;
			return true;
		}

		Utility.GetSearchStubs( out string genericSearch, out List<string> flagStubs );

		{
			bool isSuccess = Utility.GetFlagArguments( flagStubs, "in", out List<string> flagArguments );
			foreach ( var argument in flagArguments )
			{
				if ( Utility.Contains( entry.Group, argument ) is true )
					continue;

				return false;
			}
		}

		{
			bool isSuccess = Utility.GetFlagArguments( flagStubs, "not", out List<string> flagArguments );
			foreach ( var argument in flagArguments )
			{
				if ( Utility.Contains( entry.Group, argument ) is false )
					continue;

				return false;
			}
		}

		{
			bool isSuccess = Utility.GetFlagArguments( flagStubs, "done", out List<string> flagArguments );
			foreach ( var argument in flagArguments )
			{
				if ( entry.IsCode is true )
					return false;

				if ( entry.TodoEntry.IsDone is true )
					continue;

				return false;
			}
		}

		{
			bool isSuccess = Utility.GetFlagArguments( flagStubs, "pending", out List<string> flagArguments );
			foreach ( var argument in flagArguments )
			{
				if ( entry.IsCode is true )
					return false;

				if ( entry.TodoEntry.IsDone is false )
					continue;

				return false;
			}
		}

		{
			foreach ( var codeWord in TodoDock.Instance.Cookies.CodeWords )
			{
				string word = codeWord.CodeWord.Replace( ":", "" );

				bool isSuccess = Utility.GetFlagArguments( flagStubs, word, out List<string> flagArguments );
				foreach ( var argument in flagArguments )
				{
					if ( entry.IsCode is false )
						return false;

					if ( entry.CodeEntry.Style.GetHashCode() == codeWord.GetHashCode() )
						continue;

					return false;
				}
			}
		}

		{
			if ( Utility.Contains( entry.Message, genericSearch ) is false )
			{
				return false;
			}
		}

		validSearches++;

		return true;
	}
}
