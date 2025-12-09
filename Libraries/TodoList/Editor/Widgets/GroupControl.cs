using Editor;
using Editor.NodeEditor;
using System.Linq;

namespace Todo.Widgets;

internal sealed class GroupControl
{
	LineEdit GroupEdit;
	WarningBox WarningBox;

	string CurrentGroup = "";

	public GroupControl( Widget parent, string currentGroup )
	{
		CurrentGroup = currentGroup;

		WarningBox = parent.Layout.Add( new WarningBox( "Invalid Group! Reason:", parent ) );
		WarningBox.BackgroundColor = Theme.Red;
		WarningBox.Visible = false;

		parent.Layout.Add( new Label( "Group", parent ) );
		GroupEdit = parent.Layout.Add( new LineEdit( parent ) );
		GroupEdit.Text = CurrentGroup;
		GroupEdit.TextChanged += OnTextEdited;
		GroupEdit.PlaceholderText = "Group";

		Button groupEditButton = parent.Layout.Add( new Button( $"Change to Existing Group", "folder_copy", parent ) );
		groupEditButton.Clicked = BuildGroupOptions;
	}

	public string GetGroupName()
	{
		return string.IsNullOrEmpty( GroupEdit.Text ) ? "Default" : GroupEdit.Text;
	}

	private void OnTextEdited( string newString )
	{
		bool isValid = true;

		if ( string.IsNullOrEmpty( newString.Trim() ) is true )
		{
			isValid = false;
			SetWarningMessage( "Empty group name!" );
		}

		if ( (newString.EndsWith( ".cs" ) || newString.EndsWith( ".razor" )) is true )
		{
			isValid = false;
			SetWarningMessage( "Group ends with .cs or .razor extension, will break " +
				"groups if a file is named the same!" );
		}

		WarningBox.Visible = isValid is false;
	}

	private void SetWarningMessage( string reason )
	{
		WarningBox.Label.Text = $"Invalid Group! Reason: {reason}";
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

		foreach ( var item in TodoDock.Instance.Cookies.GroupsState.Keys )
		{
			if ( item == GroupEdit.Text || (isSearching && item.Contains( searchText ) is false) )
				continue;

			if ( TodoDock.Instance.Cookies.ShowCodeEntries && (item.EndsWith( ".cs" ) || item.EndsWith( ".razor" )) )
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
