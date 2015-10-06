using NUnit.Framework;

namespace NArrange.Tests.CSharp
{
	[TestFixture]
	public class CSharpGenericDelegateTests
	{
		#region Methods

		[Test]
		public void TestCovarianceContraviarance()
		{
			CSharp6FeatureTests.CompileArrangeAndRecompile("Delegates.cs");
		}

		#endregion
	}
}