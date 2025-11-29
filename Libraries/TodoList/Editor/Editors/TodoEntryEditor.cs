using Editor;
using Todo.List;
using Todo.Widgets;

namespace Todo.Editors;

internal class TodoEntryEditor : Widget
{
	TextEdit MessageEdit;
	GroupControl GroupControl;

	TodoEntry Data;
	TodoDock TodoWidget;

	public TodoEntryEditor( Widget parent, TodoEntry data, TodoDock todoWidget ) : base( parent, true )
	{
		Data = data;
		TodoWidget = todoWidget;

		DeleteOnClose = true;
		FixedSize = new( 500f, 400f );
		WindowTitle = $"Edit {data.Message}";
		SetWindowIcon( "edit" );
		WindowFlags |= WindowFlags.Widget;

		Layout = Layout.Column();
		Layout.Margin = 4;
		Layout.Spacing = 5;

		Layout.Add( new Label( "Message", this ) );
		MessageEdit = Layout.Add( new TextEdit( this ) );
		MessageEdit.PlainText = data.Message;
		MessageEdit.PlaceholderText = "Todo message";

		GroupControl = new GroupControl( this, data.Group );

		Layout.AddStretchCell();

		WidgetButtonControls.AddWidgetButtonControls( this, SaveEdits, PromptDelete );
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

	private void PromptDelete()
	{
		Dialog.AskConfirm( DeleteData, "Are you sure you want to delete this?", $"Delete {Data.Message}?", "Yes", "No" );
	}

	private void DeleteData()
	{
		TodoWidget.DeleteData( Data );
		Close();
	}

	private void SaveEdits()
	{
		Data.Message = MessageEdit.PlainText;
		Data.Group = GroupControl.GetGroupName();

		TodoWidget.SaveAndRefresh();

		Close();
	}
}
