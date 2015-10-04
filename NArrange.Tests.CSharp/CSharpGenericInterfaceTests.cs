using NUnit.Framework;

namespace NArrange.Tests.CSharp
{
	[TestFixture]
	public class CSharpGenericInterfaceTests
	{
		#region Methods

		[Test]
		public void TestInterfaces()
		{
			CSharp6FeatureTests.CompileArrangeAndRecompile("Generics.cs");
		}

		#endregion
	}
}