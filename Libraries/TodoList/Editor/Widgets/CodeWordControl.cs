using Editor;
using Sandbox;
using Todo.CodeImport;
using Todo.Editors;

namespace Todo.Widgets;

public sealed class CodeWordControl : Widget
{
	TodoCodeWord CodeWord;

	SettingsWidget SettingsWidget;

	public CodeWordControl( SettingsWidget settingsWidget, TodoCodeWord style ) : base( settingsWidget, false )
	{
		SettingsWidget = settingsWidget;

		CodeWord = style;

		MinimumHeight = Theme.RowHeight;
		Layout = Layout.Row();

		Build();
	}

	private void Build()
	{
		{
			ControlSheet sheet = new ControlSheet();
			sheet.Spacing = 1f;
			foreach ( var entry in CodeWord.GetSerialized() )
			{
				sheet.AddRow( entry );
			}
			Layout.Add( sheet );
		}

		Button deleteButton = Layout.Add( new Button.Danger( "Delete", "delete" ) );
		deleteButton.Clicked = PromptDelete;
	}

	private void PromptDelete()
	{
		Dialog.AskConfirm(Delete, $"Are you sure you want to delete {CodeWord.CodeWord}?", "Delete Code Word?", "Yes", "No" );
	}

	private void Delete()
	{
		TodoDock.Cookies.CodeWords.Remove( CodeWord );
		TodoDock.Instance.SaveAndRefresh();
		SettingsWidget.Build();
	}
}
