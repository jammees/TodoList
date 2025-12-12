using Editor;
using Todo.List;
using Todo.Widgets.List.ItemControllers;
using Todo.Widgets.List.Items;
using static Sandbox.Services.Inventory;

namespace Todo.Widgets.List;

public sealed class TodoList : ListView
{
	public TodoList()
	{
		ItemSize = new Vector2( -1f, 30f );
		Layout = Layout.Column();
	}

	protected override void PaintItem( VirtualWidget item )
	{
		if ( item.Object is ItemGroup group )
		{
			ItemGroupController.OnPaint( group, item.Rect );
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
	}

	protected override bool OnItemPressed( VirtualWidget pressedItem, MouseEvent @event )
	{
		if ( pressedItem.Object is ItemGroup group )
		{
			group.Toggle();
		}
		else if ( pressedItem.Object is TodoEntry data )
		{
			ItemManualEntry.OnClicked( data, @event );
		}
		else if ( pressedItem.Object is CodeEntry codeEntry )
		{
			ItemCodeEntry.OnClicked( codeEntry, @event );
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
