using Editor;
using Sandbox;
using Todo.Widgets;

namespace Todo.Editors;

internal class CodeWordCreatorWidget : Widget
{
	TodoCodeWord CodeWord;

	SettingsWidget SettingsWidget;

	public CodeWordCreatorWidget( Widget parent, SettingsWidget settingsWidget )
		: base( parent, true )
	{
		SettingsWidget = settingsWidget;

		WidgetUtility.SetProperties(
			this,
			200f,
			"Create New Code Word",
			"edit"
		);

		Layout = Layout.Column();
		Layout.Margin = 4;
		Layout.Spacing = 5;

		CodeWord = new()
		{
			CodeWord = "example:",
			Icon = "tag_faces",
			Tint = Theme.Yellow
		};

		Layout.Add( new WarningBox( "It is recommended to make code words end with a \":\"" +
			"to keep in style with the rest of them and to avoid false positives", this ) );

		ControlSheet sheet = new ControlSheet();
		foreach ( var prop in CodeWord.GetSerialized() )
		{
			sheet.AddRow( prop );
		}
		Layout.Add( sheet );

		Layout.AddStretchCell();

		WidgetButtonControls.AddWidgetButtonControls( this, CreateCodeWord );
	}

	protected override void OnPaint()
	{
		Paint.ClearPen();
		Paint.SetBrush( Theme.ButtonBackground );
		Paint.DrawRect( LocalRect, 6f );
	}

	protected override void OnKeyPress( KeyEvent e )
	{
		if ( e.Key == KeyCode.Escape )
		{
			Close();
		}
	}

	private void CreateCodeWord()
	{
		TodoDock.Instance.Cookies.CodeWords.Add( CodeWord );
		TodoDock.Instance.SaveAndRefresh();

		SettingsWidget.Build();

		Close();
	}
}
