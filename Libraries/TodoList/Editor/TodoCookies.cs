using System.Collections.Generic;
using System.Text;
using Todo.CodeImport;
using Todo.List;

namespace Todo;

public sealed class TodoCookies
{
	public List<TodoEntry> Datas;
	public List<TodoCodeWord> CodeWords;

	public Dictionary<string, bool> GroupsState;

	public bool ShowManualEntries;
	public bool ShowCodeEntries;
	public bool ReloadOnHotload;
	public bool UnCollapseGroupsOnSearch;
	public string DefaultGroupName;
	public string LastSearch;

	string SettingCookie;

	public TodoCookies()
	{
		SettingCookie = GetCookie();

		Load();
	}

	public void Save()
	{
		ProjectCookie.Set( $"{SettingCookie}.List", Datas );
		ProjectCookie.Set( $"{SettingCookie}.Groups", GroupsState );
		ProjectCookie.Set( $"{SettingCookie}.ShowManual", ShowManualEntries );
		ProjectCookie.Set( $"{SettingCookie}.ShowCode", ShowCodeEntries );
		ProjectCookie.Set( $"{SettingCookie}.CodeWords", CodeWords );
		ProjectCookie.Set( $"{SettingCookie}.ReloadOnHotload", ReloadOnHotload );
		ProjectCookie.Set( $"{SettingCookie}.CollapseGroupsOnSearch", UnCollapseGroupsOnSearch );
		ProjectCookie.Set( $"{SettingCookie}.DefaultGroupName", DefaultGroupName );
		ProjectCookie.Set( $"{SettingCookie}.LastSearch", LastSearch );
	}

	private void Load()
	{
		Datas = ProjectCookie.Get( $"{SettingCookie}.List", new List<TodoEntry>() );
		GroupsState = ProjectCookie.Get( $"{SettingCookie}.Groups", new Dictionary<string, bool>() );
		ShowManualEntries = ProjectCookie.Get( $"{SettingCookie}.ShowManual", true );
		ShowCodeEntries = ProjectCookie.Get( $"{SettingCookie}.ShowCode", false );
		CodeWords = ProjectCookie.Get( $"{SettingCookie}.CodeWords", TodoCodeWord.GetDefault() );
		ReloadOnHotload = ProjectCookie.Get( $"{SettingCookie}.ReloadOnHotload", true );
		UnCollapseGroupsOnSearch = ProjectCookie.Get( $"{SettingCookie}.CollapseGroupsOnSearch", true );
		DefaultGroupName = ProjectCookie.Get( $"{SettingCookie}.DefaultGroupName", "Default" );
		LastSearch = ProjectCookie.Get( $"{SettingCookie}.LastSearch", "" );
	}

	private string GetCookie()
	{
		StringBuilder cookie = new StringBuilder( "Jammees.TodoList" );

		return cookie.ToString();
	}
}
