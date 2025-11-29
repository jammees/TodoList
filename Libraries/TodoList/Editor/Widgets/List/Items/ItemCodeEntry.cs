using Editor;
using Sandbox;
using Todo.List;

namespace Todo.Widgets.List.Items;

internal static class ItemCodeEntry
{
	internal static void OnPaint( CodeEntry data, Rect rect )
	{
		Color color = Theme.Text;
		if ( Paint.HasMouseOver )
		{
			color = data.Style.Tint;
		}

		Paint.SetFont( Theme.HeadingFont, 9, 800 );

		Paint.ClearPen();
		Paint.SetBrush( Paint.HasMouseOver ? color.WithAlpha( 0.2f ) : Color.Transparent );
		Paint.DrawRect( rect.Shrink( 1f ), 6f );

		Rect checkboxRect = new( rect.Position.x + 5, rect.Position.y + 5f, 20f, 20f );
		Paint.SetPen( data.Style.Tint );
		Paint.DrawIcon( checkboxRect, data.Style.Icon, 16f );

		Paint.SetPen( in color );
		Paint.DrawText( rect.Shrink( 30, 8f, 8f, 8f ), data.Message, TextFlag.LeftCenter );
	}

	internal static void OnClicked( VirtualWidget pressedItem, MouseEvent e )
	{
		CodeEntry entry = (CodeEntry)pressedItem.Object;

		if ( CodeEditor.CanOpenFile( entry.SourceFile ) is false )
		{
			Log.Error( $"Failed to open file at: {entry.SourceFile}" );
			return;
		}

		Log.Info( $"Opening {entry.SourceFile} at {entry.SourceLine}" );

		CodeEditor.OpenFile( entry.SourceFile, entry.SourceLine, 0 );
	}
}
