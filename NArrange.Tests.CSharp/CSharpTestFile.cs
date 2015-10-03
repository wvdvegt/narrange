namespace NArrange.Tests.CSharp
{
	using Microsoft.CSharp;
	using NArrange.Tests.Core;
	using NUnit.Framework;
	using System.CodeDom.Compiler;
	using System.Collections.Generic;
	using System.IO;
	using System.Reflection;
	using System.Text;

	/// <summary>
	/// C# test file information.
	/// </summary>
	public class CSharpTestFile : ISourceCodeTestFile
	{
		#region Fields

		/// <summary>
		/// Cache of compiled test source files.
		/// </summary>
		private static Dictionary<string, Assembly> _compiledSourceFiles = new Dictionary<string, Assembly>();

		/// <summary>
		/// Assembly name.
		/// </summary>
		private Assembly _assembly;

		/// <summary>
		/// Resource file name.
		/// </summary>
		private string _resourceName;

		#endregion Fields

		#region Constructors

		/// <summary>
		/// Creates a new test file using the specified resource.
		/// </summary>
		/// <param name="resourceName">Name of the resource.</param>
		/// <param name="targetCSharp6">Set to true if C# 6 features are used. This will use the new Microsoft CodeDomCompiler, otherwise the old one is used.</param>
		public CSharpTestFile(string resourceName, bool targetCSharp6 = false)
		{
			_resourceName = resourceName;
			_assembly = GetAssembly(resourceName, targetCSharp6);
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Gets the assembly associated with the test file.
		/// </summary>
		public Assembly Assembly
		{
			get { return _assembly; }
		}

		/// <summary>
		/// Gets the name of the test file.
		/// </summary>
		public string Name
		{
			get { return _resourceName; }
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// Compiles C# source code.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="name">The assembly name.</param>
		/// <param name="targetCSharp6"></param>
		/// <returns>Compiler results.</returns>
		public static CompilerResults Compile(string source, string name, bool targetCSharp6 = false)
		{
			//
			// Compile the test source file
			//

			CompilerParameters parameters = new CompilerParameters();

			parameters.GenerateInMemory = true;
			parameters.GenerateExecutable = false;
			parameters.CompilerOptions = "/unsafe";

			parameters.ReferencedAssemblies.Add("mscorlib.dll");
			parameters.ReferencedAssemblies.Add("System.dll");
			parameters.ReferencedAssemblies.Add("System.Data.dll");
			parameters.ReferencedAssemblies.Add("System.Xml.dll");

			var provider = targetCSharp6 ?
				new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider() :
				CSharpCodeProvider.CreateProvider("CSharp");

			return provider.CompileAssemblyFromSource(parameters, source);
		}

		/// <summary>
		/// Retrieves a reader for the specified resource.
		/// </summary>
		/// <param name="resourceName">Name of the resource.</param>
		/// <returns>The test file text reader.</returns>
		public static TextReader GetTestFileReader(string resourceName)
		{
			return new StreamReader(GetTestFileStream(resourceName), Encoding.Default);
		}

		/// <summary>
		/// Opens a test file resource stream.
		/// </summary>
		/// <param name="resourceName">Name of the resource.</param>
		/// <returns>Steam for the test file resource.</returns>
		public static Stream GetTestFileStream(string resourceName)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			Stream stream = assembly.GetManifestResourceStream(
				typeof(CSharpTestUtilities), "TestSourceFiles." + resourceName);

			Assert.IsNotNull(stream, "Test stream could not be retrieved.");

			return stream;
		}

		/// <summary>
		/// Gets a TextReader for this test file.
		/// </summary>
		/// <returns>The TextReader.</returns>
		public TextReader GetReader()
		{
			return GetTestFileReader(_resourceName);
		}

		/// <summary>
		/// Gets the assembly.
		/// </summary>
		/// <param name="resourceName">Name of the resource.</param>
		/// <param name="targetCSharp6"></param>
		/// <returns>The assembly for the resource.</returns>
		private static Assembly GetAssembly(string resourceName, bool targetCSharp6 = false)
		{
			Assembly assembly = null;
			if (!_compiledSourceFiles.TryGetValue(resourceName, out assembly))
			{
				using (TextReader reader = GetTestFileReader(resourceName))
				{
					string source = reader.ReadToEnd();

					CompilerResults results = Compile(source, resourceName, targetCSharp6);

					if (results.Errors.Count > 0)
					{
						CompilerError error = null;

						error = TestUtilities.GetCompilerError(results);

						if (error != null)
						{
							string messageFormat =
								"Test source code should not produce compiler errors. " +
								"Error: {0} - {1}, line {2}, column {3} ";
							Assert.Fail(
								messageFormat,
								error.ErrorText,
								resourceName,
								error.Line,
								error.Column);
						}

						assembly = results.CompiledAssembly;
					}
				}

				if (assembly != null)
				{
					_compiledSourceFiles.Add(resourceName, assembly);
				}
			}

			return assembly;
		}

		#endregion Methods
	}
}