using Editor;
using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Editors;
using Todo.List;

namespace Todo.Widgets.List.ItemControllers;

internal static class ItemManualGroup
{
	internal static void OnPaint( EntryGroup group, Rect rect )
	{
		List<TodoEntry> groupEntries = group.Datas;
		int completedEntries = groupEntries.Where( x => x.IsDone ).Count();

		Color color = Theme.Text;
		if ( Paint.HasMouseOver )
		{
			color = Theme.Green;
		}
		else if ( completedEntries == groupEntries.Count )
		{
			color = Theme.Text.WithAlpha( 0.5f );
		}

		string groupName = $"{group.Group} ({completedEntries}/{groupEntries.Count})";

		Paint.SetFont( Theme.HeadingFont, 9, 800 );
		Paint.SetPen( in color );

		Rect arrowRect = new( rect.Position.x, rect.Position.y + 5f, 20f, 20f );

		if ( TodoDock.Instance.IsGroupUncollapsed )
		{
			Paint.DrawIcon( arrowRect, "horizontal_rule", 20 );
		}
		else
		{
			Paint.DrawIcon( arrowRect, group.IsOpen ? "keyboard_arrow_up" : "keyboard_arrow_down", 20 );
		}

		Paint.DrawText( rect.Shrink( 22f, 0f, 0f, 0f ), groupName, TextFlag.LeftCenter );
	}

	internal static void OnClicked( EntryGroup group, MouseEvent e )
	{
		if ( e.HasShift )
		{
			OpenGroupEditor( group );
			return;
		}

		if ( TodoDock.Instance.IsGroupUncollapsed )
			return;

		group.IsOpen = !group.IsOpen;

		TodoDock.Cookies.GroupsState[group.Group] = group.IsOpen;
		TodoDock.Instance.SaveAndRefresh();
	}

	private static void OpenGroupEditor( EntryGroup group )
	{
		var windowExample = new TodoGroupEditor( null, group );
		windowExample.Show();
	}
}
