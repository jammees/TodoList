using Editor;
using Sandbox;
using Todo.Widgets.List.Items;

namespace Todo.Widgets.List.ItemControllers;

public static class ItemGroupController
{
	public static void OnPaint( ItemGroup group, Rect rect )
	{
		Color color = Theme.Text;
		if ( Paint.HasMouseOver )
		{
			color = Theme.Green;
		}
		else if ( group.ShowProgress && group.CompletedEntries == group.AllEntries )
		{
			color = Theme.Text.WithAlpha( 0.5f );
		}

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

		string displayText = group.Name;
		if ( group.ShowProgress is true )
		{
			displayText += $" ({group.CompletedEntries}/{group.AllEntries})";
		}

		Paint.DrawText( rect.Shrink( 22f, 0f, 0f, 0f ), displayText, TextFlag.LeftCenter );
	}
}
