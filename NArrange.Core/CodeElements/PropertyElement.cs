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
	/// Property element.
	/// </summary>
	public sealed class PropertyElement : InterfaceMemberElement
	{
		#region Fields

		private string _autoPropertyInitializer;

		/// <summary>
		/// Index parameter name.
		/// </summary>
		private string _indexParameter;

		#endregion Fields

		#region Properties

		/// <summary>
		/// C# 6.0 feature.
		/// Auto properties can be get;set; or get; only. In both cases, it is valid to initialize inline like so:
		/// <example>public string MyProperty { get; } = "inline initializer";</example>
		/// This property will hold the initilizer content.
		/// </summary>
		public string AutoPropertyInitializer
		{
			get { return _autoPropertyInitializer; }
			set { _autoPropertyInitializer = value; }
		}

		/// <summary>
		/// Gets the element type.
		/// </summary>
		public override ElementType ElementType
		{
			get { return ElementType.Property; }
		}

		/// <summary>
		/// Used only if <see cref="IsExpressionBodyProperty"/> is true.
		/// </summary>
		public string ExpressionBodyText { get; set; }

		/// <summary>
		/// Gets or sets the property index parameter.
		/// </summary>
		public string IndexParameter
		{
			get { return _indexParameter; }
			set { _indexParameter = value; }
		}

		/// <summary>
		/// New C# 6 API for implicitly declared get-only properties (have neither getter nor setter, but only an expression body).
		/// </summary>
		public bool IsExpressionBodyProperty { get; set; }

		#endregion Properties

		#region Methods

		/// <summary>
		/// Allows an ICodeElementVisitor to process (or visit) this element.
		/// </summary>
		/// <param name="visitor">Visitor to accept the code element.</param>
		/// <remarks>See the Gang of Four Visitor design pattern.</remarks>
		public override void Accept(ICodeElementVisitor visitor)
		{
			visitor.VisitPropertyElement(this);
		}

		/// <summary>
		/// Creates a clone of this instance.
		/// </summary>
		/// <returns>Clone of the element with interface member state copied.</returns>
		protected override InterfaceMemberElement DoInterfaceMemberClone()
		{
			PropertyElement propertyElement = new PropertyElement();
			propertyElement.IsExpressionBodyProperty = IsExpressionBodyProperty;
			propertyElement._indexParameter = _indexParameter;
			propertyElement._autoPropertyInitializer = _autoPropertyInitializer;
			propertyElement.ExpressionBodyText = ExpressionBodyText;
			return propertyElement;
		}

		#endregion Methods
	}
}