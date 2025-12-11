using Editor;

namespace Todo.Widgets;

internal static class WidgetUtility
{
	internal static void SetProperties( Widget widget, float height, string title, string icon )
	{
		widget.DeleteOnClose = true;
		widget.FixedSize = new( 500f, height );
		widget.WindowTitle = title;
		widget.SetWindowIcon( icon );

		if ( TodoDock.Cookies.WidgetsOnTop )
		{
			widget.WindowFlags |= WindowFlags.WindowStaysOnTopHint;
		}
	}
}
