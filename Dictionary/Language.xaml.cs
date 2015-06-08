using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Oggy.Repository.Entities;

namespace Dictionary
{
	public class StringWrapper
	{
		public string Value { get; set; }

		public StringWrapper(string s)
		{
			Value = s;
		}
		public StringWrapper()
		{
			Value = string.Empty;
		}
	}

	/// <summary>
	/// Interaction logic for Language.xaml
	/// </summary>
	public partial class LanguageWindow : Window
	{
		public enum UseLanguageWindow { CreateLanguage, UpdateLanguage };

		private bool loaded = false;
		private UseLanguageWindow usage;
		private readonly Brush redBrush = new SolidColorBrush(Colors.Red);
		private readonly Brush blackBrush = new SolidColorBrush(Colors.Black);
		List<StringWrapper> wordTypes;

		public LanguageWindow(UseLanguageWindow usage, Language language=null)
		{
			Title = usage == UseLanguageWindow.CreateLanguage ? "Add New Language" : "Update Language";

			this.usage = usage;
			InitializeComponent();
			if (language != null)
				LanguageToLanguageWindow(language);
			else
				wordTypes = new List<StringWrapper>();

			dataWordTypes.ItemsSource = wordTypes;
			loaded = true;
			bttnOK.IsEnabled = EnableButtonOK();
		}

		private void LanguageToLanguageWindow(Language language)
		{
			this.txtAlphabet.Text = language.Alphabet;
			this.txtCode.Text = language.Code;
			this.txtLanguage.Text = language.Name;
			this.dataWordTypes.ItemsSource = language.WordTypes;
			
			wordTypes = language.WordTypes != null ?
				language.WordTypes.Select(wt => new StringWrapper(wt)).ToList()
				: new List<StringWrapper>();
		}

		public void LanguageWindowToLanguage(Language language)
		{
			language.Alphabet = this.txtAlphabet.Text;
			language.Code = this.txtCode.Text;
			language.Name = this.txtLanguage.Text;
			language.WordTypes = new HashSet<string>(
				from x
				in wordTypes
				select x.Value);
		}

		#region Helper methods
		static public bool IsUpper(string text)
		{
			return text.All(c => char.IsUpper(c));
		}

		static public bool IsLower(string text)
		{
			return text.All(c => char.IsLower(c));
		}

		static private bool AllDifferent(string text)
		{
			HashSet<char> chars = new HashSet<char>();
			foreach (char c in text)
			{
				if (chars.Contains(c))
					return false;

				chars.Add(c);
			}
			return true;
		}
		#endregion

		private bool EnableButtonOK()
		{
			// Check language name
			lblLanguage.Foreground = redBrush;
			if (txtLanguage.Text.Length < 2)
				return false;
			if (!char.IsUpper(txtLanguage.Text[0]))
				return false;
			if (!IsLower(txtLanguage.Text.Substring(1)))
				return false;
			lblLanguage.Foreground = blackBrush;

			// Check code
			lblCode.Foreground = redBrush;
			if (txtCode.Text.Length != 2)
				return false;
			if (!IsUpper(txtCode.Text))
				return false;
			lblCode.Foreground  = blackBrush;

			// Check alphabet
			lblAlphabet.Foreground = redBrush;
			if (!IsLower(txtAlphabet.Text))
			{
				int selectionStart = txtAlphabet.SelectionStart;
				txtAlphabet.Text = txtAlphabet.Text.ToLower();
				txtAlphabet.SelectionStart = selectionStart;
			}
			if (!AllDifferent(txtAlphabet.Text))
				return false;
			lblAlphabet.Foreground = blackBrush;

			return true;
		}

		#region Handlers
		private void TextChanged(object sender, TextChangedEventArgs e)
		{
			if (!loaded)
				return;

			bttnOK.IsEnabled = EnableButtonOK();
		}

		private void bttnCancel_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
			this.Close();
		}

		private void bttnOK_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
			this.Close();
		}
		#endregion
	}
}
