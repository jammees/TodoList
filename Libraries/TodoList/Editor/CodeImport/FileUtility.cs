using Sandbox;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Todo.CodeImport;

internal static class FileUtility
{
	internal static string GetRelativePath( string absolutePath )
	{
		string codeFolderPath = Project.Current.GetCodePath();
		string truncatedPath = absolutePath.Remove( 0, codeFolderPath.Length );
		truncatedPath = truncatedPath.Substring( 1, truncatedPath.Length - 1 );

		return "./" + truncatedPath;
	}

	internal static FileInfo[] GetAllFiles( string root, HashSet<string> allowedExtensions )
	{
		List<FileInfo> infos = new();

		DirectoryInfo rootDirectory = new( root );

		foreach ( FileInfo info in rootDirectory.EnumerateFiles("", SearchOption.AllDirectories) )
		{
			if ( info.Exists is false || IgnoredFolders.IsIgnored( info.FullName ) )
				continue;

			if ( allowedExtensions.Contains( info.Extension.ToLower() ) is false )
				continue;

			infos.Add( info );
		}

		return infos.ToArray();
	}

	internal static string GetFileContents( string path )
	{
		string[] stubLines = System.IO.File.ReadAllLines( path );

		StringBuilder builder = new();

		foreach ( var line in stubLines )
		{
			builder.AppendLine( line );
		}

		return builder.ToString();
	}
}
