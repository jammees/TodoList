using Editor;
using Todo.List;
using Todo.Widgets;

namespace Todo.Editors;

internal class SettingsWidget: Widget
{
	TodoWidget TodoWidget;

	public SettingsWidget( Widget parent, TodoWidget todoWidget ) : base( parent, true )
	{
		TodoWidget = todoWidget;

		DeleteOnClose = true;
		FixedSize = new( 500f, 400f );
		WindowTitle = $"Settings";
		SetWindowIcon( "settings" );
		WindowFlags |= WindowFlags.Widget;

		Layout = Layout.Column();
		Layout.Margin = 4;
		Layout.Spacing = 5;

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

	private void Build()
	{

	}
}
