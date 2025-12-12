using Editor;
using System.Linq;
using Todo.List;
using Todo.Widgets;
using Todo.Widgets.List.Items;

namespace Todo.Dialogs;

public sealed class TodoGroupEditor : Dialog
{
	GroupControl GroupControl;

	ItemGroup Group;

	public TodoGroupEditor( Widget parent, ItemGroup group ) : base( parent, false )
	{
		Group = group;

		WidgetUtility.SetProperties(
			this,
			200f,
			$"Edit {group.Name}",
			"edit"
		);

		Layout = Layout.Column();
		Layout.Margin = 4;
		Layout.Spacing = 5;

		GroupControl = new GroupControl( this, group.Name );

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
		Dialog.AskConfirm( DeleteData, "Are you sure you want to delete this group?", $"Delete {Group.Name}?", "Yes", "No" );
	}

	private void DeleteData()
	{
		TodoDock.Cookies.Datas.RemoveAll( x => x.Group == Group.Name );

		TodoDock.Instance.SaveAndRefresh();

		Close();
	}

	private void SaveEdits()
	{
		string newGroup = GroupControl.GetGroupName();

		foreach ( TodoEntry entry in TodoDock.Cookies.Datas.Where( x => x.Group == Group.Name ) )
		{
			entry.Group = newGroup;
		}

		TodoDock.Cookies.GroupsState[newGroup] = Group.IsOpen;

		TodoDock.Instance.SaveAndRefresh();

		Close();
	}
}
