// Copyright (C) 2001 Gerry Shaw
//
// This software is provided 'as-is', without any express or implied
// warranty.  In no event will the authors be held liable for any damages
// arising from the use of this software.
//
// Permission is granted to anyone to use this software for any purpose,
// including commercial applications, and to alter it and redistribute it
// freely, subject to the following restrictions:
//
// 1. The origin of this software must not be misrepresented; you must not
//	claim that you wrote the original software. If you use this software
//	in a product, an acknowledgment in the product documentation would be
//	appreciated but is not required.
// 2. Altered source versions must be plainly marked as such, and must not be
//	misrepresented as being the original software.
// 3. This notice may not be removed or altered from any source distribution.
//
// Gerry Shaw (gerry_shaw@yahoo.com)

/* #zlib - Wrapping and enhancing the zlib
 * Copyright (C) 2005-06, Tyron Madlener <zlib@tyron.at>
 * 
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * See COPYING for details
 */

using System;
using System.Collections;

namespace UsefulHeap.Zip {
	
	/// <summary>A collection that stores <see cref='UsefulHeap.Zip.ZipEntry'/> objects.</summary>
	/// <seealso cref='UsefulHeap.Zip.ZipEntryCollection'/>
	[Serializable()]
	public class ZipEntryCollection : CollectionBase {
		
		/// <summary>Initializes a new instance of <see cref='UsefulHeap.Zip.ZipEntryCollection'/>.</summary>
		public ZipEntryCollection() {
		}
		
		/// <summary>Initializes a new instance of <see cref='UsefulHeap.Zip.ZipEntryCollection'/> based on another <see cref='UsefulHeap.Zip.ZipEntryCollection'/>.</summary>
		/// <param name='value'>A <see cref='UsefulHeap.Zip.ZipEntryCollection'/> from which the contents are copied.</param>
		public ZipEntryCollection(ZipEntryCollection value) {
			this.AddRange(value);
		}
		
		/// <summary>Initializes a new instance of <see cref='UsefulHeap.Zip.ZipEntryCollection'/> containing any array of <see cref='UsefulHeap.Zip.ZipEntry'/> objects.</summary>
		/// <param name='value'>A array of <see cref='UsefulHeap.Zip.ZipEntry'/> objects with which to intialize the collection.</param>
		public ZipEntryCollection(ZipEntry[] value) {
			this.AddRange(value);
		}
		
		/// <summary>Represents the entry at the specified index of the <see cref='UsefulHeap.Zip.ZipEntry'/>.</summary>
		/// <param name='index'><para>The zero-based index of the entry to locate in the collection.</para></param>
		/// <value>
		///	<para>The entry at the specified index of the collection.</para>
		/// </value>
		/// <exception cref='System.ArgumentOutOfRangeException'><paramref name='index'/> is outside the valid range of indexes for the collection.</exception>
		public ZipEntry this[int index] {
			get {
				return ((ZipEntry)(List[index]));
			}
			set {
				List[index] = value;
			}
		}
		
		/// <summary>Adds a <see cref='UsefulHeap.Zip.ZipEntry'/> with the specified value to the <see cref='UsefulHeap.Zip.ZipEntryCollection'/>.</summary>
		/// <param name='value'>The <see cref='UsefulHeap.Zip.ZipEntry'/> to add.</param>
		/// <returns>The index at which the new element was inserted.</returns>
		/// <seealso cref='UsefulHeap.Zip.ZipEntryCollection.AddRange'/>
		public int Add(ZipEntry value) {
			return List.Add(value);
		}
		
		/// <summary>Copies the elements of an array to the end of the <see cref='UsefulHeap.Zip.ZipEntryCollection'/>.</summary>
		/// <param name='value'>An array of type <see cref='UsefulHeap.Zip.ZipEntry'/> containing the objects to add to the collection.</param>
		/// <returns>None.</returns>
		/// <seealso cref='UsefulHeap.Zip.ZipEntryCollection.Add'/>
		public void AddRange(ZipEntry[] value) {
			for (int i = 0; (i < value.Length); i = (i + 1)) {
				this.Add(value[i]);
			}
		}
		
		/// <summary>Adds the contents of another <see cref='UsefulHeap.Zip.ZipEntryCollection'/> to the end of the collection.</summary>
		/// <param name='value'>A <see cref='UsefulHeap.Zip.ZipEntryCollection'/> containing the objects to add to the collection.</param>
		/// <returns>None.</returns>
		/// <seealso cref='UsefulHeap.Zip.ZipEntryCollection.Add'/>
		public void AddRange(ZipEntryCollection value) {
			for (int i = 0; (i < value.Count); i = (i + 1)) {
				this.Add(value[i]);
			}
		}
		
		/// <summary>Gets a value indicating whether the <see cref='UsefulHeap.Zip.ZipEntryCollection'/> contains the specified <see cref='UsefulHeap.Zip.ZipEntry'/>.</summary>
		/// <param name='value'>The <see cref='UsefulHeap.Zip.ZipEntry'/> to locate.</param>
		/// <returns>
		/// <para><see langword='true'/> if the <see cref='UsefulHeap.Zip.ZipEntry'/> is contained in the collection; 
		///   otherwise, <see langword='false'/>.</para>
		/// </returns>
		/// <seealso cref='UsefulHeap.Zip.ZipEntryCollection.IndexOf'/>
		public bool Contains(ZipEntry value) {
			return List.Contains(value);
		}
		
