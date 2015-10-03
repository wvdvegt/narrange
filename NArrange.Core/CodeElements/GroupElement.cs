#region Header

/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * Copyright (c) 2007-2008 James Nies and NArrange contributors.
 *    All rights reserved.
 *
 * This program and the accompanying materials are made available under
 * the terms of the Common Public License v1.0 which accompanies this
 * distribution.
 *
 * Redistribution and use in source and binary forms, with or
 * without modification, are permitted provided that the following
 * conditions are met:
 *
 * Redistributions of source code must retain the above copyright
 * notice, this list of conditions and the following disclaimer.
 * Redistributions in binary form must reproduce the above copyright
 * notice, this list of conditions and the following disclaimer in
 * the documentation and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
 * FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
 * OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
 * TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA,
 * OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY
 * OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 *<author>James Nies</author>
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

#endregion Header

namespace NArrange.Core.CodeElements
{
	/// <summary>
	/// Element used to logically group other elements.
	/// </summary>
	public sealed class GroupElement : CodeElement
	{
		#region Fields

		/// <summary>
		/// Custom separator text.
		/// </summary>
		private string _customSeparator;

		/// <summary>
		/// Separator type.
		/// </summary>
		private GroupSeparatorType _separatorType;

		#endregion Fields

		#region Constructors

		/// <summary>
		/// Creates a group element.
		/// </summary>
		public GroupElement()
		{
		}

		/// <summary>
		/// Creates a group element with the specified name.
		/// </summary>
		/// <param name="name">The group name.</param>
		public GroupElement(string name)
			: this()
		{
			Name = name;
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Gets or sets the custom separator string.
		/// </summary>
		public string CustomSeparator
		{
			get { return _customSeparator; }
			set { _customSeparator = value; }
		}

		/// <summary>
		/// Gets the element type.
		/// </summary>
		public override ElementType ElementType
		{
			get { return ElementType.NotSpecified; }
		}

		/// <summary>
		/// Gets or sets the separator type.
		/// </summary>
		public GroupSeparatorType SeparatorType
		{
			get { return _separatorType; }
			set { _separatorType = value; }
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// Allows an ICodeElementVisitor to process (or visit) this element.
		/// </summary>
		/// <param name="visitor">Visitor to accept the code element.</param>
		/// <remarks>See the Gang of Four Visitor design pattern.</remarks>
		public override void Accept(ICodeElementVisitor visitor)
		{
			visitor.VisitGroupElement(this);
		}

		/// <summary>
		/// Creates a clone of this instance.
		/// </summary>
		/// <returns>Clone of the code element.</returns>
		protected override CodeElement DoClone()
		{
			GroupElement clone = new GroupElement();
			clone._customSeparator = _customSeparator;
			clone._separatorType = _separatorType;

			return clone;
		}

		#endregion Methods
	}
}