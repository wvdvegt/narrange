using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using EnvDTE;
using Task = System.Threading.Tasks.Task;

using NArrange.Core;

namespace NArrange
{  
    internal class Logger : ILogger
    {
        IVsOutputWindow logWnd = null;

        static readonly Guid logGuid = new Guid("{951BF0F1-9EA6-40FC-8F33-6718C5163238}");

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
