using Oggy.Transliterator.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Oggy;

namespace TestLanguageTools
{
    /// <summary>
    ///This is a test class for RuleTest and is intended
    ///to contain all RuleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RuleTest
    {
        /// <summary>
        ///A test for Rule Constructor
        ///</summary>
        [TestMethod()]
        public void RuleConstructorTest()
        {
            RuleCollection collection = new RuleCollection();
            collection.Add(new Rule("ate", "ejt", null, collection));
        }
    }
}
