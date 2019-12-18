namespace NArrange
{
    using System;
    using System.Diagnostics;
    using System.Reflection;

    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;

    using NArrange.Core;

    internal class Logger : ILogger
    {
        IVsOutputWindow logWnd = null;

        static readonly Guid logGuid = new Guid("{951BF0F1-9EA6-40FC-8F33-6718C5163238}");

        internal void EmitHeader()
        {
            LogMessage(LogLevel.Info, "".PadRight(120, '─'), new object[] { });

            //! Type of attribute that is desired
            Type type = typeof(AssemblyDescriptionAttribute);

            Assembly asm = Assembly.GetExecutingAssembly();

            //! Is there an attribute of this type already defined?
            if (AssemblyDescriptionAttribute.IsDefined(asm, type))
            {
                //! if there is, get attribute of desired type
                AssemblyDescriptionAttribute ada = (AssemblyDescriptionAttribute)AssemblyDescriptionAttribute.GetCustomAttribute(asm, type);

                LogMessage(LogLevel.Info, ada.Description, new object[] { });
            }

            FileVersionInfo vi = FileVersionInfo.GetVersionInfo(asm.Location);

            LogMessage(LogLevel.Info, "{0} v{1}", new object[] { vi.ProductName.ToString(), vi.FileVersion.ToString() });
            LogMessage(LogLevel.Info, vi.LegalCopyright, new object[] { });
            LogMessage(LogLevel.Info, "".PadRight(120, '─'), new object[] { });
        }

        public void LogMessage(LogLevel level, string message, params object[] args)
        {
            //! Copy Guid.
            Guid customGuid = logGuid;

            //! Get OutputWindow and Create a new Pane.
            if (logWnd == null)
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                logWnd = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;
                logWnd.CreatePane(ref customGuid, "NArrange", 1, 1);
            }

            //! Get Pane, Log to it and Activate it.
            if (logWnd != null)
            {
                IVsOutputWindowPane customPane;
                logWnd.GetPane(ref customGuid, out customPane);

                customPane.OutputString(String.Format(message, args));
                customPane.OutputString("\r\n");

                //! Brings this pane into view
                customPane.Activate();
            }

            Debug.WriteLine(message, args);
        }
    }
}
