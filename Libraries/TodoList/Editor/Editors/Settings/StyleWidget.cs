using Editor;
using Sandbox;

namespace Todo.Editors.Settings;

internal sealed class StyleWidget: Widget
{
	TodoCodeStyle Style;
	TodoCodeStyle EditedStyle;

	public StyleWidget( Widget parent, TodoCodeStyle style ): base( parent, false )
	{
		Style = style;

		MinimumHeight = Theme.RowHeight;
		Layout = Layout.Row();

		EditedStyle = new()
		{
			CodeWord = Style.CodeWord,
			Icon = Style.Icon,
			Tint = Style.Tint,
		};

		Build();
	}

	private void Build()
	{
		{
			ControlSheet sheet = new ControlSheet();
			sheet.Spacing = 4f;
			foreach ( var entry in EditedStyle.GetSerialized() )
			{
				sheet.AddRow( entry );
			}
			Layout.Add( sheet );
		}

		FolderMetadataDialog h;


		Layout.AddStretchCell();
	}
}
