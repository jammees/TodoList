using Editor;
using Sandbox;
using Todo.List;

namespace Todo.Widgets.List.Items;

internal static class ItemTitle
{
	internal static void OnPaint( GroupsTitle title, Rect rect )
	{
		Paint.ClearBrush();
		Paint.ClearPen();
		Paint.SetFont( Theme.HeadingFont, 13, 800 );
		Paint.SetPen( Theme.Text );
		Paint.DrawText( rect, title.Title, TextFlag.LeftCenter );
	}
}
