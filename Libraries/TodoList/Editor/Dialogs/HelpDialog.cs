using Editor;
using Sandbox;
using Todo.Widgets;

namespace Todo.Dialogs;

public sealed class HelpDialog : Dialog
{
	ScrollArea Scroll { get; set; }
	int VerticalScrollHeight { get; set; }

	public HelpDialog( Widget parent ) : base( parent, false )
	{
		WidgetUtility.SetProperties(
			this,
			400f,
			"Help",
			"question_mark"
		);

		Layout = Layout.Column();
		Layout.Margin = 4f;
		Layout.Spacing = 5f;

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

	protected override bool OnClose()
	{
		TodoDock.Instance.SaveAndRefresh();

		return base.OnClose();
	}

	public void Build()
	{
		if ( Scroll.IsValid() )
		{
			VerticalScrollHeight = Scroll.VerticalScrollbar.Value;
		}

		Layout.Clear( true );

		Scroll = Layout.Add( new ScrollArea( this ) );
		Scroll.HorizontalScrollbar.Visible = false;
		Scroll.HorizontalScrollbar.Enabled = false;
		Scroll.Canvas = new Widget( this );
		Layout canvas = Scroll.Canvas.Layout = Layout.Column();
		canvas.Spacing = 10f;
		canvas.Margin = new Sandbox.UI.Margin( 5f, 5f, 20f, 5f );

		AddTitle( "Searchbar", false );

		AddBody(
			"The searchbar allows to add flags next to the search terms to further reduce " +
			"down all the possible results."
		);

		AddBody(
			"All of the flags follow the same rule of FLAG_NAME:ARGUMENT. There " +
			"are some flags that do not require the argument part, but still " +
			"need to have the \":\" to be recognized as one."
		);

		AddBody(
			"Here's a list of all the built-in flags and their use cases:",
			true
		);

		// Built-in flags
		AddFlag(
			"in:",
			"Group Name",
			"Filters out all the groups that do not contain the argument in their " +
			"names."
		);

		AddFlag(
			"not:",
			"Group Name",
			"Filters out all the groups that do contain the argument in their " +
			"names."
		);

		AddFlag(
			"done:",
			"None",
			"Only used for manual entries. Filters them out based on, whether they " +
			"are marked as done."
		);

		AddFlag(
			"pending:",
			"None",
			"Only used for manual entries. Filters them out based on, whether they " +
			"are still being worked on."
		);

		// Code words
		AddBody(
			"Code words as flags",
			true
		);

		canvas.Add( new WarningBox( "While a \":\" is not required for code words to be " +
			"defined in the settings, if there's one already, there's no need to add another " +
			"\":\" at the end of the flag. That would result in the library looking for entries " +
			"that were defined with \"todo::\" code word!"
		));

		AddBody(
			"To filter out code words, input their name based on, how they " +
			"were defined in the settings then follow it with a \":\"."
		);

		AddBody(
			"For example todo: - will find all the code entries that were marked as such."
		);

		AddTitle( "Editing Manual Entries", true );

		AddBody(
			"Entries",
			true
		);

		AddBody(
			"If an entry needs to be changed, use the SHIFT + CLICK combination. This " +
			"will open up an editor, to change their message and save them or to delete them."
		);

		AddBody(
			"Groups",
			true
		);

		AddBody(
			"The editor for groups can be opened in the same way as entries. The editor " +
			"allows to change the group's name or to delete every single entry under that " +
			"group."
		);

		AddTitle( "Widgets", true );

		AddBody(
			"All widgets can be closed much faster by pressing ESC."
		);

		canvas.AddStretchCell();
	}

	private void AddFlag( string name, string argument, string description )
	{
		AddBody(
			name,
			bold: true,
			indent: 15f
		);

		AddBody(
			$"Argument: {argument}",
			indent: 25f
		);

		AddBody(
			description,
			indent: 25f
		);
	}

	private void AddBody( string content, bool bold = false, float indent = 0f )
	{
		Label label = Scroll.Canvas.Layout.Add( new Label(content, this) );
		label.WordWrap = true;
		label.SetStyles( "font-size: 12px;" );
		label.Indent = indent;
		if ( bold is true )
		{
			label.SetStyles( "font-size: 15px; font-weight: 500;" );
		}
	}

	private void AddTitle( string title, bool useSeparator )
	{
		if ( useSeparator )
		{
			Scroll.Canvas.Layout.Add( new Separator( 2f ) ).Color = Theme.SurfaceLightBackground;
		}

		Label label = new Label( title, this );
		label.SetStyles( "font-size: 25px; font-weight: 500;" );
		Scroll.Canvas.Layout.Add( label );
	}

	[EditorEvent.Frame]
	public void Frame()
	{
		if ( VerticalScrollHeight > 0 )
		{
			Scroll.VerticalScrollbar.Value = VerticalScrollHeight;
			VerticalScrollHeight = 0;
		}
	}
}
