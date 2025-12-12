using Todo.List;

namespace Todo.Search;

internal static class SearchEntries
{
	internal static bool FilterEntry( TodoEntry entry, ref int validSearches )
	{
		return Filter.FilterEntry( new AbstractEntry()
		{
			Entry = entry,
			Message = entry.Message,
			Group = entry.Group,
		}, ref validSearches );
	}

	internal static bool FilterCode( CodeEntry entry, string fileName, ref int validSearches )
	{
		return Filter.FilterEntry( new AbstractEntry()
		{
			Entry = entry,
			Message = entry.Message,
			Group = fileName,
		}, ref validSearches );
	}
}
