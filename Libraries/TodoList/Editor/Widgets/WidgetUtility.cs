using Editor;
using Sandbox.Internal;

namespace Todo.Widgets;

public static class WidgetUtility
{
	public static void SetProperties( Dialog dialog, float height, string title, string icon )
	{
		dialog.FocusMode = FocusMode.Click;
		
		Window dialogWindow = dialog.Window;

		dialogWindow.IsDialog = false;
		dialogWindow.SetWindowIcon( icon );
		dialogWindow.StatusBar = null;
		dialogWindow.Title = title;
		dialogWindow.Size = new( 500f, height );

		dialogWindow.Parent = EditorWindow.GetWindow();
		dialogWindow.WindowFlags |= WindowFlags.Window;

		if ( TodoDock.Cookies.WidgetsOnTop )
		{
			dialog.WindowFlags |= WindowFlags.WindowStaysOnTopHint;
		}
	}
}
