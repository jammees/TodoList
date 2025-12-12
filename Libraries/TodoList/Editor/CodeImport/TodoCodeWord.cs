using Editor;
using Sandbox;
using System.Collections.Generic;

namespace Todo.CodeImport;

public sealed class TodoCodeWord
{
	public string CodeWord { get; set; }

	public Color Tint { get; set; }

	[IconName]
	public string Icon { get; set; }

	public static List<TodoCodeWord> GetDefault()
	{
		return new List<TodoCodeWord>()
		{
			new()
			{
				Icon = "checklist",
				CodeWord = "todo:",
				Tint = Theme.Green
			},

			new()
			{
				Icon = "build",
				CodeWord = "fixme:",
				Tint = Theme.Yellow
			},

			new()
			{
				Icon = "bug_report",
				CodeWord = "bug:",
				Tint = Theme.Red
			},

			new()
			{
				Icon = "priority_high",
				CodeWord = "hack:",
				Tint = Theme.Red
			},

			new()
			{
				Icon = "sticky_note_2",
				CodeWord = "note:",
				Tint = Theme.Blue
			},

			new()
			{
				Icon = "question_mark",
				CodeWord = "xxx:",
				Tint = Theme.Pink
			},

			new()
			{
				Icon = "electric_bolt",
				CodeWord = "optimize:",
				Tint = Theme.Yellow
			}
		};
	}
}
