using Editor;

namespace Todo.Editors.Settings;

internal class SettingsWidget : Widget
{
	TodoDock TodoWidget;

	public SettingsWidget( Widget parent, TodoDock todoWidget ) : base( parent )
	{
		TodoWidget = todoWidget;

		DeleteOnClose = true;
		MinimumSize = new( 500f, 400f );
		WindowTitle = $"Settings";
		SetWindowIcon( "settings" );

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
		Paint.SetBrush( Theme.SurfaceBackground );
		Paint.DrawRect( LocalRect, 6f );
	}

	private void Build()
	{
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

		Button addStyleButton = codeContainer.Add( new Button( "Add New Code Word", "add", this ) );
		addStyleButton.Clicked = OpenStyleCreatorWidget;

		codeContainer.Add( new Separator( 8f ) );

		foreach ( TodoCodeStyle style in TodoWidget.CodeStyles )
		{
			codeContainer.Add( new StyleWidget( this, style ) );
			codeContainer.Add( new Separator( 5 ) );
			codeContainer.Add( new Separator( 2f ) ).Color = Theme.SurfaceLightBackground;
			codeContainer.Add( new Separator( 5 ) );
		}

		canvas.AddStretchCell();

		{
			Layout layout = Layout.Add( Layout.Row() );
			layout.Spacing = 5f;

			layout.Add( new Button.Primary( "Save", "save", this ) );
			layout.Add( new Button( "Cancel", "cancel", this ) );
		}
	}

	private void OpenStyleCreatorWidget()
	{
		Log.Info( "h" );
	}

	private void AddTitle( Layout canvas, string title )
	{
		Label label = new Label( title, this );
		label.SetStyles( "font-size: 20px; font-weight: 400;" );
		canvas.Add( label );
	}
}
