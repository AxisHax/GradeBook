namespace GradeBook
{
    /// <summary> Defines the contract for a generic resizeable array.</summary>
    /// <typeparam name="T"></typeparam>
    public interface IDynamicArray<T> : IEnumerable<T>
    {
        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the currently allocated capacity.
        /// </summary>
        int Capacity { get; }

        /// <summary>
        /// Adds the specified item to the collection.
        /// </summary>
        /// <param name="item">The item to add to the collection.</param>
        void Add(T item);

        /// <summary>
        /// Inserts an item into the collection at the specified index, shifting elements to the right.
        /// </summary>
        /// <remarks>
        ///     Subsequent items are shifted to accommodate the new item. If the index is equal to
        ///     the number of items, the item is added to the end of the collection.
        /// </remarks>
        /// <param name="index">
        ///     The zero-based index at which the item should be inserted. Must be greater than or equal to 0 and less than
        ///     or equal to the number of items in the collection.
        /// </param>
        /// <param name="item">The item to insert into the collection.</param>
        void InsertAt(int index, T item);

        /// <summary>
        /// Removes the element at the specified index of the collection, shifting elements to the left.
        /// </summary>
        /// <param name="index">
        ///     The zero-based index of the element to remove. Must be greater than or equal to 0 and less than the number
        ///     of elements in the collection.
        /// </param>
        void RemoveAt(int index);

        /// <summary>
        /// Gets the element at the specified zero-based index.
        /// </summary>
        /// <param name="index">
        ///     The zero-based index of the element to retrieve. Must be greater than or equal to 0 and less than the total
        ///     number of elements.
        /// </param>
        /// <returns>The element of type T at the specified index.</returns>
        T Get(int index);

        void Clear();
    }
}
