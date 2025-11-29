using Editor;
using Sandbox;
using Todo.List;

namespace Todo.Widgets.List.Items;

internal static class ItemCodeGroup
{
	internal static void OnPaint( CodeGroup group, Rect rect )
	{
		Color color = Theme.Text;
		if ( Paint.HasPressed )
		{
			color = Theme.Yellow;
		}
		else if ( Paint.HasMouseOver )
		{
			color = Theme.Green;
		}

		Paint.SetFont( Theme.HeadingFont, 9, 800 );
		Paint.SetPen( in color );

		Rect arrowRect = new( rect.Position.x, rect.Position.y + 5f, 20f, 20f );
		Paint.DrawIcon( arrowRect, group.IsOpen ? "keyboard_arrow_up" : "keyboard_arrow_down", 20 );
		Paint.DrawText( rect.Shrink( 22f, 0f, 0f, 0f ), group.Group, TextFlag.LeftCenter );
	}

	internal static void OnClicked( VirtualWidget pressedItem, MouseEvent e )
	{
		CodeGroup group = (CodeGroup)pressedItem.Object;

		group.IsOpen = !group.IsOpen;

		TodoDock.Instance.Cookies.GroupsState[group.Group] = group.IsOpen;
		TodoDock.Instance.SaveAndRefresh();
	}
}
