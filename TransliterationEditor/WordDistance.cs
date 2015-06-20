using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oggy.TransliterationEditor
{
	static public class WordDistance
	{
		/// <summary>
		/// Calculates the distance between the words in terms of edits. A single edit is
		/// - Add char
		/// - Delete char
		/// - Change
		/// </summary>
		/// <param name="s1">The first string</param>
		/// <param name="s2">The second string</param>
		/// <returns>Distance between the strings in edits.</returns>
		public static int CalculateDistance(string s1, string s2)
		{
			if (s1 == null || s1.Length == 0)
				return s2.Length;
			if (s2 == null || s2.Length == 0)
				return s1.Length;

			int m = s1.Length;
			int n = s2.Length;

			int[,] matrix = new int[m, n];

			// The corner of the matrx
			matrix[0, 0] = s1[0] == s2[0] ? 0 : 1;

			// The second coordinate = 0
			bool found = s1[0] == s2[0];
			for (int i = 1; i < s1.Length; i++)
			{
				if (s1[i] == s2[0])
					found = true;
				matrix[i, 0] = i + (found ? 0 : 1);
			}

			// The first coordinate = 0
			found = s1[0] == s2[0];
			for (int i = 1; i < s2.Length; i++)
			{
				if (s2[i] == s1[0])
					found = true;
				matrix[0, i] = i + (found ? 0 : 1);
			}

			// The rest of the matrix
			for (int i = 1; i < m; i++)
				for (int j = 1; j < n; j++)
				{
					if (s1[i] == s2[j])
					{
						matrix[i, j] = min(
							matrix[i - 1, j - 1],
							matrix[i - 1, j] + 1,
							matrix[i, j - 1] + 1
							);
					}
					else
					{
						matrix[i,j] = min(
							matrix[i - 1, j - 1] + 1,
							matrix[i - 1, j] + 1,
							matrix[i,j-1] + 1
							);
					}
				}
			return matrix[m - 1, n - 1];
		}

		private static int min(int a, int b, int c)
		{
			return Math.Min(a, Math.Min(b, c));
		}
	}
}
