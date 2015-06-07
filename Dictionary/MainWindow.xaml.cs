using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Reflection;
using Oggy.Repository;
using Oggy.Repository.Entities;
using Oggy.Transliterator;
using System.Threading.Tasks;
using Google.Apis.Translate.v2;
using Google.Apis.Services;
using System.Diagnostics;

namespace Dictionary
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		ARepository repository;
		ListBox activeListBox = null;
		int listbox2To = 1; // either 1 or 3 representing listbox1 or listbox3 in which a word is relocated after listbox2
		string xmlFile, sqlConnectionString;
		const string settingsFile = "settings";
		Word activeWord = null;
		Comparison<Word> order = null;
		Transliterator transliterator = null;
		Dictionary.KeyboardForm keyboard;

		public MainWindow()
		{
			InitializeComponent();
			loadSettings();

			try
			{
				repository = new Oggy.Repository.XMLRepository(xmlFile);
				loadAllLanguages();

				// Create English if it doesn't exist and set it as the source language
				if (!repository.ListLanguages().Any(lang => lang.Code == "EN"))
				{
					repository.CreateLanguage(new Oggy.Repository.Entities.Language()
					{
						Code = "EN",
						Name = "English",
						Alphabet = "abcdefghijklmnopqrstuvwxyz",
						WordTypes = new HashSet<string>() { "noun", "verb", "adjective", "adverb"}
					});
				}
				repository.SetSourceLanguage("EN");

				loadLanguage();
			}
			catch (Exception exp)
			{
				MessageBox.Show("The application crashed because of the following exception: " + exp.ToString());
			}
		}

		private void saveSettings()
		{
			var writter = new StreamWriter(settingsFile);
			writter.WriteLine(xmlFile);
			writter.WriteLine(sqlConnectionString);
			writter.Close();
		}

		#region Load Routines and updateLabels
		private void loadSettings()
		{
			try
			{
				var reader = new StreamReader(settingsFile);
				xmlFile = reader.ReadLine();
				txtConnectionString.Text = sqlConnectionString = reader.ReadLine();
				reader.Close();
			}
			catch (FileNotFoundException) { }

			if (xmlFile == null)
				xmlFile = "Dictionary.xml";
		}

		private void loadLanguage()
		{
			loadWordTypes();
			loadWords();
			loadTransliterator();
			this.Title = "Dictionary - " + repository.srcLanguage.Code;
			if (keyboard!=null)
			{
				keyboard.SetLanguage(repository.srcLanguage.Code, repository.srcLanguage.Alphabet);
			}
		}

		private void loadAllLanguages()
		{
			menuLanguages.Items.Clear();

			foreach (Oggy.Repository.Entities.Language language in repository.ListLanguages())
			{
				MenuItem menuLang = new MenuItem()
				{
					Header = language.Code + " - " + language.Name
				};
				menuLang.Click += menuLang_Click;
				menuLanguages.Items.Add(menuLang);
			}

			MenuItem menuAddLanguage = new MenuItem()
			{
				Header = "Add New Language",
			};
			menuAddLanguage.Click += this.AddLanguage_Click;
			menuLanguages.Items.Add(menuAddLanguage);

			MenuItem menuUpdateLanguage = new MenuItem()
			{
				Header = "Edit Language",
			};
			menuUpdateLanguage.Click += this.EditLanguage_Click;
			menuLanguages.Items.Add(menuUpdateLanguage);
		}

		private void loadWordTypes()
		{
			comboWordTypes.Items.Clear();
			if (repository.srcLanguage.WordTypes == null)
				return;

			List<string> list = repository.srcLanguage.WordTypes.ToList();
			foreach (string s in list)
				comboWordTypes.Items.Add(s);
		}

		private void loadTransliterator()
		{
			Task.Run(() =>
			{
				transliterator = new Transliterator(new SqlRepository(), repository.srcLanguage.Code, "SR");

				txtTransliteration.Dispatcher.Invoke(
					 () => { txtTransliteration.Text = transliterator.Transliterate(txtWord.Text); });
			});
		}

		private void loadWords()
		{
			listBox1.Items.Clear();
			listBox2.Items.Clear();
			listBox3.Items.Clear();

			foreach (Word word in repository.ListWords())
			{
				switch (word.status)
				{
					case 1: listBox1.Items.Add(word);
						break;
					case 2: listBox2.Items.Add(word);
						break;
					case 3: listBox3.Items.Add(word);
						break;

					default: throw new KeyNotFoundException("The status=" + word.status + " not supported!");
				}
			}
			updateLabels();
		}

		private void updateLabels()
		{
			label1.Content = "Unknown(" + listBox1.Items.Count + ")";
			label2.Content = "Known(" + listBox2.Items.Count + ")";
			label3.Content = "Sure(" + listBox3.Items.Count + ")";
		}

		#endregion

		#region Menu
		void menuLang_Click(object sender, RoutedEventArgs e)
		{
			this.Title = "Dictionary: " + (sender as MenuItem).Header.ToString();
			string code = (sender as MenuItem).Header.ToString().Substring(0, 2);
			repository.SetSourceLanguage(code);
			loadLanguage();
		}

		private void sortListBox(ListBox listBox, Comparison<Word> comparer)
		{
			List<Word> words = new List<Word>();
			foreach (var item in listBox.Items)
			{
				words.Add(item as Word);
			}

			words.Sort(comparer);

			listBox.Items.Clear();
			foreach (var word in words)
				listBox.Items.Add(word);
		}

		private void Alphabetical_Click(object sender, RoutedEventArgs e)
		{
			order = (w1, w2) => w1.Name.CompareTo(w2.Name);
			sortListBox(listBox1, order);
			sortListBox(listBox2, order);
			sortListBox(listBox3, order);
		}

		private void Created_Click(object sender, RoutedEventArgs e)
		{
			order = (w1, w2) => w2.Created.CompareTo(w1.Created);
			sortListBox(listBox1, order);
			sortListBox(listBox2, order);
			sortListBox(listBox3, order);
		}

		private void Modified_Click(object sender, RoutedEventArgs e)
		{
			order = (w1, w2) => w2.Modified.CompareTo(w1.Modified);
			sortListBox(listBox1, order);
			sortListBox(listBox2, order);
			sortListBox(listBox3, order);
		}

		private void WordTypeItem_Click(object sender, RoutedEventArgs e)
		{
			order = (w1, w2) => w2.Type.CompareTo(w1.Type);
			sortListBox(listBox1, order);
			sortListBox(listBox2, order);
			sortListBox(listBox3, order);
		}

		private void XMLRepository_Click(object sender, RoutedEventArgs e)
		{
			var openFileDialog = new Microsoft.Win32.OpenFileDialog();
			openFileDialog.DefaultExt = ".xml";
			openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
			openFileDialog.FileName = xmlFile;
			if (openFileDialog.ShowDialog().Value)
			{
				repository = new XMLRepository(openFileDialog.FileName);
				loadLanguage();
			}
		}

		private void SQLRepository_Click(object sender, RoutedEventArgs e)
		{
			repository = new SqlRepository(SqlRepository.CONNECTION_STRING);
			loadAllLanguages();
			loadLanguage();
		}

		private void Exit_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void About_Click(object sender, RoutedEventArgs e)
		{
			var assembly = System.Reflection.Assembly.GetExecutingAssembly().GetName();

			string aboutText = assembly.Name + "\n";
			aboutText += "by Ognjen Sobajic\n";
			aboutText += "Version = " + assembly.Version;

			MessageBox.Show(aboutText, "Dictionary");
		}

		private string googleTranslation;

		private void GoogleTranslate_Click(object sender, RoutedEventArgs e)
		{
			txtTranslation.Text = new string('.', txtWord.Text.Length);

			string text = txtWord.Text;
			Task.Run(() =>
			{
				var service = new TranslateService(new BaseClientService.Initializer()
				{
					ApiKey = "AIzaSyBLM8SA9YVEDjhx3FoQsedcFlftm1sefBc"
				});

				var response = service.Translations.List(text, "sr").Execute();
				googleTranslation = response.Translations[0].TranslatedText;
				txtTranslation.Dispatcher.Invoke(
					 () => { txtTranslation.Text = googleTranslation; });
			});
		}


		#endregion

		#region Buttons Click routines: Add, Edit, Delete
		private void Add_Click(object sender, RoutedEventArgs e)
		{
			Word word = new Word()
			{
				Name = txtWord.Text,
				Translation = txtTranslation.Text,
				Type = comboWordTypes.SelectedItem as string,
				status = 1
			};
			repository.CreateWord(word);
			listBox1.Items.Add(word);
			updateLabels();
			txtWord.Text = string.Empty;
		}

		private void Edit_Click(object sender, RoutedEventArgs e)
		{
			activeWord.Translation = txtTranslation.Text;
			activeWord.Type = comboWordTypes.SelectedItem as string;
			repository.UpdateWord(activeWord);
			bttnEdit.IsEnabled = false;
		}

		private void Delete_Click(object sender, RoutedEventArgs e)
		{
			Word word = activeListBox.SelectedItem as Word;
			activeListBox.Items.Remove(word);
			repository.DeleteWord(word.Name);
			updateLabels();
			txtWord.Text = string.Empty;
		}
		#endregion

		#region listBox_Selected
		private void listBoxSelected(Word word)
		{
			if (word == null)
				return;

			this.txtWord.Text = word.Name;
			this.txtTranslation.Text = word.Translation;
			this.comboWordTypes.SelectedItem = word.Type;
			if (word.Type == string.Empty)
				this.comboWordTypes.SelectedIndex = -1;
			bttnDelete.IsEnabled = true;
		}

		private void listBox1_Selected(object sender, RoutedEventArgs e)
		{
			activeWord = listBox1.SelectedItem as Word;
			listBoxSelected(listBox1.SelectedItem as Word);
			activeListBox = listBox1;
			listbox2To = 1;
		}

		private void listBox2_Selected(object sender, RoutedEventArgs e)
		{
			activeWord = listBox2.SelectedItem as Word;
			listBoxSelected(listBox2.SelectedItem as Word);
			activeListBox = listBox2;
		}

		private void listBox3_Selected(object sender, RoutedEventArgs e)
		{
			activeWord = listBox3.SelectedItem as Word;
			listBoxSelected(listBox3.SelectedItem as Word);
			activeListBox = listBox3;
			listbox2To = 3;
		}
		#endregion listBox_Selected

		#region ListBox_DoubleClick
		private void AddToListBox(ListBox listBox, Word word)
		{
			int index;
			for (index = 0; index < listBox.Items.Count && order(word, listBox.Items[index] as Word) > 0; index++) ;
			listBox.Items.Insert(index, word);
		}

		private void listBox_DoubleClick(ListBox lbFrom, ListBox lbTo, int newStatus)
		{
			Word word = lbFrom.SelectedItem as Word;
			if (word == null)
				return;

			word.status = newStatus;
			word.Modified = DateTime.Now;

			// Move the word into lbTo
			lbFrom.Items.Remove(word);
			if (order == null)
				lbTo.Items.Add(word);
			else
				AddToListBox(lbTo, word);

			repository.UpdateWord(word);

			updateLabels();
		}

		private void listBox1_DoubleClick(object sender, MouseButtonEventArgs e)
		{
			listBox_DoubleClick(listBox1, listBox2, 2);
		}

		private void listBox2_DoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (listbox2To == 1)
			{
				listBox_DoubleClick(listBox2, listBox1, 1);
			}
			else if (listbox2To == 3)
			{
				listBox_DoubleClick(listBox2, listBox3, 3);
			}
		}

		private void listBox3_DoubleClick(object sender, MouseButtonEventArgs e)
		{
			listBox_DoubleClick(listBox3, listBox2, 2);
		}
		#endregion ListBox_DoubleClick

		#region ListBox_MouseDown
		private void listBox1_MouseDown(object sender, MouseButtonEventArgs e)
		{
			listbox2To = 1;
			listBox1_Selected(null, null);
		}

		private void listBox2_MouseDown(object sender, MouseButtonEventArgs e)
		{
			listBox2_Selected(null, null);
		}

		private void listBox3_MouseDown(object sender, MouseButtonEventArgs e)
		{
			listbox2To = 3;
			listBox3_Selected(null, null);
		}
		#endregion ListBox_MouseDown

		private IEnumerable<Word> allWords()
		{
			return listBox1.Items.Cast<Word>()
				 .Concat(listBox2.Items.Cast<Word>())
				 .Concat(listBox3.Items.Cast<Word>());
		}

		private void txtWord_Changed(object sender, TextChangedEventArgs e)
		{
			if (txtWord.Text.Length == 0)
				txtTranslation.Text = string.Empty;

			bttnAdd.IsEnabled =
				 txtWord.Text != string.Empty &&                     // TextBox is empty
				 !allWords().Any(word => word.Name == txtWord.Text); // There is no txtWord.Text in the listBoxes

			bttnEdit.IsEnabled =
				 activeWord != null
				 && (txtTranslation.Text != activeWord.Translation || txtWord.Text != activeWord.Name);

			if (transliterator != null)
				txtTransliteration.Text = transliterator.Transliterate(txtWord.Text);
			else
				txtTransliteration.Text = new string('.', txtWord.Text.Length);
		}

		private void txtTranslation_Changed(object sender, TextChangedEventArgs e)
		{
			bttnEdit.IsEnabled =
				 activeWord != null
				 && (txtTranslation.Text != activeWord.Translation || txtWord.Text != activeWord.Name);
		}

		private void refreshListBoxes(ListBox listBox)
		{
			List<Word> list = new List<Word>();

			// Move the words into a List
			while (listBox.Items.Count > 0)
			{
				list.Add(listBox.Items[0] as Word);
				listBox.Items.RemoveAt(0);
			}

			// Move the words back to the listbox
			foreach (Word word in list)
			{
				listBox.Items.Add(word);
			}
		}

		#region Word Menu Item
		private void refreshListBoxes()
		{
			refreshListBoxes(listBox1);
			refreshListBoxes(listBox2);
			refreshListBoxes(listBox3);
		}

		private void WordItem_Click(object sender, RoutedEventArgs e)
		{
			Word.toStringHandler = (Word word) => { return word.Name; };
			refreshListBoxes();
		}

		private void WordAndTranslation_Click(object sender, RoutedEventArgs e)
		{
			Word.toStringHandler = (Word word) => { return word.Name + " - " + word.Translation; };
			refreshListBoxes();
		}

		private void Translation_Click(object sender, RoutedEventArgs e)
		{
			Word.toStringHandler = (Word word) => { return word.Translation; };
			refreshListBoxes();
		}
		#endregion

		private void AddLanguage_Click(object o, RoutedEventArgs e)
		{
			Dictionary.LanguageWindow languageWindow = new LanguageWindow(Dictionary.LanguageWindow.UseLanguageWindow.CreateLanguage);
			Oggy.Repository.Entities.Language language = new Language();
			while (languageWindow.ShowDialog().Value)
			{
				languageWindow.LanguageWindowToLanguage(language);
				if (repository.CreateLanguage(language))
					break;
			}
			repository.SetSourceLanguage(language.Code);
			loadAllLanguages();
			loadLanguage();
		}

		private void EditLanguage_Click(object o, RoutedEventArgs e)
		{
			Dictionary.LanguageWindow languageWindow = new LanguageWindow(
				Dictionary.LanguageWindow.UseLanguageWindow.UpdateLanguage,
				repository.srcLanguage
				);
			
			if (!languageWindow.ShowDialog().Value)
				return;

			languageWindow.LanguageWindowToLanguage(repository.srcLanguage);
			languageWindow.Close();
			repository.UpdateLanguage(repository.srcLanguage);
			loadAllLanguages();
			loadLanguage();
		}

		private void DateCreated_Click(object sender, RoutedEventArgs e)
		{
			Word.ShowDateCreated = !Word.ShowDateCreated;
			refreshListBoxes();
		}

		private void DateModified_Click(object sender, RoutedEventArgs e)
		{
			Word.ShowDateModified = !Word.ShowDateModified;
			refreshListBoxes();
		}

		private void comboWordTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			bttnEdit.IsEnabled = activeWord != null && activeWord.Type != (comboWordTypes.SelectedValue ?? string.Empty).ToString();
		}

		private void Keyboard_Click(object sender, RoutedEventArgs e)
		{
			lock (this)
			{
				if (keyboard == null)
				{
					var language = repository.srcLanguage;
					keyboard = new KeyboardForm(this.txtWord, language.Code, language.Alphabet);
					keyboard.Show();
				}
				else
				{
					keyboard.Dispose();
					keyboard = null;
				}
			}
		}
	}
}