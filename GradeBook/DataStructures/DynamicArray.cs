using System.Collections;

namespace GradeBook.DataStructures
{
	public class DynamicArray<T> : IDynamicArray<T> where T : IComparable<T>
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="DynamicArray{T}"/> class with the default capacity.
		/// </summary>
		public DynamicArray() : this(DefaultCapacity)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DynamicArray{T}"/> class with the specified initial capacity.
		/// </summary>
		/// <param name="initialCapacity">The number of elements that the array can initially store. Must be at least 1.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="initialCapacity"/> is less than 1.</exception>
		public DynamicArray(int initialCapacity)
		{
			if (initialCapacity < 1)
			{
				throw new ArgumentOutOfRangeException(
					nameof(initialCapacity), $"{nameof(initialCapacity)} must be at least 1.");
			}

			this.data = new T[initialCapacity];
			this.count = 0;
		}
		#endregion //Constructors

		#region Public
		#region Properties
		/// <inheritdoc/>
		public int Capacity => this.data.Length;

		/// <inheritdoc/>
		public int Count => this.count;
		#endregion //Properties

		#region Methods
		/// <inheritdoc/>
		public void Add(T item)
		{
			this.EnsureCapacity();

			this.data[this.count++] = item;
		}

		/// <summary>
		/// Searches the collection for the specified item using binary search. The collection
		/// MUST be sorted in ascending order; otherwise, behavior is undefined.
		/// </summary>
		/// <param name="item">The item to search for in the collection.</param>
		/// <returns>The zero-based index of <paramref name="item"/>, or -1 if not found.</returns>
		public int BinarySearch(T item)
		{
			int low = 0;
			int high = this.count - 1;

			while (low <= high)
			{
				// Protection from integer overflow.
				int midpoint = low + (high - low) / 2;
				int comparison = this.data[midpoint].CompareTo(item);

				if (comparison == 0)
				{
					return midpoint;
				}
				else if (comparison < 0)
				{
					low = midpoint + 1;
				}
				else
				{
					high = midpoint - 1;
				}
			}

			return -1;
		}

		/// <inheritdoc/>
		public void Clear()
		{
			// Clear references to allow for garbage collection.
			Array.Clear(this.data, 0, this.count);
			this.count = 0;
		}

		/// <inheritdoc/>
		public bool Contains(T item)
		{
			var comparer = EqualityComparer<T>.Default;

			for (int i = 0; i < this.count; ++i)
			{
				if (comparer.Equals(this.data[i], item))
				{
					return true;
				}
			}

			return false;
		}

		/// <inheritdoc/>
		public T Get(int index)
		{
			this.GuardIndex(index);

			return this.data[index];
		}

		/// <inheritdoc/>
		public IEnumerator<T> GetEnumerator()
		{
			for (int i = 0; i < this.count; i++)
			{
				yield return this.data[i];
			}
		}

		/// <summary>Searches for the first occurrence of <paramref name="item"/> using linear search.</summary>
		/// <param name="item">The item to search for in the collection.</param>
		/// <returns>The zero-based index of the item if it's found in the collection, -1 if not</returns>
		/// <remarks>
		///		Callers should use <see cref="IDynamicArray{T}.Contains(T)"/> if only checking
		///		if the element is in the collection is needed.
		/// </remarks>
		public int IndexOf(T item)
		{
			var comparer = EqualityComparer<T>.Default;

			for (int i = 0; i < this.count; ++i)
			{
				if (comparer.Equals(this.data[i], item))
				{
					return i;
				}
			}

			return -1;
		}

		/// <inheritdoc/>
		public void InsertAt(int index, T item)
		{
			// Allow inserting at Count (equivalent to add).
			if (index < 0 || index > this.count)
			{
				throw new DynamicArrayIndexOutOfRangeException(index, this.count + 1);
			}

			this.EnsureCapacity();

			// Shift all elements to the right to open a slot.
			for (int i = this.count; i > index; i--)
			{
				this.data[i] = this.data[i - 1];
			}

			this.data[index] = item;
			this.count++;
		}

		/// <summary>Sorts the collection in ascending order using insertion sort.</summary>
		public void InsertionSort()
		{
			for (int i = 1; i < this.count; i++)
			{
				T key = this.data[i];
				int j = i - 1;

				// Shift elements greater than key one position to the right.
				while (j >= 0 && this.data[j].CompareTo(key) > 0)
				{
					this.data[j + 1] = this.data[j];
					j--;
				}

				this.data[j + 1] = key;
			}
		}

		/// <inheritdoc/>
		public void RemoveAt(int index)
		{
			this.GuardIndex(index);

			// Shift elements left to fill the gap.
			for (int i = index; i < this.count - 1; i++)
			{
				this.data[i] = this.data[i + 1];
			}

			// Clear the last slot to release the reference (important for Garbage
			// Collector with reference types.
			// What's Happening?
			//		Set the element at the current index to a default value (can be null, hence the null forgiving operator)
			//		and then decrement count.
			this.data[--this.count] = default!;
		}
		#endregion //Methods
		#endregion //Public

		#region Private
		#region Constants
		/// <summary>Specifies the default initial capacity for the collection when no capacity is specified.</summary>
		private const int DefaultCapacity = 4;

		/// <summary>Specifies the factor by which the capacity increases when resizing is required.</summary>
		private const int GrowthFactor = 2;
		#endregion //Constants

		#region Members
		/// <summary>The amount of valid items contained in the array.</summary>
		private int count;

		/// <summary>The data contained in by the collection.</summary>
		private T[] data;
		#endregion //Members

		#region Methods
		/// <summary>Ensures that the underlying data array has sufficient capacity to store additional elements.</summary>
		/// <remarks>
		///		If the current capacity is insufficient, the method increases the size of the internal array to
		///		accommodate more elements. Call this before adding new items to the collection to prevent
		///		index out of range errors.
		/// </remarks>
		private void EnsureCapacity()
		{
			// Check if the current capacity is enough.
			if (this.count < this.data.Length)
			{
				return;
			}

			// Allocate more data for the collection.
			var newData = new T[this.data.Length * GrowthFactor];
			Array.Copy(this.data, newData, this.count);

			this.data = newData;
		}

		/// <summary>Validates that the specified index is within the valid range for the collection.</summary>
		/// <param name="index">The zero-based index to validate. Must be greater than or equal to 0 and less than the current count.</param>
		/// <exception cref="DynamicArrayIndexOutOfRangeException">Thrown if index is less than 0 or greater than or equal to the number of elements in the collection.</exception>
		private void GuardIndex(int index)
		{
			if (index < 0 || index >= this.count)
			{
				throw new DynamicArrayIndexOutOfRangeException(index, this.count);
			}
		}

		IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
		#endregion //Methods
		#endregion //Private
	}
}
