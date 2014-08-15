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

using System;
using System.Collections.Generic;

namespace Store
{
	/// <summary>
	/// Common store delegate interface.
	/// </summary>
	public interface IStoreDelegate
	{
		/// <summary>
		/// Handles the request success event.
		/// </summary>
		/// <param name="identifier">product identifier.</param>
		void OnStoreRequestSuccess(IEnumerable<Product> products);
		
		/// <summary>
		/// Handles the request failed event.
		/// </summary>
		void OnStoreRequestFailed(string error);
		
		/// <summary>
		/// Handles the transaction success event.
		/// </summary>
		/// <param name="identifier">product identifier.</param>
		void OnStoreTransactionSuccess(string identifier);
		
		/// <summary>
		/// Handles the transaction failed event.
		/// </summary>
		/// <param name="identifier">product identifier.</param>
		void OnStoreTransactionFailed(string identifier);
		
		/// <summary>
		/// Handles the transaction restore event.
		/// </summary>
		/// <param name="identifier">product identifier.</param>
		void OnStoreTransactionRestore(string identifier);
	}
}