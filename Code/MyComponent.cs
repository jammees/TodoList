
public sealed class MyComponent : Component
{
	[Property] public string StringProperty { get; set; }

	// TODO: finish working on this
	public void Empty()
	{
		// TODO: this is inside of the thing
		// TODO: under this one ^
	}

	//toDo:this is some uneven formatting
	//with multilines
	public void Empty2()
	{
	}

	// TODO: this is a todo message
	//
	// that expans multiple lines
	protected override void OnUpdate()
	{
		Log.Info( "wara2" );
	}
}
