using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Oggy.Transliterator;
using Oggy.Repository;
using Oggy.Repository.Entities;
using System.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TransliterationEditor.BusinessCollections;

namespace TransliterationEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables

        private Transliterator transliterator;
        private SqlRepository repository;
        Language sourceLanguage = null;
        Language destinationLanguage = null;
		RuleCollection rules;

		#endregion

        #region Methods
        
        public MainWindow()
        {
            BindingErrorTraceListener.SetTrace();
            InitializeComponent();

            repository = new SqlRepository();

            repository.SetSourceLanguage("EN");
            repository.SetDstLanguage("SR");

            sourceLanguage = repository.RetreiveLanguage("EN");
            destinationLanguage = repository.RetreiveLanguage("SR");
            
            ReloadEverything();

            var languages = repository.ListLanguages();

            foreach (var language in languages)
            {
                sourceLanguageCombo.Items.Add(language.Code + " - " + language.Name);
                destinationLanguageCombo.Items.Add(language.Code + " - " + language.Name);
            }

            sourceLanguageCombo.SelectedValue = sourceLanguage.Code + " - " + sourceLanguage.Name;
            destinationLanguageCombo.SelectedValue = destinationLanguage.Code + " - " + destinationLanguage.Name;

            txtSource.Text = string.Empty;
            triggerReloadForCombos = true;
        }

        private void ReloadEverything()
        {
            // Set the caption
            string caption = "Transliterator Editor ";
            if (sourceLanguage != null)
                caption += sourceLanguage.Code;
            if (destinationLanguage != null)
                caption += " -> " + destinationLanguage.Code;
            Title = caption;

            // Load rules
			rules = new RuleCollection(repository);
			dataGridRules.ItemsSource = rules;
            transliterator = new Transliterator(repository, sourceLanguage.Code, destinationLanguage.Code);
			TransliterationExample.Transliterator = (s => transliterator.Transliterate(s, false));
			TransliterationExample.Trans_li_te_rator = (s => transliterator.Transliterate(s, true));
			rules.MyTransliterator = transliterator;

            // Load TransliterationExamples
            examplesCollection = new ExampleCollection(repository);
            dataGridExamples.ItemsSource = examplesCollection;

			TransliterationExample.DestinationFunc = Oggy.TransliterationEditor.WordDistance.CalculateDistance;
			TextBox_TextChanged(null, null);
        }

        private ExampleCollection examplesCollection;

        #endregion

        #region Handlers

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTranslireation();
        }

        private void UpdateTranslireation()
        {
			  txtDestination.Text = transliterator.Transliterate(txtSource.Text, this.checkWithDashes.IsChecked.Value);
        }

        private bool triggerReloadForCombos = false;
        
        private void SourceLanguage_Change(object sender, SelectionChangedEventArgs e)
        {
            if (!triggerReloadForCombos)
                return;
            string selectedCode = (((ComboBox)(sender)).SelectedValue as string).Substring(0, 2);
            sourceLanguage = repository.RetreiveLanguage(selectedCode);
            repository.SetSourceLanguage(selectedCode);
            ReloadEverything();
        }

        private void DestinationLanguage_Change(object sender, SelectionChangedEventArgs e)
        {
            if (!triggerReloadForCombos)
                return;
            string selectedCode = (((ComboBox)(sender)).SelectedValue as string).Substring(0, 2);
            destinationLanguage = repository.RetreiveLanguage(selectedCode);
            repository.SetDstLanguage(selectedCode);
            ReloadEverything();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dataGridExamples.ItemsSource = null;
            dataGridExamples.ItemsSource = examplesCollection;
            TextBox_TextChanged(null, null);

            int correct = examplesCollection.Count(example => example.Correct);
            int total = examplesCollection.Count;
			int incorrect = total - correct;
            MessageBox.Show("There are " + correct + "/" + total + " correct/total examples ratio.\n" + (total-correct) + " incorrect examples (" +  (int)(100 * (double)(incorrect)/total) +"%)");
        }

        private void checkWithDashes_Click(object sender, RoutedEventArgs e)
        {
			  UpdateTranslireation();
        }

        private void AddExamples_Click(object sender, RoutedEventArgs e)
        {
            RuleCollection rules = (RuleCollection)(dataGridRules.ItemsSource);

            List<string> words = new List<string>();
            foreach (var rule in rules)
            {
                if (rule.Examples != null)
                    words.AddRange(rule.Examples.Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries));
                if (rule.CounterExamples != null)
                    words.AddRange(rule.CounterExamples.Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries));
            }

            ExampleCollection examples = (ExampleCollection)(dataGridExamples.ItemsSource);
            var wordsInExamples = examples.Select(example => example.Source);

            // Show the dialog
            var addExamplesWindow = new AddExamples(words.Distinct().Except(wordsInExamples), examples);
            addExamplesWindow.ShowDialog();
        }

        private void ReverseTransliteration_Click(object sender, RoutedEventArgs e)
        {
            repository.SwapSourceAndDestination();
            Transliterator reverseTransliterator = new Transliterator(repository, repository.srcLanguage.Code, repository.dstLanguage.Code);
            RuleCollection ruleCollection = (RuleCollection)(dataGridRules.ItemsSource);
            RuleCollection newRuleCollection = new RuleCollection();

            var rules = repository.ListRules(true);

            foreach (var rule in rules)
            {
                if (rule.Destination.Length == 0)
                    continue;

                string newSource = rule.Destination, newDestination = rule.Source;

                if (newDestination.StartsWith("|"))
                {
                    newSource = "|" + newSource;
                    newDestination = newDestination.Substring(1);
                }

                if (newDestination.EndsWith("|"))
                {
                    newSource = newSource + "|";
                    newDestination = newDestination.Substring(0, newDestination.Length - 1);
                }

                if (!ruleCollection.All(r => newSource != r.Source))
                    continue;

                newRuleCollection.Add(new TransliterationRule(newSource,
                    newDestination,
                    reverseTransliterator.Transliterate(rule.Examples)
                    ));
            }

            // Show the dialog
            RuleCollection oldRules = (RuleCollection)(dataGridRules.ItemsSource);
            var reverseTransliterationWindow = new ReverseTransliteration(newRuleCollection, oldRules);
            reverseTransliterationWindow.ShowDialog();

            repository.SwapSourceAndDestination();
        }

        private void SwitchLanguages_Click(object sender, RoutedEventArgs e)
        {
            // Swap languages
            Language temp = sourceLanguage;
            sourceLanguage = destinationLanguage;
            destinationLanguage = temp;

            repository.srcLanguage = sourceLanguage;
            repository.dstLanguage = destinationLanguage;

			triggerReloadForCombos = false;
			int sourceIndex = sourceLanguageCombo.SelectedIndex;
			sourceLanguageCombo.SelectedIndex = destinationLanguageCombo.SelectedIndex;
			destinationLanguageCombo.SelectedIndex = sourceIndex;

			// Reload everthing
			triggerReloadForCombos = true;
			ReloadEverything();
		}

		private void ReloadLanguages_Click(object sender, RoutedEventArgs e)
		{
			triggerReloadForCombos = false;

			// Reload everthing
			triggerReloadForCombos = true;
			ReloadEverything();
		}

		private void txtRulesFilter_TextChanged(object sender, TextChangedEventArgs e)
		{
			CollectionViewSource.GetDefaultView(rules).Filter = obj =>
			{
				return (obj as TransliterationRule).Contains(txtRulesFilter.Text);
			};
		}

		private void txtExamplesFilter_TextChanged(object sender, TextChangedEventArgs e)
		{
			CollectionViewSource.GetDefaultView(examplesCollection).Filter = obj =>
			{
				return (obj as TransliterationExample).Contains(txtExamplesFilter.Text);
			};
		}

		#endregion
	}
}