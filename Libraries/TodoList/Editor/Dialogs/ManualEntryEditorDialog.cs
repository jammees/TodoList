using Editor;
using Todo.List;
using Todo.Widgets;

namespace Todo.Dialogs;

public sealed class ManualEntryEditorDialog : Dialog
{
	TextEdit MessageEdit;
	GroupControl GroupControl;

	TodoEntry Data;

	public ManualEntryEditorDialog( Widget parent, TodoEntry data ) : base( parent, false )
	{
		Data = data;

		WidgetUtility.SetProperties(
			this,
			400f,
			$"Edit {data.Group}",
			"edit"
		);

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
		TodoDock.Cookies.Datas.Remove( Data );
		TodoDock.Instance.SaveAndRefresh();
		Close();
	}

	private void SaveEdits()
	{
		Data.Message = MessageEdit.PlainText;
		Data.Group = GroupControl.GetGroupName();

		TodoDock.Instance.SaveAndRefresh();

		Close();
	}
}
