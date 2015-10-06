namespace SampleNamespace
{
	public delegate void Test();

	public delegate void Test<T>(T sender);

	public delegate void Test2<in T>(T sender);

	public delegate void Test3<out T>();

	public delegate void Test2<in T, out T2, in TMore, out TSome>();
}