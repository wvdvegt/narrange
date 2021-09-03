namespace NArrange.Tests.Core
{
    using NArrange.Core;
    using NArrange.Core.CodeElements;
    using NUnit.Framework;
    using System;

    /// <summary>
    /// Test fixture for the ChainElementArranger class.
    /// </summary>
    [TestFixture]
    public class ChainElementArrangerTests
    {
        #region Methods

        /// <summary>
        /// Tests the AddArranger method with a null arranger.
        /// </summary>
        [Test]
        public void AddArrangerNullTest()
        {
            ChainElementArranger chainArranger = new ChainElementArranger();
            Assert.Throws<ArgumentNullException>(
     delegate
     {
         chainArranger.AddArranger(null);
     });
        }

        /// <summary>
        /// Tests the CanArrange method.
        /// </summary>
        [Test]
        public void CanArrangeTest()
        {
            ChainElementArranger chain = new ChainElementArranger();
            FieldElement fieldElement = new FieldElement();

            //
            // No arrangers in chain
            //
            Assert.IsFalse(
                chain.CanArrange(fieldElement),
                "Empty chain element arranger should not be able to arrange an element.");

            //
            // Add an arranger that can't arrange the element
            //
            TestElementArranger disabledArranger = new TestElementArranger(false);
            chain.AddArranger(disabledArranger);
            Assert.IsFalse(chain.CanArrange(fieldElement), "Unexpected return value from CanArrange.");

            //
            // Add an arranger that can arrange the element
            //
            TestElementArranger enabledArranger = new TestElementArranger(true);
            chain.AddArranger(enabledArranger);
            Assert.IsTrue(chain.CanArrange(fieldElement), "Unexpected return value from CanArrange.");

            //
            // Null
            //
            Assert.IsFalse(chain.CanArrange(null), "Unexpected return value from CanArrange.");
        }

        /// <summary>
        /// Tests the Arrange method with an element that cannot be handled.
        /// </summary>
        [Test]
        public void UnsupportedArrangeNoParentTest()
        {
            ChainElementArranger chain = new ChainElementArranger();
            FieldElement fieldElement = new FieldElement();

            //
            // Add an arranger that can't arrange the element
            //
            TestElementArranger disabledArranger = new TestElementArranger(false);

            Assert.Throws<InvalidOperationException>(
             delegate
             {
                 chain.AddArranger(disabledArranger);
                 Assert.IsFalse(chain.CanArrange(fieldElement), "Unexpected return value from CanArrange.");

                 chain.ArrangeElement(null, fieldElement);
             });
        }

        /// <summary>
        /// Tests the Arrange method with an element that cannot be handled.
        /// </summary>
        [Test]
        public void UnsupportedArrangeWithParentTest()
        {
            GroupElement parentElement = new GroupElement();
            ChainElementArranger chain = new ChainElementArranger();
            FieldElement fieldElement = new FieldElement();

            //
            // Add an arranger that can't arrange the element
            //
            TestElementArranger disabledArranger = new TestElementArranger(false);
            chain.AddArranger(disabledArranger);
            Assert.IsFalse(chain.CanArrange(fieldElement), "Unexpected return value from CanArrange.");

            chain.ArrangeElement(parentElement, fieldElement);
            Assert.IsTrue(parentElement.Children.Contains(fieldElement));
        }

        #endregion

        #region Other

        /// <summary>
        /// Test code element arranger.
        /// </summary>
        private class TestElementArranger : IElementArranger
        {
            /// <summary>
            /// Whether or not arrage was called.
            /// </summary>
            private bool _arrangeCalled;

            /// <summary>
            /// Whether or not this arranger can process elements.
            /// </summary>
            private bool _canArrange;

            /// <summary>
            /// Creates a test element arranger.
            /// </summary>
            /// <param name="canArrange">Fixed value for the CanArrange property.</param>
            public TestElementArranger(bool canArrange)
            {
                _canArrange = canArrange;
            }

            /// <summary>
            /// Gets a value indicating whether or not arrange was called.
            /// </summary>
            public bool ArrangeCalled
            {
                get { return _arrangeCalled; }
            }

            /// <summary>
            /// Arranges the specified element within the parent element.
            /// </summary>
            /// <param name="parentElement">The parent element.</param>
            /// <param name="codeElement">The code element.</param>
            public void ArrangeElement(ICodeElement parentElement, ICodeElement codeElement)
            {
                _arrangeCalled = true;
            }

            /// <summary>
            /// Determines whether or not this arranger can handle arrangement of
            /// the specified element.
            /// </summary>
            /// <param name="codeElement">The code element.</param>
            /// <returns>
            /// <c>true</c> if this instance can arrange the specified code element; otherwise, <c>false</c>.
            /// </returns>
            public bool CanArrange(ICodeElement codeElement)
            {
                return _canArrange;
            }

            /// <summary>
            /// Determines whether or not this arranger can handle arrangement of
            /// the specified element.
            /// </summary>
            /// <param name="parentElement">The parent element.</param>
            /// <param name="codeElement">The code element.</param>
            /// <returns>
            /// <c>true</c> if this instance can arrange the specified parent element; otherwise, <c>false</c>.
            /// </returns>
            public bool CanArrange(ICodeElement parentElement, ICodeElement codeElement)
            {
                return _canArrange;
            }
        }

        #endregion
    }
}