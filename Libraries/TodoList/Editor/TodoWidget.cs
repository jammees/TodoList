using Editor;
using Sandbox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Todo.Editors;
using Todo.List;
using Todo.Widgets;

namespace Todo;

[Dock( "Editor", "Todos", "checklist" )]
public sealed class TodoWidget : Widget
{
	TodoList List;
	LineEdit SearchBar;

	internal Dictionary<string, bool> GroupsState;
	internal List<TodoEntry> Datas;
	internal static TodoWidget Instance;

	bool IsSearching => string.IsNullOrEmpty( SearchText ) is false;

	string SettingCookie;
	string SearchText = "";
	int SaveCounter = 0;
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

	[Menu( "Editor", "Todo List/Import Entries" )]
	internal static void Import()
	{
		if ( Instance.IsValid() is false)
			return;

		string defaultPath = Editor.FileSystem.Root.GetFullPath( "" );
		string filePath = EditorUtility.OpenFileDialog( "Import Todo Entries", "txt", defaultPath );

		if ( string.IsNullOrEmpty( filePath ) )
		{
			EditorUtility.DisplayDialog( "Invalid path", "An invalid or empty path had been provided!" );
			return;
		}

		string json = System.IO.File.ReadAllText( filePath );

		Instance.Datas = JsonSerializer.Deserialize<List<TodoEntry>>( json );

		Instance.TriggerSave();
	}

	[Menu( "Editor", "Todo List/Export Entries" )]
	internal static void Export()
	{
		if ( Instance.IsValid() is false )
			return;

		string defaultPath = Editor.FileSystem.Root.GetFullPath( "" );
		string filePath = EditorUtility.SaveFileDialog( "Export Todo Entries", "txt", defaultPath + "\\TodoEntries.txt" );

		if ( string.IsNullOrEmpty( filePath ) )
		{
			EditorUtility.DisplayDialog( "Invalid path", "An invalid or empty path had been provided!" );
			return;
		}

		string json = JsonSerializer.Serialize( Instance.Datas );

		StreamWriter file = System.IO.File.CreateText( filePath );
		file.Write( json );
		file.Close();
	}

	internal void TriggerSave()
	{
		ProjectCookie.Set( $"{SettingCookie}.List", Datas );
		ProjectCookie.Set( $"{SettingCookie}.Groups", GroupsState );

		SaveCounter++;
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

		if ( SetContentHash( ContentHash, 0.1f ) )
		{
			if ( List.IsValid() )
				VericalScrollHeight = List.VerticalScrollbar.Value;

			LoadItems();
		}
	}

	private int ContentHash() => HashCode.Combine( SaveCounter, SearchText );
}
