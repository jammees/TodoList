using Editor;
using Sandbox;
using System.Collections.Generic;
using System.Linq;
using Todo.List;
using Todo.Editors;

namespace Todo.Widgets;

internal sealed class TodoList : ListView
{
	TodoWidget TodoWidget { get; set; }

	public TodoList( TodoWidget widget )
	{
		ItemSize = new Vector2( -1f, 30f );
		Layout = Layout.Column();
		TodoWidget = widget;
	}

	protected override void PaintItem( VirtualWidget item )
	{
		if ( item.Object is EntryGroup group )
		{
			PaintGroup( group, item.Rect );
		}
		else if ( item.Object is TodoEntry data )
		{
			PaintData( data, item.Rect );
		}
		else if ( item.Object is CodeEntry code )
		{
			PaintCode( code, item.Rect );
		}
		else if ( item.Object is GroupsTitle title )
		{
			PaintTitle( title, item.Rect );
		}
		else if ( item.Object is CodeGroup codeGroup )
		{
			PaintCodeGroup( codeGroup, item.Rect );
		}
	}

	protected override bool OnItemPressed( VirtualWidget pressedItem, MouseEvent e )
	{
		if ( pressedItem.Object is EntryGroup group )
		{
			OnGroupClicked( pressedItem, e );
		}
		else if ( pressedItem.Object is TodoEntry data )
		{
			OnEntryClicked( pressedItem, e );
		}
		else if ( pressedItem.Object is CodeGroup codeGroup )
		{
			OnCodeGroupClicked( pressedItem, e );
		}
		else if ( pressedItem.Object is CodeEntry codeEntry )
		{
			OnCodeEntryClicked( pressedItem, e );
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

	private void PaintCodeGroup( CodeGroup group, Rect rect )
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

	private void PaintTitle( GroupsTitle title, Rect rect )
	{
		Paint.ClearBrush();
		Paint.ClearPen();
		Paint.SetFont( Theme.HeadingFont, 13, 800 );
		Paint.SetPen( Theme.Text );
		Paint.DrawText( rect, title.Title, TextFlag.LeftCenter );
	}

	private void PaintGroup( EntryGroup group, Rect rect )
	{
		List<TodoEntry> groupEntries = group.Datas;
		int completedEntries = groupEntries.Where( x => x.IsDone ).Count();

		Color color = Theme.Text;
		if ( Paint.HasPressed )
		{
			color = Theme.Yellow;
		}
		else if ( Paint.HasMouseOver )
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
		Paint.DrawIcon( arrowRect, group.IsOpen ? "keyboard_arrow_up" : "keyboard_arrow_down", 20 );
		Paint.DrawText( rect.Shrink( 22f, 0f, 0f, 0f ), groupName, TextFlag.LeftCenter );
	}

	private void PaintCode( CodeEntry data, Rect rect )
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

	private void PaintData( TodoEntry data, Rect rect )
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

	private void OnCodeGroupClicked( VirtualWidget pressedItem, MouseEvent e )
	{
		CodeGroup group = (CodeGroup)pressedItem.Object;

		group.IsOpen = !group.IsOpen;

		TodoWidget.SetGroupState( group.Group, group.IsOpen );
	}

	private void OnGroupClicked( VirtualWidget pressedItem, MouseEvent e )
	{
		EntryGroup group = (EntryGroup)pressedItem.Object;

		if ( e.HasShift )
		{
			OpenGroupEditor( group );
			return;
		}

		group.IsOpen = !group.IsOpen;

		TodoWidget.SetGroupState( group.Group, group.IsOpen );
	}

	private void OnCodeEntryClicked( VirtualWidget pressedItem, MouseEvent e )
	{
		CodeEntry entry = (CodeEntry)pressedItem.Object;

		if ( CodeEditor.CanOpenFile( entry.SourceFile ) is false )
		{
			Log.Error($"Failed to open file at: {entry.SourceFile}");
			return;
		}

		Log.Info( $"Opening {entry.SourceFile} at {entry.SourceLine}" );

		CodeEditor.OpenFile( entry.SourceFile, entry.SourceLine, 0 );
	}

	private void OnEntryClicked( VirtualWidget pressedItem, MouseEvent e )
	{
		TodoEntry entry = (TodoEntry)pressedItem.Object;

		if ( e.HasShift )
		{
			OpenEntryEditor( entry );
			return;
		}

		entry.IsDone = !entry.IsDone;

		TodoWidget.SaveAndRefresh();
	}

	private void OpenGroupEditor( EntryGroup group )
	{
		var windowExample = new TodoGroupEditor( null, group, TodoWidget );
		windowExample.Show();
	}

	private void OpenEntryEditor( TodoEntry data )
	{
		var windowExample = new TodoEntryEditor( null, data, TodoWidget );
		windowExample.Show();
	}
}
