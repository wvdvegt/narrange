using NArrange.Core;
using NArrange.Tests.Core;
using NUnit.Framework;
using System.IO;

namespace NArrange.Tests.CSharp
{
	/// <summary>
	/// Tests the new C# 6 feature where auto properties can have initializers
	/// </summary>
	[TestFixture]
	public class InlinePropertyWhenPossibleTests
	{
		#region Fields

		private string _implementedPropertyResourceName = "PropertyChanges.PropertyWithLogicCode.cs";
		private string _prettyCodeResourceName = "PropertyChanges.PrettyCode.cs";
		private string _uglyCodeResourceName = "PropertyChanges.UglyCode.cs";

		#endregion

		#region Methods

		/// <summary>
		/// Asserts that multiline auto properties are reformatted to single line.
		/// </summary>
		[Test]
		public void CodeWithMultilineAutoPropertyShouldBeFormatted()
		{
			var tmp = GetTempCSharpFile();
			try
			{
				File.WriteAllText(tmp, GetTestFileContents(_uglyCodeResourceName));
				TestLogger logger = new TestLogger();
				FileArranger fileArranger = new FileArranger(null, logger);

				bool success = fileArranger.Arrange(tmp, null);

				Assert.IsTrue(success, "Expected file to be arranged succesfully. - " + logger);
				Assert.IsTrue(
					logger.HasMessage(LogLevel.Verbose, "1 files written."), "Expected 1 file to be written. - " + logger);

				string originalContents = GetTestFileContents(_prettyCodeResourceName);
				Assert.AreEqual(originalContents, File.ReadAllText(tmp), "File should have been formatted to look like the pretty code file.");
			}
			finally
			{
				File.Delete(tmp);
			}
		}

		/// <summary>
		/// Asserts that multiline properties (with backing field) are not reformatted.
		/// </summary>
		[Test]
		public void CodeWithMultilinePropertyShouldNotBeFormatted()
		{
			var tmp = GetTempCSharpFile();
			try
			{
				File.WriteAllText(tmp, GetTestFileContents(_implementedPropertyResourceName));
				TestLogger logger = new TestLogger();
				FileArranger fileArranger = new FileArranger(null, logger);

				bool success = fileArranger.Arrange(tmp, null);

				Assert.IsTrue(success, "Expected file to be arranged succesfully. - " + logger);
				Assert.IsTrue(
					logger.HasMessage(LogLevel.Verbose, "0 files written."), "Expected no file to be written. - " + logger);

				string originalContents = GetTestFileContents(_implementedPropertyResourceName);
				Assert.AreEqual(originalContents, File.ReadAllText(tmp), "File contents should have been preserved.");
			}
			finally
			{
				File.Delete(tmp);
			}
		}

		/// <summary>
		/// Asserts that inline properties are not modified.
		/// </summary>
		[Test]
		public void PrettyCodeShouldNotBeModified()
		{
			var tmp = GetTempCSharpFile();
			try
			{
				File.WriteAllText(tmp, GetTestFileContents(_prettyCodeResourceName));
				TestLogger logger = new TestLogger();
				FileArranger fileArranger = new FileArranger(null, logger);

				bool success = fileArranger.Arrange(tmp, null);

				Assert.IsTrue(success, "Expected file to be arranged succesfully. - " + logger);
				Assert.IsTrue(
					logger.HasMessage(LogLevel.Verbose, "0 files written."), "Expected no file to be written. - " + logger);

				string originalContents = GetTestFileContents(_prettyCodeResourceName);
				Assert.AreEqual(originalContents, File.ReadAllText(tmp), "File contents should have been preserved.");
			}
			finally
			{
				File.Delete(tmp);
			}
		}

		/// <summary>
		/// Gets the test file contents.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <returns>The test file contents.</returns>
		private static string GetTestFileContents(string fileName)
		{
			string contents = null;

			using (Stream stream = CSharpTestFile.GetTestFileStream(fileName))
			{
				Assert.IsNotNull(stream, "Test stream could not be retrieved.");

				StreamReader reader = new StreamReader(stream);
				contents = reader.ReadToEnd();
			}

			return contents;
		}

		private string GetTempCSharpFile()
		{
			var f = Path.GetTempFileName();
			File.Delete(f);
			return f + ".cs";
		}

		#endregion
	}
}