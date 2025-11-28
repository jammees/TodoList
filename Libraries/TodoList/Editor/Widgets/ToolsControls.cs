using Editor;
using System;

namespace Todo.Widgets;

internal sealed class ToolsControls : Widget
{
	public Action OnRefreshClicked { get; set; }
	public Action OnVisibilityClicked { get; set; }
	public Action OnMoreClicked { get; set; }

	public ToolsControls( Widget parent ) : base( parent )
	{
		Layout = Layout.Row();
		Layout.Spacing = 2f;

		ToolButton refreshButton = Layout.Add( new ToolButton( "", "refresh", this ) );
		refreshButton.MouseClick = () => OnRefreshClicked?.Invoke();
		refreshButton.ToolTip = "Refresh All";

		ToolButton showVisibilityButton = Layout.Add( new ToolButton( "", "visibility", this ) );
		showVisibilityButton.MouseClick = () => OnVisibilityClicked?.Invoke();
		showVisibilityButton.ToolTip = "Change Visibility";

		ToolButton moreButton = Layout.Add( new ToolButton( "", "more_vert", this ) );
		moreButton.MouseClick = () => OnMoreClicked?.Invoke();
		moreButton.ToolTip = "More";
	}

	protected override void OnPaint()
	{
		Paint.ClearPen();
		Paint.SetBrush( Theme.ControlBackground );
		Paint.DrawRect( LocalRect, 4 );
	}
}
