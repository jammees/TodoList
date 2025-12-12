using Editor;

namespace Todo.Widgets.List.Items;

public struct ItemText
{
	public enum TextType
	{
		Hint,
		Title
	}

	public string Text { get; set; }

	public TextType Type { get; set; }

	public ItemText()
	{
	}

	public ItemText( string text, TextType type )
	{
		Text = text;
		Type = type;
	}
}
