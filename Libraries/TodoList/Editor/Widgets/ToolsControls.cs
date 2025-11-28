using Editor;
using System;

namespace Todo.Widgets;

internal sealed class ToolsControls : Widget
{
	public Action OnRefreshClicked;
	public Action OnVisibilityClicked;

	public ToolsControls( Widget parent ) : base( parent )
	{
		Layout = Layout.Row();
		Layout.Spacing = 2f;

		ToolButton refreshButton = Layout.Add( new ToolButton( "", "refresh", this ) );
		refreshButton.MouseClick = OnRefreshClicked;
		refreshButton.ToolTip = "Refresh All";

		ToolButton showVisibilityButton = Layout.Add( new ToolButton( "", "visibility", this ) );
		showVisibilityButton.MouseClick = OnVisibilityClicked;
		showVisibilityButton.ToolTip = "Change Visibility";
	}

	protected override void OnPaint()
	{
		Paint.ClearPen();
		Paint.SetBrush( Theme.ControlBackground );
		Paint.DrawRect( LocalRect, 4 );
	}
}
