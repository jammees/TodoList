using Editor;
using Sandbox;
using System.Collections.Generic;
using System.Linq;
using Todo.CodeImport;
using Todo.Editors;
using Todo.List;
using Todo.Search;
using Todo.Widgets;
using Todo.Widgets.List;
using Todo.Widgets.List.Items;

namespace Todo;

[Dock( "Editor", "Todos", "checklist" )]
public sealed partial class TodoDock : Widget
{
	public string SearchText = "";

	public static TodoCookies Cookies;

	public static TodoDock Instance;

	public bool IsGroupUncollapsed => IsSearching && Cookies.UnCollapseGroupsOnSearch;

	public bool IsSearching => string.IsNullOrEmpty( SearchText ) is false;

	int VerticalScrollHeight = 0;
	TodoList List;

	public TodoDock( Widget parent ) : base( parent, true )
	{
		Instance = this;

		Cookies = new();

		SearchText = Cookies.LastSearch;

		DeleteOnClose = true;
		MinimumSize = new( 320f, 400f );

		FocusMode = FocusMode.Click;

		Layout = Layout.Column();
		Layout.Spacing = 4f;
		Layout.Margin = 2f;

		Build();
	}

	public void RefreshItems()
	{
		if ( List.IsValid() )
			VerticalScrollHeight = List.VerticalScrollbar.Value;

		LoadItems();
	}

	public void SaveAndRefresh()
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
		searchBar.Text = SearchText;
		searchBar.TextChanged += ( searchText ) =>
		{
			SearchText = searchText.ToLower().Trim();
			Cookies.LastSearch = SearchText;
			Cookies.Save();
			RefreshItems();
		};

		Layout controlLayout = Layout.Add( new Widget( this ) ).Layout = Layout.Row();
		controlLayout.Spacing = 4f;

		Button addButton = controlLayout.Add( new Button( "Add New Entry", "add", this ), 9 );
		addButton.Clicked = OpenEntryWidget;

		controlLayout.Add( new ToolsControls( this ) );

		List = Layout.Add( new TodoList() );

		LoadItems();
	}

	[Shortcut( "todo.new-entry", "CTRL+W", ShortcutType.Application )]
	private void OpenEntryWidget()
	{
		var widget = new TodoEntryCreatorWidget( null );
		widget.Show();
	}

	private void LoadItems()
	{
		List.Clear();

		if ( Cookies.ShowManualEntries )
		{
			if ( Cookies.ShowCodeEntries && IsSearching is false )
				List.AddItem( new ItemText( "Manual Entries", ItemText.TextType.Title ) );

			LoadManualEntries();
		}

		if ( Cookies.ShowCodeEntries )
		{
			if ( Cookies.ShowManualEntries && IsSearching is false )
				List.AddItem( new ItemText( "Code Entries", ItemText.TextType.Title ) );

			LoadCodeEntries();
		}

		if ( List.Items.Count() < 1 )
		{
			if ( IsSearching )
				List.AddItem( new ItemText( "Couldn't Find Any Entries", ItemText.TextType.Hint ) );
			else if ( (Cookies.ShowManualEntries && Cookies.ShowCodeEntries) is false )
				List.AddItem( new ItemText( "Visibility Turned Off", ItemText.TextType.Hint ) );
			else
				List.AddItem( new ItemText( "Add New Entries With \"Add New Entry\"", ItemText.TextType.Hint ) );
		}

		PurgeDeadGroups();
	}

	private void PurgeDeadGroups()
	{
		HashSet<string> manualGroups = GetAllManualGroups();
		HashSet<string> codeGroups = GetAllCodeGroups();

		List<string> deadGroups = new List<string>();
		foreach ( string group in Cookies.GroupsState.Keys )
		{
			if ( manualGroups.Contains( group ) || codeGroups.Contains( group ) )
				continue;

			deadGroups.Add( group );
		}

		foreach ( var deadGroup in deadGroups )
		{
			Cookies.GroupsState.Remove( deadGroup );
		}
	}

	private HashSet<string> GetAllManualGroups()
	{
		return Cookies.Datas.Select( data => data.Group ).ToHashSet();
	}

	private HashSet<string> GetAllCodeGroups()
	{
		return FileUtility.GetAllFiles( new() { ".cs", ".razor" } ).Select( x => x.Name ).ToHashSet();
	}

	private void LoadManualEntries()
	{
		Dictionary<string, List<TodoEntry>> grouppedEntries = new();

		foreach ( var item in Cookies.Datas )
		{
			grouppedEntries.GetOrCreate( item.Group ).Add( item );
		}

		List<string> sortedGroups = grouppedEntries.Keys.ToList();
		sortedGroups.Sort();

		foreach ( string groupName in sortedGroups )
		{
			if ( Cookies.GroupsState.ContainsKey( groupName ) is false )
			{
				Cookies.GroupsState.Add( groupName, true );
			}

			ItemGroup group = List.AddItem(
				new ItemGroup()
				{
					Name = groupName,
					ShowProgress = true
				}
			);

			if ( group.IsOpen is false )
				continue;

			int validSearches = 0;

			foreach ( var entry in grouppedEntries[groupName] )
			{
				if ( SearchEntries.FilterEntry( entry, ref validSearches ) is false )
					continue;

				List.AddItem( entry );
			}

			if ( validSearches == 0 )
			{
				List.RemoveItem( group );
			}
		}
	}

	private void LoadCodeEntries()
	{
		Dictionary<string, List<CodeEntry>> entries =
			ParseCode.ProcessFiles( FileUtility.GetAllFiles( new() { ".cs", ".razor" } ) );

		List<string> sortedGroups = entries.Keys.ToList();
		sortedGroups.Sort();

		foreach ( var groupName in sortedGroups )
		{
			if ( Cookies.GroupsState.ContainsKey( groupName ) is false )
			{
				Cookies.GroupsState.Add( groupName, true );
			}

			ItemGroup group = List.AddItem(
				new ItemGroup()
				{
					Name = groupName
				}
			);

			if ( group.IsOpen is false )
				continue;

			int validSearches = 0;

			foreach ( var entry in entries[groupName] )
			{
				if ( SearchEntries.FilterCode( entry, groupName, ref validSearches ) is false )
					continue;

				List.AddItem( entry );
			}

			if ( validSearches == 0 )
			{
				List.RemoveItem( group );
			}
		}
	}

	[EditorEvent.Frame]
	public void Frame()
	{
		if ( VerticalScrollHeight > 0 )
		{
			List.VerticalScrollbar.Value = VerticalScrollHeight;
			VerticalScrollHeight = 0;
		}
	}

	[EditorEvent.Hotload]
	public void OnHotload()
	{
		if ( Cookies.ShowCodeEntries is false )
			return;

		RefreshItems();
	}
}
