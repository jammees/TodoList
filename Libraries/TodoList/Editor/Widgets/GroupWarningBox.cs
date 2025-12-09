using Editor;

namespace Todo.Widgets;

internal sealed class GroupWarningBox: WarningBox
{
	public GroupWarningBox( Widget parent, LineEdit groupEdit )
	{
		BackgroundColor = Theme.Red;
		Icon = "error";
		Visible = false;

		groupEdit.TextChanged += OnTextEdited;
		OnTextEdited( groupEdit.Text );
	}

	private void OnTextEdited( string newString )
	{
		bool isValid = true;

		if ( string.IsNullOrEmpty( newString.Trim() ) is true )
		{
			isValid = false;
			SetWarningMessage( $"Empty group name, will default to {TodoDock.Instance.Cookies.DefaultGroupName}!" );
		}

		if ( (newString.EndsWith( ".cs" ) || newString.EndsWith( ".razor" )) is true )
		{
			isValid = false;
			SetWarningMessage( "Group ends with .cs or .razor extension, will break " +
				"groups if a file is named the same!" );
		}

		Visible = isValid is false;
	}

	private void SetWarningMessage( string reason )
	{
		Label.Text = $"Invalid Group! Reason: {reason}";
	}
}
