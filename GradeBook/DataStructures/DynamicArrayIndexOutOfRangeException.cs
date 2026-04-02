namespace GradeBook.DataStructures
{
	/// <summary>
	///		Exception thrown when an index operation falls outside the valid range of a
	///		<see cref="DynamicArray{T}"/>.
	/// </summary>
	public class DynamicArrayIndexOutOfRangeException : Exception
	{
		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="attemptedIndex"></param>
		/// <param name="validRange"></param>
		public DynamicArrayIndexOutOfRangeException(int attemptedIndex, int validRange)
			: base($"Index {attemptedIndex} is out of range. Valid range is 0 to {validRange - 1}.")
		{
			this.AttemptedIndex = attemptedIndex;
			this.ValidRange = validRange;
		}
		#endregion //Constructors

		#region Public
		#region Properties
		public int AttemptedIndex { get; }

		public int ValidRange { get; }
		#endregion //Properties
		#endregion //Public
	}
}
