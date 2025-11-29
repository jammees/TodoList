namespace Todo.CodeImport;

internal static class IgnoredFolders
{
	private static readonly string[] Folders = {
		"obj",
		"properties"
	};

	internal static bool IsIgnored( string path )
	{
		foreach ( var ignoredPath in Folders )
		{
			if ( path.ToLower().Contains( ignoredPath.ToLower() ) is false )
				continue;

			return true;
		}

		return false;
	}
}
