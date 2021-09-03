namespace NArrange.Tests.Core
{
    using System.Reflection;
    using NArrange.Core;
    using NArrange.Core.Configuration;
    using NUnit.Framework;
    using System;

    /// <summary>
    /// Test fixture for the ProjectHandler class.
    /// </summary>
    [TestFixture]
    public class ProjectHandlerTests
    {
        #region Methods

        /// <summary>
        /// Tests creating a new project handler.
        /// </summary>
        [Test]
        public void CreateTest()
        {
            ProjectHandlerConfiguration configuration = new ProjectHandlerConfiguration();
            configuration.ParserType = "NArrange.Core.MonoDevelopProjectParser";

            ProjectHandler handler = new ProjectHandler(configuration);

            Assert.IsNotNull(handler.ProjectParser, "Project parser was not created.");
            Assert.That(handler.ProjectParser, Is.InstanceOf(typeof(MonoDevelopProjectParser)));
        }

        /// <summary>
        /// Tests creating a new project handler.
        /// </summary>
        [Test]
        public void CreateWithAssemblyTest()
        {
#warning Failed because the assembly name was wrong.

            string assemblyName = Assembly.GetExecutingAssembly().FullName;

            //was hardcoded: "NArrange.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            ProjectHandlerConfiguration configuration = new ProjectHandlerConfiguration();
            configuration.AssemblyName = assemblyName;
            configuration.ParserType = "NArrange.Core.MSBuildProjectParser";

            ProjectHandler handler = new ProjectHandler(configuration);

            Assert.IsNotNull(handler.ProjectParser, "Project parser was not created.");
            Assert.That(handler.ProjectParser, Is.InstanceOf(typeof(MSBuildProjectParser)));
        }

        /// <summary>
        /// Tests creating a project handler with a default configuration.
        /// </summary>
        [Test]
        public void CreateWithDefaultConfigurationTest()
        {
            ProjectHandlerConfiguration configuration = new ProjectHandlerConfiguration();
            ProjectHandler projectHandler = new ProjectHandler(configuration);

            Assert.IsNotNull(projectHandler.ProjectParser, "Expected a project parser instance.");
            Assert.That(projectHandler.ProjectParser, Is.InstanceOf(typeof(MSBuildProjectParser) ));
        }

        /// <summary>
        /// Tests creating with a null configuration.
        /// </summary>
        [Test]
        public void CreateWithNullConfigurationTest()
        {
            Assert.Throws<ArgumentNullException>(
             delegate
             {
                 new ProjectHandler(null);
             });
        }

        #endregion
    }
}