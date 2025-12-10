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
