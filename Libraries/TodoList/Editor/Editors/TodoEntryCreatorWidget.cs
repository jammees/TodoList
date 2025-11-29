using Editor;
using Todo.List;
using Todo.Widgets;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Todo.Editors;

internal class TodoEntryCreatorWidget : Widget
{
	TextEdit MessageEdit;
	GroupControl GroupControl;

	public TodoEntryCreatorWidget( Widget parent ) : base( parent, true )
	{
		WidgetUtility.SetProperties(
			this,
			400f,
			"Create New Entry",
			"edit"
		);

		Layout = Layout.Column();
		Layout.Margin = 4;
		Layout.Spacing = 5;

		Layout.Add( new Label( "Message", this ) );
		MessageEdit = Layout.Add( new TextEdit( this ) );
		MessageEdit.PlaceholderText = "Todo Message";

		GroupControl = new GroupControl( this, "Default" );

		Layout.AddStretchCell();

		WidgetButtonControls.AddWidgetButtonControls( this, CreateEntry );
	}

	protected override void OnPaint()
	{
		Paint.ClearPen();
		Paint.SetBrush( Theme.ButtonBackground );
		Paint.DrawRect( LocalRect, 6f );
	}

	protected override void OnKeyPress( KeyEvent e )
	{
		if ( e.Key == KeyCode.Escape )
		{
			Close();
		}
	}

	private void CreateEntry()
	{
		TodoEntry entry = new()
		{
			Message = MessageEdit.PlainText,
			Group = GroupControl.GetGroupName()
		};

		TodoDock.Instance.Cookies.Datas.Add( entry );

		TodoDock.Instance.SaveAndRefresh();

		Close();
	}
}
