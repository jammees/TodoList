using Editor;
using Sandbox;
using Todo.Widgets.List.Items;

namespace Todo.Widgets.List.ItemControllers;

public static class ItemTextController
{
	public static void OnPaint( ItemText title, Rect rect )
	{
		Color color = Theme.Text;
		TextFlag flag = TextFlag.LeftCenter;

		if ( title.Type == ItemText.TextType.Hint )
		{
			color = Theme.Text.WithAlpha( 0.5f );
			flag = TextFlag.Center;
		}

		Paint.ClearBrush();
		Paint.SetPen( color );

		switch ( title.Type )
		{
			case ItemText.TextType.Hint:
				Paint.SetFont( Theme.DefaultFont, 9, 300 );
				break;
			case ItemText.TextType.Title:
				Paint.SetFont( Theme.HeadingFont, 13, 800 );
				break;
		}

		
		Paint.DrawText( rect, title.Text, flag );
	}
}
