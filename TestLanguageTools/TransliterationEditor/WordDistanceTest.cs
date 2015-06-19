using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransliterationEditor;

namespace TestLanguageTools.TransliterationEditor
{
	[TestClass]
	public class WordDistanceTest
	{
		private void test(string s1, string s2, int distanceExpected)
		{
			int distanceActual = WordDistance.CalculateDistance(s1, s2);
			Assert.AreEqual(distanceExpected, distanceActual,
				String.Format("Distance('{0}','{1}')",
					s1, s2, distanceActual, distanceExpected
				));
		}

		private void test2(string s1, string s2, int distanceExpected)
		{
			test(s1, s2, distanceExpected);
			test(s2, s1, distanceExpected);
		}

		[TestMethod]
		public void DistanceTest()
		{
			test("", "", 0);
			test("abcd", "abcd", 0);

			test2("abc", "abbc", 1);
			test2("pun", "pjun", 1);
			test2("ognjen", "ognien", 1);

			test("abc", "b", 2);
			test("c", "abc", 2);

			test2("abc", "", 3);
			test2("", "xyz", 3);
			test2("abc", "xyz", 3);
		}
	}
}
