using Editor;
using System;
using static Editor.Button;

namespace Todo.Widgets;

public static class WidgetButtonControls
{
	public static void AddWidgetButtonControls( Widget parent, Action onSave, Action onDelete = null )
	{
		Widget controls = parent.Layout.Add( new Widget( parent ) );
		Layout controlsLayout = controls.Layout = Layout.Row();
		controlsLayout.Spacing = 5f;

		Primary saveButton = controlsLayout.Add( new Primary( "Save", "save", controls ) );
		saveButton.Clicked = onSave;

		Button closeButton = controlsLayout.Add( new Button( "Cancel", "cancel", controls ) );
		closeButton.Clicked = parent.Close;

		if ( onDelete is not null )
		{
			Danger deleteButton = parent.Layout.Add( new Danger( "Delete", "delete", parent ) );
			deleteButton.Clicked = () => onDelete();
		}
	}
}
