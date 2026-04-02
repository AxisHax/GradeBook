namespace GradeBook.Models
{
	/// <summary>
	/// Represents a student's grade, including the student's name and the score achieved.
	/// </summary>
	/// <remarks>
	///		Instances of this class are immutable and can be compared based on their score. This type is sealed
	///		and cannot be inherited.
	/// </remarks>
	public sealed class StudentGrade : IComparable<StudentGrade>
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="StudentGrade"/> class with the specified student name and score.
		/// </summary>
		/// <param name="studentName">The name of the student. Cannot be null, empty, or consist only of white-space characters.</param>
		/// <param name="score">The score achieved by the student. Must be greater than or equal to 0.0.</param>
		public StudentGrade(string studentName, double score)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(studentName, nameof(studentName));
			ArgumentOutOfRangeException.ThrowIfLessThan(score, 0.0, nameof(score));

			this.StudentName = studentName;
			this.Score = score;
		}
		#endregion //Constructors

		#region Public
		#region Properties
		/// <summary>Gets the score for the current instance.</summary>
		public double Score { get; }

		/// <summary>Gets the name of the student.</summary>
		public string StudentName { get; }
		#endregion //Properties

		#region Methods
		/// <inheritdoc/>
		public int CompareTo(StudentGrade? other)
		{
			if (other is null)
			{
				return 1;
			}

			return Score.CompareTo(other.Score);
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return $"{this.StudentName}:{this.Score}";
		}
		#endregion //Methods
		#endregion //Public
	}
}
