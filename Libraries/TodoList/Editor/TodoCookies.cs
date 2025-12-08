using System.Collections.Generic;
using System.Text;
using Todo.List;

namespace Todo;

internal sealed class TodoCookies
{
	internal List<TodoEntry> Datas;
	internal List<TodoCodeWord> CodeWords;

	internal Dictionary<string, bool> GroupsState;

	internal bool ShowManualEntries;
	internal bool ShowCodeEntries;
	internal bool ReloadOnHotload;
	internal bool WidgetsOnTop;
	internal bool CollapseGroupsOnSearch;
	internal bool EnableKeybinds;
	internal bool ShowDefaultGroup;
	internal string DefaultGroupName;

	string SettingCookie;

	public TodoCookies()
	{
		SettingCookie = GetCookie();

		Load();
	}

	internal void Save()
	{
		ProjectCookie.Set( $"{SettingCookie}.List", Datas );
		ProjectCookie.Set( $"{SettingCookie}.Groups", GroupsState );
		ProjectCookie.Set( $"{SettingCookie}.ShowManual", ShowManualEntries );
		ProjectCookie.Set( $"{SettingCookie}.ShowCode", ShowCodeEntries );
		ProjectCookie.Set( $"{SettingCookie}.CodeWords", CodeWords );
		ProjectCookie.Set( $"{SettingCookie}.ReloadOnHotload", ReloadOnHotload );
		ProjectCookie.Set( $"{SettingCookie}.WidgetsOnTop", WidgetsOnTop );
		ProjectCookie.Set( $"{SettingCookie}.CollapseGroupsOnSearch", CollapseGroupsOnSearch );
		ProjectCookie.Set( $"{SettingCookie}.EnableKeybinds", EnableKeybinds );
		ProjectCookie.Set( $"{SettingCookie}.ShowDefaultGroup", ShowDefaultGroup );
		ProjectCookie.Set( $"{SettingCookie}.DefaultGroupName", DefaultGroupName );
	}

	private void Load()
	{
		Datas = ProjectCookie.Get( $"{SettingCookie}.List", new List<TodoEntry>() );
		GroupsState = ProjectCookie.Get( $"{SettingCookie}.Groups", new Dictionary<string, bool>() );
		ShowManualEntries = ProjectCookie.Get( $"{SettingCookie}.ShowManual", true );
		ShowCodeEntries = ProjectCookie.Get( $"{SettingCookie}.ShowCode", false );
		CodeWords = ProjectCookie.Get( $"{SettingCookie}.CodeWords", TodoCodeWord.GetDefault() );
		ReloadOnHotload = ProjectCookie.Get( $"{SettingCookie}.ReloadOnHotload", true );
		WidgetsOnTop = ProjectCookie.Get( $"{SettingCookie}.WidgetsOnTop", false );
		CollapseGroupsOnSearch = ProjectCookie.Get( $"{SettingCookie}.CollapseGroupsOnSearch", true );
		EnableKeybinds = ProjectCookie.Get( $"{SettingCookie}.EnableKeybinds", true );
		ShowDefaultGroup = ProjectCookie.Get( $"{SettingCookie}.ShowDefaultGroup", false );
		DefaultGroupName = ProjectCookie.Get( $"{SettingCookie}.DefaultGroupName", "Default" );
	}

	private string GetCookie()
	{
		StringBuilder cookie = new StringBuilder( "Jammees.TodoList" );

		return cookie.ToString();
	}
}
