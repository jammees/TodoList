using Editor;
using Sandbox;
using Todo.Widgets;

namespace Todo.Editors;

internal class HelpWidget : Widget
{
	ScrollArea Scroll { get; set; }
	int VerticalScrollHeight { get; set; }

	public HelpWidget( Widget parent ) : base( parent, true )
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

	internal void Build()
	{
		if ( Scroll.IsValid() )
		{
			VerticalScrollHeight = Scroll.VerticalScrollbar.Value;
		}

		Layout.Clear( true );

		Scroll = Layout.Add( new ScrollArea( this ) );
		Scroll.Canvas = new Widget( this );
		Layout canvas = Scroll.Canvas.Layout = Layout.Column();
		canvas.Spacing = 10f;
		canvas.Margin = new Sandbox.UI.Margin( 5f, 5f, 20f, 5f );

		AddTitle( canvas, "Miscellaneous", false );

		canvas.AddStretchCell();
	}

	private void AddTitle( Layout canvas, string title, bool useSeparator )
	{
		if ( useSeparator )
		{
			canvas.Add( new Separator( 2f ) ).Color = Theme.SurfaceLightBackground;
		}

		Label label = new Label( title, this );
		label.SetStyles( "font-size: 20px; font-weight: 400;" );
		canvas.Add( label );
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
