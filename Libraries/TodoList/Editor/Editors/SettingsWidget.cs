using Editor;
using Sandbox;
using Todo.CodeImport;
using Todo.Widgets;

namespace Todo.Editors;

public sealed class SettingsWidget : Dialog
{
	ScrollArea Scroll { get; set; }
	int VerticalScrollHeight { get; set; }

	public SettingsWidget( Widget parent ) : base( parent, false )
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

	public void Build()
	{
		if ( Scroll.IsValid() )
		{
			VerticalScrollHeight = Scroll.VerticalScrollbar.Value;
		}

		Layout.Clear( true );

		Scroll = Layout.Add( new ScrollArea( this ) );
		Scroll.Canvas = new Widget( this );
		Layout canvas = Scroll.Canvas.Layout = Layout.Column();
		canvas.Spacing = 10f;
		canvas.Margin = new Sandbox.UI.Margin( 5f, 5f, 20f, 5f );

		AddTitle( canvas, "Miscellaneous", false );

		{
			Checkbox checkbox = canvas.Add( new Checkbox( "Refresh on Hotload", this ) );
			checkbox.State = TodoDock.Cookies.ReloadOnHotload ? CheckState.On : CheckState.Off;
			checkbox.StateChanged = state =>
			{
				TodoDock.Cookies.ReloadOnHotload = state == CheckState.On ? true : false;
				TodoDock.Cookies.Save();
			};
		}

		AddTitle( canvas, "Groups", true );

		{
			Checkbox checkbox = canvas.Add( new Checkbox( "Uncollapse on Search", this ) );
			checkbox.State = TodoDock.Cookies.UnCollapseGroupsOnSearch ? CheckState.On : CheckState.Off;
			checkbox.StateChanged = state =>
			{
				TodoDock.Cookies.UnCollapseGroupsOnSearch = state == CheckState.On ? true : false;
				TodoDock.Cookies.Save();
			};
		}

		{
			Layout layout = canvas.Add( Layout.Row() );
			layout.Spacing = 10f;

			layout.Add( new Label( "Default Group Name", this ) );

			LineEdit group = layout.Add( new LineEdit( TodoDock.Cookies.DefaultGroupName, this ) );
			group.PlaceholderText = "Insert Group Name";
			group.EditingFinished += () =>
			{
				TodoDock.Cookies.DefaultGroupName = group.Text;
				TodoDock.Cookies.Save();
			};

			canvas.Add( new GroupWarningBox( this, group ) );
		}

		AddTitle( canvas, "Code Words", true );

		canvas.Add( new WarningBox( "It is recommended to make code words end with a \":\"" +
			"to keep in style with the rest of them and to avoid false positives", this ) );

		Layout codeContainer = canvas.Add( new Widget( this ) ).Layout = Layout.Column();

		Layout buttonLayout = codeContainer.Add( Layout.Row() );
		buttonLayout.Spacing = 4f;

		Button addStyleButton = buttonLayout.Add( new Button( "Add New Code Word", "add", this ), 2 );
		addStyleButton.Clicked = OpenStyleCreatorWidget;

		Button resetButton = buttonLayout.Add( new Button.Danger( "Reset All", "refresh", this ) );
		resetButton.Clicked = PromptReset;

		codeContainer.Add( new Separator( 8f ) );

		foreach ( TodoCodeWord style in TodoDock.Cookies.CodeWords )
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
		TodoDock.Cookies.CodeWords = TodoCodeWord.GetDefault();
		TodoDock.Instance.SaveAndRefresh();
		Build();
	}

	private void OpenStyleCreatorWidget()
	{
		var widget = new CodeWordCreatorWidget( null, this );
		widget.Show();
	}

	private void AddTitle( Layout canvas, string title, bool useSeparator )
	{
		if ( useSeparator )
		{
			canvas.Add( new Separator( 2f ) ).Color = Theme.SurfaceLightBackground;
		}

		Label label = new Label( title, this );
		label.SetStyles( "font-size: 20px; font-weight: 400;" );
		canvas.Add( label );
	}

	[EditorEvent.Frame]
	public void Frame()
	{
		if ( VerticalScrollHeight > 0 )
		{
			Scroll.VerticalScrollbar.Value = VerticalScrollHeight;
			VerticalScrollHeight = 0;
		}
	}
}
