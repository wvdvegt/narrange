using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System.Reflection;

namespace ZipTool
{
	class Program
	{
		#region Methods

		static void Main()
		{
			// helper to create a new release
			var version = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);

			var name = $"narrange-release {version}.zip";
			ZipFiles(name,
				new[]
				{
					"narrange-config.exe",
					"narrange-console.exe",
					"NArrange.Core.dll",
					"NArrange.CSharp.dll",
					"NArrange.Gui.dll",
					"NArrange.VisualBasic.dll",
					"DefaultConfig.v026.xml",
					"DefaultConfig.xml",
					"ICSharpCode.SharpZipLib.dll"
				});

			const string target = @"..\..\!Releases";
			if (!Directory.Exists(target))
				Directory.CreateDirectory(target);

			var tf = Path.Combine(target, name);

			if (File.Exists(tf))
				File.Delete(tf);

			File.Move(name, tf);
		}

		private static void ZipFiles(string output, string[] files)
		{
			if (File.Exists(output))
				File.Delete(output);

			using (var zip = ZipFile.Create(output))
			{
				zip.BeginUpdate();
				foreach (var file in files)
				{
					if (!File.Exists(file))
						throw new FileNotFoundException($"Missing file '{file}'. Cannot create release package!");
					zip.Add(file);
				}
				zip.CommitUpdate();
			}
		}

		#endregion
	}
}