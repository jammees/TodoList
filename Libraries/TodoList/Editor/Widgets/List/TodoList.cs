using Editor;
using Todo.List;
using Todo.Widgets.List.ItemControllers;
using Todo.Widgets.List.Items;
using static Sandbox.ParticleModelRenderer;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Todo.Widgets.List;

public sealed class TodoList : ListView
{
	public TodoList()
	{
		ItemSize = new Vector2( -1f, 30f );
		Layout = Layout.Column();
	}

	protected override void PaintItem( VirtualWidget item )
	{
		switch ( item.Object )
		{
			case ItemGroup group:
				ItemGroupController.OnPaint( group, item.Rect );
				break;
			case ManualEntry manual:
				ItemEntryManualController.OnPaint( manual, item.Rect );
				break;
			case CodeEntry code:
				ItemEntryCodeController.OnPaint( code, item.Rect );
				break;
			case ItemText text:
				ItemTextController.OnPaint( text, item.Rect );
				break;
		}
	}

	protected override bool OnItemPressed( VirtualWidget item, MouseEvent @event )
	{
		switch ( item.Object )
		{
			case ItemGroup group:
				ItemGroupController.OnClicked( group, @event );
				break;
			case ManualEntry manual:
				ItemEntryManualController.OnClicked( manual, @event );
				break;
			case CodeEntry code:
				ItemEntryCodeController.OnClicked( code, @event );
				break;
		}

		return false;
	}

	protected override void OnPaint()
	{
		Paint.ClearPen();
		Paint.SetBrush( Theme.ControlBackground );
		Paint.DrawRect( LocalRect, 4 );

		Paint.Antialiasing = true;
		Paint.TextAntialiasing = true;

		base.OnPaint();
	}
}
