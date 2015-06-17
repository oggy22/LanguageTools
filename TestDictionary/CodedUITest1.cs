using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;


namespace TestDictionary
{
	/// <summary>
	/// Summary description for CodedUITest1
	/// </summary>
	[CodedUITest]
	public class CodedUITest1
	{
		ApplicationUnderTest app;

		public CodedUITest1()
		{
		}

		[TestMethod]
		public void CodedUITestMethod1()
		{
			string st = string.Empty;
			foreach (var control in app.GetChildren())
			{
				st += control.Name + "," + control.ClassName + " ";
				if (control.ClassName == "Uia.Button")
				{
					UITestControl ui;
					//Microsoft.VisualStudio.TestTools.UITesting
					//Button button = control as Button;
				}
			}
			var st2 = app.GetChildren().ToString();
		}

		#region Additional test attributes

		// You can use the following additional attributes as you write your tests:

		//Use TestInitialize to run code before running each test 
		[TestInitialize()]
		public void MyTestInitialize()
		{
			app = ApplicationUnderTest.Launch(@"D:\c#\LanguageTools\Dictionary\bin\Debug\Dictionary.exe");
			//UITestControl buttonAdd = app.
		}

		//Use TestCleanup to run code after each test has run
		[TestCleanup()]
		public void MyTestCleanup()
		{
			app.Close();
		}

		#endregion

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}
		private TestContext testContextInstance;
	}
}
