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
	}

	private string GetCookie()
	{
		StringBuilder cookie = new StringBuilder( "Jammees.TodoList" );

		return cookie.ToString();
	}
}
