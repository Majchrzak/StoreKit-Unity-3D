/*
 *  The MIT License (MIT)
 *
 *	Copyright (c) 2014 Mateusz Majchrzak
 *
 *	Permission is hereby granted, free of charge, to any person obtaining a copy
 *	of this software and associated documentation files (the "Software"), to deal
 *	in the Software without restriction, including without limitation the rights
 *	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *	copies of the Software, and to permit persons to whom the Software is
 *	furnished to do so, subject to the following conditions:
 *
 *	The above copyright notice and this permission notice shall be included in all
 *	copies or substantial portions of the Software.
 *
 *	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *	SOFTWARE.
 */

namespace Store
{
	/// <summary>
	/// Store product struct.
	/// </summary>
	public struct Product
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">Unique product identifier.</param>
		/// <param name="title">Product title.</param>
		/// <param name="price">ocalized product price (with price tag).</param>
		/// <param name="description">Localized description.</param>
		internal Product(string id, string title, string price, string description)
		{
		    ID = id;
		    Title = title;
		    Price = price;
		    Description = description;
		}

		/// <summary>
		/// Unique product identifier.
		/// </summary>
		public string ID { get; private set; }

		/// <summary>
		/// Product title.
		/// </summary>
		public string Title { get; private set; }

		/// <summary>
		/// Localized product price (with price tag).
		/// </summary>
		public string Price { get; private set; }

		/// <summary>
		/// Localized description.
		/// </summary>
		public string Description { get; private set; }
	}
}