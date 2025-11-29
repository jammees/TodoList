using Editor;
using System.Linq;
using Todo.List;
using Todo.Widgets;

namespace Todo.Editors;

internal class TodoGroupEditor : Widget
{
	GroupControl GroupControl;

	EntryGroup Group;
	TodoWidget TodoWidget;

	public TodoGroupEditor( Widget parent, EntryGroup data, TodoWidget todoWidget ) : base( parent, true )
	{
		Group = data;
		TodoWidget = todoWidget;

		DeleteOnClose = true;
		FixedSize = new( 500f, 200f );
		WindowTitle = $"Edit {data.Group}";
		SetWindowIcon( "edit" );
		WindowFlags |= WindowFlags.Widget;

		Layout = Layout.Column();
		Layout.Margin = 4;
		Layout.Spacing = 5;

		GroupControl = new GroupControl( this, data.Group );

		Layout.AddStretchCell();

		WidgetButtonControls.AddWidgetButtonControls( this, SaveEdits, PromptDelete );
	}

	protected override void OnPaint()
	{
		Paint.ClearPen();
		Paint.SetBrush( Theme.ButtonBackground );
		Paint.DrawRect( LocalRect, 6f );
	}

	protected override void OnKeyPress( KeyEvent e )
	{
		if ( e.Key == KeyCode.Escape )
		{
			Close();
		}
	}

	private void PromptDelete()
	{
		Dialog.AskConfirm( DeleteData, "Are you sure you want to delete this group?", $"Delete {Group.Group}?", "Yes", "No" );
	}

	private void DeleteData()
	{
		TodoWidget.Datas.RemoveAll( x => x.Group == Group.Group );

		TodoWidget.SaveAndRefresh();

		Close();
	}

	private void SaveEdits()
	{
		string newGroup = GroupControl.GetGroupName();

		foreach ( TodoEntry entry in TodoWidget.Datas.Where( x => x.Group == Group.Group ) )
		{
			entry.Group = newGroup;	
		}

		TodoWidget.SaveAndRefresh();

		Close();
	}
}
