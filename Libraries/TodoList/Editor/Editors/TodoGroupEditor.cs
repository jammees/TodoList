using Editor;
using System.Linq;
using Todo.List;
using Todo.Widgets;

namespace Todo.Editors;

public sealed class TodoGroupEditor : Widget
{
	GroupControl GroupControl;

	EntryGroup Group;

	public TodoGroupEditor( Widget parent, EntryGroup data ) : base( parent, true )
	{
		Group = data;

		WidgetUtility.SetProperties(
			this,
			200f,
			$"Edit {data.Group}",
			"edit"
		);

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
		TodoDock.Cookies.Datas.RemoveAll( x => x.Group == Group.Group );

		TodoDock.Instance.SaveAndRefresh();

		Close();
	}

	private void SaveEdits()
	{
		string newGroup = GroupControl.GetGroupName();

		foreach ( TodoEntry entry in TodoDock.Cookies.Datas.Where( x => x.Group == Group.Group ) )
		{
			entry.Group = newGroup;	
		}

		TodoDock.Instance.SaveAndRefresh();

		Close();
	}
}
