using Editor;
using Sandbox;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Todo.Editors;
using Todo.Editors.Settings;
using Todo.List;
using Todo.Widgets;

namespace Todo;

[Dock( "Editor", "Todos", "checklist" )]
internal sealed partial class TodoDock : Widget
{
	internal TodoList List;
	internal TodoCookies Cookies;

	internal static TodoDock Instance;

	bool IsSearching => string.IsNullOrEmpty( SearchText ) is false;

	string SearchText = "";
	int VericalScrollHeight = 0;

	public TodoDock( Widget parent ) : base( parent, true )
	{
		Instance = this;

		Cookies = new();

		DeleteOnClose = true;
		MinimumSize = new( 320f, 400f );

		FocusMode = FocusMode.Click;

		Layout = Layout.Column();
		Layout.Spacing = 4f;
		Layout.Margin = 2f;

		Build();
	}

	internal void RefreshItems()
	{
		if ( List.IsValid() )
			VericalScrollHeight = List.VerticalScrollbar.Value;

		LoadItems();
	}

	internal void SaveAndRefresh()
	{
		Cookies.Save();
		RefreshItems();
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
			RefreshItems();
		};

		Layout controlLayout = Layout.Add( new Widget( this ) ).Layout = Layout.Row();
		controlLayout.Spacing = 4f;

		Button addButton = controlLayout.Add( new Button( "Add New Entry", "add", this ), 9 );
		addButton.Clicked = OpenEntryWidget;

		ToolsControls controls = controlLayout.Add( new ToolsControls( this ) );
		controls.OnRefreshClicked = RefreshItems;
		controls.OnVisibilityClicked = OpenVisibilityMenu;
		controls.OnMoreClicked = OpenMoreMenu;

		List = Layout.Add( new TodoList( this ) );

		LoadItems();
	}

	private void OpenEntryWidget()
	{
		var widget = new TodoEntryCreatorWidget( null, this );
		widget.Show();
	}

	private void OpenVisibilityMenu()
	{
		var menu = new Menu( this );

		{
			var option = menu.AddOption( new Option( this, "Show Manual Entries", "checklist" ) );
			option.Checkable = true;
			option.Checked = Cookies.ShowManualEntries;
			option.Toggled = ( b ) =>
			{
				Cookies.ShowManualEntries = b;
				SaveAndRefresh();
			};
		}

		{
			var option = menu.AddOption( new Option( this, "Show Code Entries", "code" ) );
			option.Checkable = true;
			option.Checked = Cookies.ShowCodeEntries;
			option.Toggled = ( b ) =>
			{
				Cookies.ShowCodeEntries = b;
				SaveAndRefresh();
			};
		}

		menu.DeleteOnClose = true;
		menu.OpenAtCursor( true );
	}

	private void OpenMoreMenu()
	{
		var menu = new Menu( this );

		{
			var option = menu.AddOption( new Option( this, "Import Entries", "download" ) );
			option.Triggered = () => Import();
		}

		{
			var option = menu.AddOption( new Option( this, "Export Entries", "upload" ) );
			option.Triggered = () => Export();
		}

		menu.AddSeparator();

		{
			var option = menu.AddOption( new Option( this, "Settings", "settings" ) );
			option.Triggered = () => OpenSettingsWidget();
		}

		menu.DeleteOnClose = true;
		menu.OpenAtCursor( true );
	}

	private void OpenSettingsWidget()
	{
		var widget = new SettingsWidget( null );
		widget.Show();
	}

	private void LoadItems()
	{
		List.Clear();

		HashSet<string> usedGroups = new();

		if ( Cookies.ShowManualEntries )
			LoadManualEntries( ref usedGroups );

		if ( Cookies.ShowCodeEntries )
			LoadCodeEntries( ref usedGroups );

		PurgeDeadGroups( usedGroups );
	}

	private void PurgeDeadGroups( HashSet<string> groups )
	{
		string[] deadGroups = Cookies.GroupsState.Keys.Except( groups ).ToArray();
		foreach ( string deadGroup in deadGroups )
		{
			Cookies.GroupsState.Remove( deadGroup );
		}
	}

	private void LoadManualEntries( ref HashSet<string> groups )
	{
		if ( Cookies.ShowCodeEntries )
		{
			List.AddItem( new GroupsTitle( "Manual Entries" ) );
		}

		Dictionary<string, List<TodoEntry>> grouppedEntries = new();

		foreach ( var item in Cookies.Datas )
		{
			groups.Add( item.Group );
			grouppedEntries.GetOrCreate( item.Group ).Add( item );
		}

		List<string> sortedGroups = groups.ToList();
		sortedGroups.Sort();

		foreach ( string group in sortedGroups )
		{
			if ( Cookies.GroupsState.ContainsKey( group ) is false )
			{
				Cookies.GroupsState.Add( group, true );
			}

			List.AddItem( new EntryGroup() { Group = group, Datas = grouppedEntries[group], IsOpen = Cookies.GroupsState[group] } );

			if ( Cookies.GroupsState[group] is false )
				continue;

			foreach ( var entry in grouppedEntries[group] )
			{
				if ( IsSearching && entry.Message.ToLower().Contains( SearchText ) is false )
					continue;

				List.AddItem( entry );
			}
		}
	}

	private void LoadCodeEntries( ref HashSet<string> groups )
	{
		if ( Cookies.ShowManualEntries )
		{
			List.AddItem( new GroupsTitle( "Code Entries" ) );
		}

		Dictionary<string, List<CodeEntry>> entries = ImportFromCode();

		List<string> sortedGroups = entries.Keys.ToList();
		sortedGroups.Sort();

		foreach ( var group in sortedGroups )
		{
			groups.Add( group );

			if ( Cookies.GroupsState.ContainsKey( group ) is false )
			{
				Cookies.GroupsState.Add( group, true );
			}

			List.AddItem( new CodeGroup() { Group = group, IsOpen = Cookies.GroupsState[group] } );

			if ( Cookies.GroupsState[group] is false )
				continue;

			foreach ( var entry in entries[group] )
			{
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