		/// <summary>Copies the <see cref='UsefulHeap.Zip.ZipEntryCollection'/> values to a one-dimensional <see cref='System.Array'/> instance at the specified index.</summary>
		/// <param name='array'><para>The one-dimensional <see cref='System.Array'/> that is the destination of the values copied from <see cref='UsefulHeap.Zip.ZipEntryCollection'/> .</para></param>
		/// <param name='index'>The index in <paramref name='array'/> where copying begins.</param>
		/// <returns>None.</returns>
		/// <exception cref='System.ArgumentException'><para><paramref name='array'/> is multidimensional.</para> <para>-or-</para> <para>The number of elements in the <see cref='UsefulHeap.Zip.ZipEntryCollection'/> is greater than the available space between <paramref name='arrayIndex'/> and the end of <paramref name='array'/>.</para></exception>
		/// <exception cref='System.ArgumentNullException'><paramref name='array'/> is <see langword='null'/>. </exception>
		/// <exception cref='System.ArgumentOutOfRangeException'><paramref name='arrayIndex'/> is less than <paramref name='array'/>'s lowbound. </exception>
		/// <seealso cref='System.Array'/>
		public void CopyTo(ZipEntry[] array, int index) {
			List.CopyTo(array, index);
		}
		
		/// <summary>Returns the index of a <see cref='UsefulHeap.Zip.ZipEntry'/> in the <see cref='UsefulHeap.Zip.ZipEntryCollection'/>.</summary>
		/// <param name='value'>The <see cref='UsefulHeap.Zip.ZipEntry'/> to locate.</param>
		/// <returns>
		/// <para>The index of the <see cref='UsefulHeap.Zip.ZipEntry'/> of <paramref name='value'/> in the 
		/// <see cref='UsefulHeap.Zip.ZipEntryCollection'/>, if found; otherwise, -1.</para>
		/// </returns>
		/// <seealso cref='UsefulHeap.Zip.ZipEntryCollection.Contains'/>
		public int IndexOf(ZipEntry value) {
			return List.IndexOf(value);
		}
		
		/// <summary>Inserts a <see cref='UsefulHeap.Zip.ZipEntry'/> into the <see cref='UsefulHeap.Zip.ZipEntryCollection'/> at the specified index.</summary>
		/// <param name='index'>The zero-based index where <paramref name='value'/> should be inserted.</param>
		/// <param name=' value'>The <see cref='UsefulHeap.Zip.ZipEntry'/> to insert.</param>
		/// <returns><para>None.</para></returns>
		/// <seealso cref='UsefulHeap.Zip.ZipEntryCollection.Add'/>
		public void Insert(int index, ZipEntry value) {
			List.Insert(index, value);
		}
		
		/// <summary>Returns an enumerator that can iterate through the <see cref='UsefulHeap.Zip.ZipEntryCollection'/>.</summary>
		/// <returns><para>None.</para></returns>
		/// <seealso cref='System.Collections.IEnumerator'/>
		public new ZipEntryEnumerator GetEnumerator() {
			return new ZipEntryEnumerator(this);
		}
		
		/// <summary>Removes a specific <see cref='UsefulHeap.Zip.ZipEntry'/> from the <see cref='UsefulHeap.Zip.ZipEntryCollection'/>.</summary>
		/// <param name='value'>The <see cref='UsefulHeap.Zip.ZipEntry'/> to remove from the <see cref='UsefulHeap.Zip.ZipEntryCollection'/> .</param>
		/// <returns><para>None.</para></returns>
		/// <exception cref='System.ArgumentException'><paramref name='value'/> is not found in the Collection. </exception>
		public void Remove(ZipEntry value) {
			List.Remove(value);
		}
		
		/// <summary>Enumerator for <see cref="ZipEntryCollection"/>.</summary>
		public class ZipEntryEnumerator : object, IEnumerator {
			
			private IEnumerator baseEnumerator;
			
			private IEnumerable temp;
			
			/// <summary>Initializes a new instance of the <see cref="ZipEntryEnumerator"/> class.</summary>
			public ZipEntryEnumerator(ZipEntryCollection mappings) {
				this.temp = ((IEnumerable)(mappings));
				this.baseEnumerator = temp.GetEnumerator();
			}
			
			/// <summary>Gets the current entry.</summary>
			public ZipEntry Current {
				get {
					return ((ZipEntry)(baseEnumerator.Current));
				}
			}
			
			object IEnumerator.Current {
				get {
					return baseEnumerator.Current;
				}
			}
			

			/// <summary>Advance the enumerator to the next entry in the collection.</summary>
			/// <returns><c>true</c> if there are more entries; <c>false</c> if there are no more entires in the collection.</returns>
			public bool MoveNext() {
				return baseEnumerator.MoveNext();
			}
			
			bool IEnumerator.MoveNext() {
				return baseEnumerator.MoveNext();
			}

			/// <summary>Set the enumerator to just before the start of the collection.  Call <see cref="MoveNext"/> to advance to the first entry in the collection.</summary>
			public void Reset() {
				baseEnumerator.Reset();
			}
			
			void IEnumerator.Reset() {
				baseEnumerator.Reset();
			}
		}
	}
}
