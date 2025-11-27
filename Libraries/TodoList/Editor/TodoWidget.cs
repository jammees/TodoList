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
	LineEdit SearchBar;

	internal Dictionary<string, bool> GroupsState;
	internal List<TodoEntry> Datas;
	internal static TodoWidget Instance;

	bool IsSearching => string.IsNullOrEmpty( SearchText ) is false;

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

		Layout = Layout.Column();
		Layout.Spacing = 4f;
		Layout.Margin = 2f;

		Build();
	}

	internal void TriggerSave( bool doRefresh = true )
	{
		ProjectCookie.Set( $"{SettingCookie}.List", Datas );
		ProjectCookie.Set( $"{SettingCookie}.Groups", GroupsState );

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

		SearchBar = Layout.Add( new LineEdit( this ) );
		SearchBar.PlaceholderText = "Search...";
		SearchBar.MinimumHeight = 30f;
		SearchBar.TextChanged += OnSearchChanged;

		Layout controlLayout = Layout.Add( new Widget( this ) ).Layout = Layout.Row();
		controlLayout.Spacing = 4f;

		Button addButton = controlLayout.Add( new Button( "Add new entry", this ) );
		addButton.Clicked = OpenEntryWidget;

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

	private void OnSearchChanged( string search )
	{
		SearchText = search.ToLower();
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
