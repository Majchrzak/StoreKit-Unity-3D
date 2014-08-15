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

using UnityEngine;
using System;
using System.Runtime.InteropServices;

/// <summary>
/// Base protocol class that connects native device API and Unity layer.
/// </summary>
public abstract class DataProtocol<T> : MonoBehaviour where T : class
{
	/// <summary>
	/// Singleton instance.
	/// </summary>
	private static T _Instance;

	/// <summary>
	/// Returns class instance.
	/// </summary>
	public static T Instance
	{
		get
		{
			if (_Instance == null)
			{
				_Instance = GameObject.FindObjectOfType(typeof(T)) as T;
				
				if (_Instance == null)
				{
					GameObject gameObject = new GameObject(typeof(T).Name);
					GameObject.DontDestroyOnLoad(gameObject);
					
					_Instance = gameObject.AddComponent(typeof(T)) as T;
				}
			}
			
			return _Instance;
		}
	}

	/// <summary>
	/// Converts native Objective-C array into C# array.
	/// </summary>
	protected U[] ObjCMarshalArray<U>(IntPtr source, int length, U[] @default) where U : new()
	{
		if (source == IntPtr.Zero || length == 0)
			return @default;

		U[] array = new U[length];
		
		for (int i = 0; i < length; i++)
		{
			int offset = Marshal.SizeOf(typeof(U)) * i;
			
			array[i] = (U)Marshal.PtrToStructure(new IntPtr(source.ToInt32() + offset), typeof(U));
		}
		
		Marshal.FreeHGlobal(source);
		
		return array;
	}

}
