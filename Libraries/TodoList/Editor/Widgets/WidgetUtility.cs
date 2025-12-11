using Editor;

namespace Todo.Widgets;

internal static class WidgetUtility
{
	internal static void SetProperties( Widget widget, float height, string title, string icon )
	{
		widget.DeleteOnClose = true;
		widget.MinimumSize = new( 500f, height );
		widget.WindowTitle = title;
		widget.SetWindowIcon( icon );
		widget.FocusMode = FocusMode.Click;

		if ( TodoDock.Cookies.WidgetsOnTop )
		{
			widget.WindowFlags |= WindowFlags.WindowStaysOnTopHint;
		}
	}
}
