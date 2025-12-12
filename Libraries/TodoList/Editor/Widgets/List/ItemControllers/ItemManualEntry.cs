using Editor;
using Sandbox;
using Todo.Dialogs;
using Todo.List;

namespace Todo.Widgets.List.ItemControllers;

public static class ItemManualEntry
{
	public static void OnPaint( TodoEntry data, Rect rect )
	{
		Color color = Theme.Text;
		if ( Paint.HasMouseOver )
		{
			color = Theme.Green;
		}
		else if ( data.IsDone )
		{
			color = Theme.Text.WithAlpha( 0.3f );
		}

		Paint.ClearPen();
		Paint.SetBrush( Paint.HasMouseOver ? color.WithAlpha( 0.2f ) : Color.Transparent );
		Paint.DrawRect( rect.Shrink( 1f ), 6f );

		Rect checkboxRect = new( rect.Position.x + 5, rect.Position.y + 5f, 20f, 20f );
		Paint.SetBrush( Theme.SelectedBackground );
		Paint.DrawRect( checkboxRect, 4f );
		Paint.SetBrush( Theme.ControlBackground );
		Paint.DrawRect( checkboxRect.Shrink( 1f ), 4f );
		if ( data.IsDone )
		{
			Paint.SetPen( Theme.Green );
			Paint.DrawIcon( checkboxRect, "check", 16f );
		}

		Paint.SetFont( Theme.HeadingFont, 8, 500, data.IsDone );
		Paint.SetPen( in color );
		Paint.DrawText( rect.Shrink( 30, 8f, 8f, 8f ), data.Message, TextFlag.LeftCenter );
	}

	public static void OnClicked( TodoEntry entry, MouseEvent e )
	{
		if ( e.HasShift )
		{
			OpenEntryEditor( entry );
			return;
		}

		entry.IsDone = !entry.IsDone;

		TodoDock.Instance.SaveAndRefresh();
	}

	private static void OpenEntryEditor( TodoEntry data )
	{
		var windowExample = new TodoEntryEditor( null, data );
		windowExample.Show();
	}
}
