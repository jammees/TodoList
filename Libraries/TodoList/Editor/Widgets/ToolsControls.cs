using Editor;
using Sandbox;
using Todo.Editors;

namespace Todo.Widgets;

internal sealed class ToolsControls : Widget
{
	public ToolsControls( Widget parent ) : base( parent )
	{
		Layout = Layout.Row();
		Layout.Spacing = 2f;

		ToolButton refreshButton = Layout.Add( new ToolButton( "", "refresh", this ) );
		refreshButton.MouseClick = TodoDock.Instance.RefreshItems;
		refreshButton.ToolTip = "Refresh All";

		ToolButton showVisibilityButton = Layout.Add( new ToolButton( "", "visibility", this ) );
		showVisibilityButton.MouseClick = OpenVisibilityMenu;
		showVisibilityButton.ToolTip = "Change Visibility";
		showVisibilityButton.OnPaintOverride = () =>
		{
			OnVisibilityPaint( showVisibilityButton.LocalRect, showVisibilityButton );
			return true;
		};

		ToolButton moreButton = Layout.Add( new ToolButton( "", "more_vert", this ) );
		moreButton.MouseClick = OpenMoreMenu;
		moreButton.ToolTip = "More";
	}

	protected override void OnPaint()
	{
		Paint.ClearPen();
		Paint.SetBrush( Theme.ControlBackground );
		Paint.DrawRect( LocalRect, 4 );
	}

	// Edited version of ToolButton's OnPaint method
	private void OnVisibilityPaint( Rect rect, ToolButton button )
	{
		Color color = Color.White;

		bool showManual = TodoDock.Cookies.ShowManualEntries;
		bool showCode = TodoDock.Cookies.ShowCodeEntries;

		if ( showManual && showCode )
		{
			color = Theme.Blue;
		}
		else if ( showCode )
		{
			color = Theme.Green;
		}
		else if ( showManual is false && showCode is false )
		{
			color = Theme.TextDisabled;
		}

		Paint.ClearPen();
		if ( Paint.HasMouseOver )
			Paint.SetBrush( Theme.SurfaceBackground );
		else
			Paint.SetBrush( Theme.ControlBackground );

		Paint.DrawRect( rect.Shrink( 1.0f ), Theme.ControlRadius );

		Paint.SetPen( color );
		Paint.DrawIcon( rect, button.Icon, 14, TextFlag.Center );

		Update();
	}

	private void OpenVisibilityMenu()
	{
		var menu = new Menu( this );

		{
			var option = menu.AddOption( new Option( this, "Show Manual Entries", "checklist" ) );
			option.Checkable = true;
			option.Checked = TodoDock.Cookies.ShowManualEntries;
			option.Toggled = SetManual;
		}

		{
			var option = menu.AddOption( new Option( this, "Show Code Entries", "code" ) );
			option.Checkable = true;
			option.Checked = TodoDock.Cookies.ShowCodeEntries;
			option.Toggled = SetCode;
		}

		menu.DeleteOnClose = true;
		menu.OpenAtCursor( true );
	}

	[Shortcut( "todo.toggle-manual-entries", "CTRL+1", typeof( TodoDock ), ShortcutType.Application )]
	private void ToggleManual()
	{
		SetManual( !TodoDock.Cookies.ShowManualEntries );
	}

	[Shortcut( "todo.toggle-code-entries", "CTRL+2", typeof( TodoDock ), ShortcutType.Application )]
	private void ToggleCode()
	{
		SetCode( !TodoDock.Cookies.ShowCodeEntries );
	}

	private void SetManual( bool state )
	{
		TodoDock.Cookies.ShowManualEntries = state;
		TodoDock.Instance.SaveAndRefresh();
	}

	private void SetCode( bool state )
	{
		TodoDock.Cookies.ShowCodeEntries = state;
		TodoDock.Instance.SaveAndRefresh();
	}

	private void OpenMoreMenu()
	{
		var menu = new Menu( this );

		{
			var option = menu.AddOption( new Option( this, "Import Entries", "download" ) );
			option.Triggered = () => TodoDock.Import();
		}

		{
			var option = menu.AddOption( new Option( this, "Export Entries", "upload" ) );
			option.Triggered = () => TodoDock.Export();
		}

		menu.AddSeparator();

		{
			var option = menu.AddOption( new Option( this, "Settings", "settings" ) );
			option.Triggered = () => OpenSettingsWidget();
		}

		{
			var option = menu.AddOption( new Option( this, "Help", "question_mark" ) );
			option.Triggered = () => OpenHelpWidget();
		}

		menu.DeleteOnClose = true;
		menu.OpenAtCursor( true );
	}

	private void OpenHelpWidget()
	{
		var widget = new HelpWidget( null );
		widget.Show();
	}

	private void OpenSettingsWidget()
	{
		var widget = new SettingsWidget( null );
		widget.Show();
	}
}
