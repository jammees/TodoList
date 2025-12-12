using Editor;
using Editor.NodeEditor;
using System.Linq;

namespace Todo.Widgets;

public sealed class GroupControl
{
	LineEdit GroupEdit;

	string CurrentGroup = "";

	public GroupControl( Widget parent, string currentGroup )
	{
		CurrentGroup = currentGroup;

		parent.Layout.Add( new Label( "Group", parent ) );
		GroupEdit = parent.Layout.Add( new LineEdit( parent ) );
		GroupEdit.Text = CurrentGroup;
		GroupEdit.EditingFinished += OnEditingFinished;
		GroupEdit.PlaceholderText = "Group";

		parent.Layout.Add( new GroupWarningBox( parent, GroupEdit ) );

		Button groupEditButton = parent.Layout.Add( new Button( $"Change to Existing Group", "folder_copy", parent ) );
		groupEditButton.Clicked = BuildGroupOptions;
	}

	public string GetGroupName()
	{
		return string.IsNullOrEmpty( GroupEdit.Text ) ? "Default" : GroupEdit.Text;
	}

	private void OnEditingFinished()
	{
		GroupEdit.Text = GroupEdit.Text.Trim();
	}

	private void BuildGroupOptions()
	{
		Menu popupMenu = new();

		popupMenu.AddLineEdit(
			"Search",
			placeholder: "for Group...",
			autoFocus: true,
			onChange: searchText => PopulateGroupMenu( ref popupMenu, searchText.ToLower() )
		);

		popupMenu.AboutToShow += () => PopulateGroupMenu( ref popupMenu );
		popupMenu.DeleteOnClose = true;
		popupMenu.OpenAtCursor( true );
		popupMenu.MinimumWidth = TodoDock.Instance.ScreenRect.Width;
	}

	private void PopulateGroupMenu( ref Menu menu, string searchText = "" )
	{
		ClearMenu( ref menu );

		bool isSearching = string.IsNullOrEmpty( searchText ) is false;

		menu.AddHeading( $"Current Group: {CurrentGroup}" );

		foreach ( var item in TodoDock.Cookies.GroupsState.Keys )
		{
			if ( item == GroupEdit.Text || (isSearching && item.Contains( searchText ) is false) )
				continue;

			if ( TodoDock.Cookies.ShowCodeEntries && (item.EndsWith( ".cs" ) || item.EndsWith( ".razor" )) )
				continue;

			menu.AddOption( $"{item}", action: () => GroupEdit.Text = item );
		}
	}

	private void ClearMenu( ref Menu menu )
	{
		menu.RemoveOptions();
		menu.RemoveMenus();

		foreach ( var item in menu.Widgets.Skip( 1 ) )
		{
			menu.RemoveWidget( item );
		}
	}
}
