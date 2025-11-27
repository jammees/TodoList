using Editor;
using Sandbox;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Todo.Editors;
using Todo.List;
using Todo.Widgets;

namespace Todo;

[Dock( "Editor", "Todos", "checklist" )]
public sealed partial class TodoWidget : Widget
{
	TodoList List;

	internal Dictionary<string, bool> GroupsState;
	internal List<TodoEntry> Datas;
	internal static TodoWidget Instance;

	bool IsSearching => string.IsNullOrEmpty( SearchText ) is false;

	bool ShowManualEntries = true;
	bool ShowCodeEntries = false;
	string SettingCookie;
	string SearchText = "";
	int VericalScrollHeight = 0;

	public TodoWidget( Widget parent ) : base( parent, true )
	{
		Instance = this;

		DeleteOnClose = true;
		MinimumSize = new( 320f, 400f );

		FocusMode = FocusMode.Click;

		SettingCookie = GetCookie();

		Datas = ProjectCookie.Get( $"{SettingCookie}.List", new List<TodoEntry>() );
		GroupsState = ProjectCookie.Get( $"{SettingCookie}.Groups", new Dictionary<string, bool>() );
		ShowManualEntries = ProjectCookie.Get( $"{SettingCookie}.ShowManual", true );
		ShowCodeEntries = ProjectCookie.Get( $"{SettingCookie}.ShowCode", false );

		Layout = Layout.Column();
		Layout.Spacing = 4f;
		Layout.Margin = 2f;

		Build();
	}

	internal void TriggerSave( bool doRefresh = true )
	{
		ProjectCookie.Set( $"{SettingCookie}.List", Datas );
		ProjectCookie.Set( $"{SettingCookie}.Groups", GroupsState );
		ProjectCookie.Set( $"{SettingCookie}.ShowManual", ShowManualEntries );
		ProjectCookie.Set( $"{SettingCookie}.ShowCode", ShowCodeEntries );

		if ( doRefresh is true )
			Refresh();
	}

	internal void Refresh()
	{
		if ( List.IsValid() )
			VericalScrollHeight = List.VerticalScrollbar.Value;

		LoadItems();
	}

	internal void SetGroupState( string group, bool state )
	{
		GroupsState[group] = state;
		TriggerSave();
	}

	internal void DeleteData( TodoEntry data )
	{
		Datas.Remove( data );
		TriggerSave();
	}

	private void Build()
	{
		Layout.Clear( true );

		LineEdit searchBar = Layout.Add( new LineEdit( this ) );
		searchBar.PlaceholderText = "Search...";
		searchBar.MinimumHeight = 30f;
		searchBar.TextChanged += ( searchText ) =>
		{
			SearchText = searchText.ToLower().Trim();
			Refresh();
		};

		Widget controlWidget = Layout.Add( new Widget( this ) );
		Layout controlLayout = controlWidget.Layout = Layout.Row();
		controlLayout.Spacing = 4f;

		Button addButton = controlLayout.Add( new Button( "Add New Entry", "add", this ) );
		addButton.Clicked = OpenEntryWidget;

		Splitter splitter = new( controlWidget );
		splitter.IsHorizontal = true;
		splitter.AddWidget( addButton );
		splitter.SetStretch( 0, 9 );
		controlLayout.Add( splitter, 1 );

		Widget buttonWidget = controlLayout.Add( new Widget( this ) );
		buttonWidget.OnPaintOverride = () =>
		{
			Paint.ClearPen();
			Paint.SetBrush( Theme.ControlBackground );
			Paint.DrawRect( buttonWidget.LocalRect, 4 );

			return true;
		};

		Layout buttonLayout = buttonWidget.Layout = Layout.Row();
		buttonLayout.Spacing = 2f;

		ToolButton refreshButton = buttonLayout.Add( new ToolButton( "", "refresh", buttonWidget ) );
		refreshButton.MouseClick = Refresh;
		refreshButton.ToolTip = "Refresh All";

		ToolButton showVisibilityButton = buttonLayout.Add( new ToolButton( "", "visibility", buttonWidget ) );
		showVisibilityButton.MouseClick = OpenVisibilityMenu;
		showVisibilityButton.ToolTip = "Change Visibility";

		List = Layout.Add( new TodoList( this ) );

		LoadItems();
	}

	private string GetCookie()
	{
		StringBuilder cookie = new StringBuilder( "Jammees.TodoList" );

		return cookie.ToString();
	}

	private void OpenEntryWidget()
	{
		var windowExample = new TodoEntryCreatorWidget( null, this );
		windowExample.Show();
	}

	private void OpenVisibilityMenu()
	{
		var menu = new Menu( this );

		{
			var option = menu.AddOption( new Option( this, "Show Manual Entries", "checklist" ) );
			option.Checkable = true;
			option.Checked = ShowManualEntries;
			option.Toggled = ( b ) =>
			{
				ShowManualEntries = b;
				TriggerSave( false );
			};
		}

		{
			var option = menu.AddOption( new Option( this, "Show Code Entries", "code" ) );
			option.Checkable = true;
			option.Checked = ShowCodeEntries;
			option.Toggled = ( b ) =>
			{
				ShowCodeEntries = b;
				TriggerSave( false );
			};
		}

		menu.DeleteOnClose = true;
		menu.OpenAtCursor( true );
	}

	private void LoadItems()
	{
		List.Clear();

		HashSet<string> groups = new();
		Dictionary<string, List<TodoEntry>> entries = new();

		foreach ( var item in Datas )
		{
			groups.Add( item.Group );
			entries.GetOrCreate( item.Group ).Add( item );
		}

		string[] deadGroups = GroupsState.Keys.Except( groups ).ToArray();
		foreach ( string deadGroup in deadGroups )
		{
			GroupsState.Remove( deadGroup );
		}

		List<string> sortedGroups = groups.ToList();
		sortedGroups.Sort();

		foreach ( string group in sortedGroups )
		{
			if ( GroupsState.ContainsKey( group ) is false )
			{
				GroupsState.Add( group, true );
			}

			List.AddItem( new EntryGroup() { Group = group, Datas = entries[group], IsOpen = GroupsState[group] } );

			if ( GroupsState[group] is false )
				continue;

			foreach ( var entry in entries[group] )
			{
				if ( IsSearching && entry.Message.ToLower().Contains( SearchText ) is false )
					continue;

				List.AddItem( entry );
			}
		}
	}

	[EditorEvent.Frame]
	public void Frame()
	{
		if ( VericalScrollHeight > 0 )
		{
			List.VerticalScrollbar.Value = VericalScrollHeight;
			VericalScrollHeight = 0;
		}
	}
}
