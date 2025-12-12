using System.Collections.Generic;
using System.Linq;
using Todo.List;

namespace Todo.Widgets.List.Items;

public struct ItemGroup
{
	public string Name { get; set; }

	public bool ShowProgress { get; set; }

	public bool CanBeEdited { get; set; }

	public bool IsOpen
	{
		get
		{
			if ( TodoDock.Instance.IsGroupUncollapsed )
				return true;

			TodoDock.Cookies.GroupsState.TryGetValue( Name, out var groupsState );
			return groupsState;
		}
		set
		{
			if ( TodoDock.Instance.IsGroupUncollapsed )
				return;

			TodoDock.Cookies.GroupsState[Name] = value;
			TodoDock.Instance.SaveAndRefresh();
		}
	}

	public int CompletedEntries
	{
		get
		{
			if ( ShowProgress is false )
				return 0;

			string groupName = Name;

			IEnumerable<ManualEntry> entries =
				TodoDock.Cookies.Datas.Where( x => x.Group == groupName );

			return entries.Count( x => x.IsDone );
		}
	}

	public int AllEntries
	{
		get
		{
			if ( ShowProgress is false )
				return 0;

			string groupName = Name;

			IEnumerable<ManualEntry> entries =
				TodoDock.Cookies.Datas.Where( x => x.Group == groupName );

			return entries.Count();
		}
	}

	public void Toggle()
	{
		IsOpen = !IsOpen;
	}
}
