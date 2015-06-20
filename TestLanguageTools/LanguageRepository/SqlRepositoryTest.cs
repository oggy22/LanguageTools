using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oggy.Repository;

namespace TestLanguageTools.LanguageRepository
{
	[TestClass]
	public class SqlRepositoryTest
	{
		[TestMethod]
		public void NormalizeTest()
		{
			Assert.AreEqual("", SqlRepository.Normalize(null));
			Assert.AreEqual("''", SqlRepository.Normalize("'"));
			Assert.AreEqual("''''", SqlRepository.Normalize("''"));
			Assert.AreEqual("it''s", SqlRepository.Normalize("it's"));
			Assert.AreEqual("what''s", SqlRepository.Normalize("what's"));
		}
	}
}
