using Editor;
using Sandbox;

namespace Todo.Editors.Settings;

internal sealed class CodeWordWidget : Widget
{
	TodoCodeWord CodeWord;

	SettingsWidget SettingsWidget;

	public CodeWordWidget( SettingsWidget settingsWidget, TodoCodeWord style ) : base( settingsWidget, false )
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
		Dialog.AskConfirm(Delete, $"Are you sure you want to delete {CodeWord.CodeWord}?", "Delete Code Word?");
	}

	private void Delete()
	{
		TodoDock.Instance.CodeWords.Remove( CodeWord );
		TodoDock.Instance.SaveAndRefresh();
		SettingsWidget.Build();
	}
}
