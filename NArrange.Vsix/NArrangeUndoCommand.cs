namespace NArrange
{
    using System;
    using System.ComponentModel.Design;
    using System.IO;

    using Microsoft.VisualStudio.Shell;

    using EnvDTE;

    using NArrange.Core;
    using Task = System.Threading.Tasks.Task;

    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class NArrangeUndoCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0101;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("e553ddcc-24a6-46dc-9086-248e430667e2");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="NArrangeCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private NArrangeUndoCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static NArrangeUndoCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in NArrangeCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
            Instance = new NArrangeUndoCommand(package, commandService);
        }

        ILogger logger = new Logger();

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            ((Logger)logger).EmitHeader();

            //! Get Active Document Path.
            DTE dte = (DTE)ServiceProvider.GetServiceAsync(typeof(DTE))?.Result;

            if (dte == null)
            {
                logger.LogMessage(LogLevel.Error, "NArrange Could not obtain DTE.", new object[] { });

                return;
            }

            string document = dte?.ActiveDocument?.FullName;

            if (String.IsNullOrEmpty(document))
            {
                logger.LogMessage(LogLevel.Error, "NArrange Could not obtain the Path of the ActiveDocument.", new object[] { });

                return;
            }

            //! No CommandArguments (i.e. null).
            FileArranger fileArranger = new FileArranger(null, logger);

            string key = BackupUtilities.CreateFileNameKey(document);

            if (Directory.Exists(Path.Combine(BackupUtilities.BackupRoot, key)))
            {
                logger.LogMessage(LogLevel.Info, $"Restoring backup from {Path.Combine(BackupUtilities.BackupRoot, key)}...");

                //! Try to Undo NArrange of the Active File.
                Boolean success = BackupUtilities.RestoreFiles(BackupUtilities.BackupRoot, key);

                switch (success)
                {
                    case true:
                        logger.LogMessage(LogLevel.Info, "Undo NArrange Successful.", new object[] { });
                        break;
                    case false:
                        logger.LogMessage(LogLevel.Error, "Undo NArrange Failure.", new object[] { });
                        break;
                }
            }
            else
            {
                logger.LogMessage(LogLevel.Warning, "NArrange backup folder not found.", new object[] { });
            }
        }
    }
}
