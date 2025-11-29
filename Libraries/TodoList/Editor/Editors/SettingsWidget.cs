using Editor;
using Todo.Widgets;

namespace Todo.Editors;

internal class SettingsWidget : Widget
{
	public SettingsWidget( Widget parent ) : base( parent, true )
	{
		WidgetUtility.SetProperties(
			this,
			400f,
			"Settings",
			"settings"
		);

		Layout = Layout.Column();
		Layout.Margin = 4f;
		Layout.Spacing = 5f;

		Build();
	}

	protected override void OnKeyPress( KeyEvent e )
	{
		if ( e.Key == KeyCode.Escape )
		{
			Close();
		}
	}

	protected override void OnPaint()
	{
		Paint.ClearPen();
		Paint.SetBrush( Theme.ButtonBackground );
		Paint.DrawRect( LocalRect, 6f );
	}

	protected override bool OnClose()
	{
		TodoDock.Instance.SaveAndRefresh();

		return base.OnClose();
	}

	internal void Build()
	{
		Layout.Clear( true );

		ScrollArea scroll = Layout.Add( new ScrollArea( this ) );
		scroll.Canvas = new Widget( this );
		Layout canvas = scroll.Canvas.Layout = Layout.Column();
		canvas.Spacing = 10f;
		canvas.Margin = new Sandbox.UI.Margin( 0f, 0f, 20f, 0f );

		AddTitle( canvas, "Miscellaneous" );

		canvas.Add( new Checkbox( "Refresh on Hotload", this ) );
		canvas.Add( new Checkbox( "Widgets Stay on Top", this ) );

		canvas.Add( new Separator( 2f ) ).Color = Theme.SurfaceLightBackground;

		AddTitle( canvas, "Code Words" );

		Layout codeContainer = canvas.Add( new Widget( this ) ).Layout = Layout.Column();

		Layout buttonLayout = codeContainer.Add( Layout.Row() );
		buttonLayout.Spacing = 4f;

		Button addStyleButton = buttonLayout.Add( new Button( "Add New Code Word", "add", this ), 2 );
		addStyleButton.Clicked = OpenStyleCreatorWidget;

		Button resetButton = buttonLayout.Add( new Button.Danger( "Reset All", "refresh", this ) );
		resetButton.Clicked = PromptReset;

		codeContainer.Add( new Separator( 8f ) );

		foreach ( TodoCodeWord style in TodoDock.Instance.Cookies.CodeWords )
		{
			codeContainer.Add( new CodeWordControl( this, style ) );
			codeContainer.Add( new Separator( 5 ) );
			codeContainer.Add( new Separator( 2f ) ).Color = Theme.SurfaceLightBackground;
			codeContainer.Add( new Separator( 5 ) );
		}

		canvas.AddStretchCell();
	}

	private void PromptReset()
	{
		Dialog.AskConfirm(
			ResetAll,
			"Are you sure you want to reset all code words to the " +
			"default ones?",
			"Reset All Code Words?",
			"Yes",
			"No"
		);
	}

	private void ResetAll()
	{
		TodoDock.Instance.Cookies.CodeWords = TodoCodeWord.GetDefault();
		TodoDock.Instance.SaveAndRefresh();
		Build();
	}

	private void OpenStyleCreatorWidget()
	{
		var widget = new CodeWordCreatorWidget( null, this );
		widget.Show();
	}

	private void AddTitle( Layout canvas, string title )
	{
		Label label = new Label( title, this );
		label.SetStyles( "font-size: 20px; font-weight: 400;" );
		canvas.Add( label );
	}
}
