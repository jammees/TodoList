using Todo.List;

namespace Todo.Search;

public static class SearchEntries
{
	public static bool FilterEntry( ManualEntry entry, ref int validSearches )
	{
		return Filter.FilterEntry( new AbstractEntry()
		{
			Entry = entry,
			Message = entry.Message,
			Group = entry.Group,
		}, ref validSearches );
	}

	public static bool FilterCode( CodeEntry entry, string fileName, ref int validSearches )
	{
		return Filter.FilterEntry( new AbstractEntry()
		{
			Entry = entry,
			Message = entry.Message,
			Group = fileName,
		}, ref validSearches );
	}
}
