using Editor;
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

	private void OpenVisibilityMenu()
	{
		var menu = new Menu( this );

		{
			var option = menu.AddOption( new Option( this, "Show Manual Entries", "checklist" ) );
			option.Checkable = true;
			option.Checked = TodoDock.Instance.Cookies.ShowManualEntries;
			option.Toggled = SetManual;
		}

		{
			var option = menu.AddOption( new Option( this, "Show Code Entries", "code" ) );
			option.Checkable = true;
			option.Checked = TodoDock.Instance.Cookies.ShowCodeEntries;
			option.Toggled = SetCode;
		}

		menu.DeleteOnClose = true;
		menu.OpenAtCursor( true );
	}

	[Shortcut( "todo.toggle-manual-entries", "CTRL+1", typeof( TodoDock ), ShortcutType.Widget )]
	private void ToggleManual()
	{
		SetManual( !TodoDock.Instance.Cookies.ShowManualEntries );
	}

	[Shortcut( "todo.toggle-code-entries", "CTRL+2", typeof( TodoDock ), ShortcutType.Widget )]
	private void ToggleCode()
	{
		SetCode( !TodoDock.Instance.Cookies.ShowCodeEntries );
	}

	private void SetManual( bool state )
	{
		TodoDock.Instance.Cookies.ShowManualEntries = state;
		TodoDock.Instance.SaveAndRefresh();
	}

	private void SetCode( bool state )
	{
		TodoDock.Instance.Cookies.ShowCodeEntries = state;
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

		menu.DeleteOnClose = true;
		menu.OpenAtCursor( true );
	}

	private void OpenSettingsWidget()
	{
		var widget = new SettingsWidget( null );
		widget.Show();
	}
}
