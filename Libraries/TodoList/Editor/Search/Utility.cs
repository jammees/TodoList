using System.Collections.Generic;
using System.Text;

namespace Todo.Search;

internal static class Utility
{
	internal static void GetSearchStubs( out string genericSearch, out List<string> flagStubs )
	{
		List<string> generic = new();
		List<string> flags = new();
		string[] searchStubs = TodoDock.Instance.SearchText.Split( " " );

		foreach ( var stub in searchStubs )
		{
			if ( string.IsNullOrEmpty( stub ) )
				continue;

			string normalizedStub = stub.Trim().ToLower();

			if ( stub.Contains( ':' ) is true )
			{
				flags.Add( normalizedStub );

				continue;
			}

			generic.Add( normalizedStub );
		}

		genericSearch = new StringBuilder().AppendJoin( " ", generic ).ToString();
		flagStubs = flags;
	}

	internal static bool Contains( string target, string searchText )
	{
		return target.Contains( searchText, System.StringComparison.CurrentCultureIgnoreCase );
	}

	internal static bool GetFlagArguments( List<string> flagStubs, string flagName, out List<string> flagArguments )
	{
		List<string> arguments = new();

		foreach ( var flagStub in flagStubs )
		{
			string[] splitFlag = flagStub.Split( ":" );
			if ( splitFlag.Length < 1 )
				continue;

			string flag = splitFlag[0];
			string argument = splitFlag[1];

			if ( (flag == flagName) is false )
				continue;

			arguments.Add( argument );
		}

		flagArguments = arguments;

		return flagArguments.Count > 0;
	}
}
