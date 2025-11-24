
public sealed class MyComponent : Component
{
	[Property] public string StringProperty { get; set; }

	protected override void OnUpdate()
	{
		Log.Info( "wara2" );
	}
}
