using Editor;
using Todo.List;
using Todo.Widgets.List.Items;

namespace Todo.Widgets.List;

internal sealed class TodoList : ListView
{
	TodoDock TodoWidget { get; set; }

	public TodoList( TodoDock widget )
	{
		ItemSize = new Vector2( -1f, 30f );
		Layout = Layout.Column();
		TodoWidget = widget;
	}

	protected override void PaintItem( VirtualWidget item )
	{
		if ( item.Object is EntryGroup group )
		{
			ItemManualGroup.OnPaint( group, item.Rect );
		}
		else if ( item.Object is TodoEntry data )
		{
			ItemManualEntry.OnPaint( data, item.Rect );
		}
		else if ( item.Object is CodeEntry code )
		{
			ItemCodeEntry.OnPaint( code, item.Rect );
		}
		else if ( item.Object is GroupsTitle title )
		{
			ItemTitle.OnPaint( title, item.Rect );
		}
		else if ( item.Object is CodeGroup codeGroup )
		{
			ItemCodeGroup.OnPaint( codeGroup, item.Rect );
		}
	}

	protected override bool OnItemPressed( VirtualWidget pressedItem, MouseEvent e )
	{
		if ( pressedItem.Object is EntryGroup group )
		{
			ItemManualGroup.OnClicked( pressedItem, e );
		}
		else if ( pressedItem.Object is TodoEntry data )
		{
			ItemManualEntry.OnClicked( pressedItem, e );
		}
		else if ( pressedItem.Object is CodeGroup codeGroup )
		{
			ItemCodeGroup.OnClicked( pressedItem, e );
		}
		else if ( pressedItem.Object is CodeEntry codeEntry )
		{
			ItemCodeEntry.OnClicked( pressedItem, e );
		}

		return false;
	}

	protected override void OnPaint()
	{
		Paint.ClearPen();
		Paint.SetBrush( Theme.ControlBackground );
		Paint.DrawRect( LocalRect, 4 );

		Paint.Antialiasing = true;
		Paint.TextAntialiasing = true;

		base.OnPaint();
	}
}
