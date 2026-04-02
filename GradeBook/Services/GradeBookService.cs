using GradeBook.DataStructures;
using GradeBook.Models;
namespace GradeBook.Services
{
	/// <summary>
	/// Provides functionality for managing a collection of student grades, including adding grades, sorting, searching,
	/// and calculating statistics such as average, highest, and lowest scores.
	/// </summary>
	public sealed class GradeBookService
	{
		#region Constructors
		/// <summary>Initializes a new instance of the <see cref="GradeBookService"/> class.</summary>
		public GradeBookService()
		{
			this.grades = new DynamicArray<StudentGrade>();
			this.isSorted = false;
		}
		#endregion //Constructors

		#region Public
		#region Methods
		/// <summary>Adds a new <see cref="StudentGrade"/> entry.</summary>
		/// <param name="studentName">The name of the student for whom the grade is being added. Cannot be null or empty.</param>
		/// <param name="score">The grade score to assign to the student. Typically expected to be within the valid grading range.</param>
		public void AddGrade(string studentName, double score)
		{
			this.grades.Add(new StudentGrade(studentName, score));
			this.isSorted = false;
		}

		/// <summary>Finds the first student grade that matches the specified score.</summary>
		/// <remarks>
		///		If the collection is sorted, a binary search is used for improved performance. Otherwise, a
		///		linear search is performed.
		/// </remarks>
		/// <param name="score">The score to search for within the collection of student grades.</param>
		/// <returns>
		///		A <see cref="StudentGrade"/> object representing the first grade with the specified score, or <see
		///		langword="null"/> if no matching grade is found.
		/// </returns>
		public StudentGrade? FindByScore(double score)
		{
			var target = new StudentGrade("search", score);
			int index = this.isSorted
				? ((DynamicArray<StudentGrade>)this.grades).BinarySearch(target)
				: this.grades.IndexOf(target);

			return index >= 0 ? this.grades.Get(index) : null;
		}

		/// <summary>Calculates the average score of all grades in the gradebook.</summary>
		/// <returns>The arithmetic mean of the scores for all grades in the gradebook.</returns>
		/// <exception cref="InvalidOperationException">Thrown if the gradebook contains no grades.</exception>
		public double GetAverage()
		{
			if (this.grades.Count == 0)
			{
				throw new InvalidOperationException("Cannot compute the average of an empty gradebook.");
			}

			double sum = 0;

			foreach (var grade in this.grades)
			{
				sum += grade.Score;
			}

			return sum / this.grades.Count;
		}

		/// <summary>Retrieves the student grade with the highest grade.</summary>
		/// <returns>The highest student grade in the collection.</returns>
		public StudentGrade GetHighest()
		{
			this.EnsureSorted();

			return this.grades.Get(this.grades.Count - 1);
		}

		/// <summary>Retrieves the student grade with the lowest grade.</summary>
		/// <returns>The lowest student grade in the collection.</returns>
		public StudentGrade GetLowest()
		{
			this.EnsureSorted();

			return this.grades.Get(0);
		}

		/// <summary>Sorts the collection of student grades in ascending order.</summary>
		public void Sort()
		{
			((DynamicArray<StudentGrade>)this.grades).InsertionSort();
			this.isSorted = true;
		}
		#endregion //Methods
		#endregion //Public

		#region Private
		#region Members
		/// <summary>Represents the collection of student grades managed by the class.</summary>
		private readonly IDynamicArray<StudentGrade> grades;

		/// <summary>Represents whether or not the underlying collection is sorted.</summary>
		private bool isSorted;
		#endregion //Members

		#region Methods
		/// <summary>Ensures that the grades collection is sorted before proceeding.</summary>
		/// <remarks>
		///		This method is intended to be used internally to verify that the grades are in sorted order. It
		///		should be called before performing operations that require sorted data.
		/// </remarks>
		/// <exception cref="InvalidOperationException">Thrown if the grades collection is not empty and has not been sorted. Call Sort() before invoking this method.</exception>
		private void EnsureSorted()
		{
			if (!(this.grades.Count == 0) && !this.isSorted)
			{
				throw new InvalidOperationException("Grades must be sorted before calling this method. Call Sort() first.");
			}
		}
		#endregion //Methods
		#endregion //Private
	}
}
