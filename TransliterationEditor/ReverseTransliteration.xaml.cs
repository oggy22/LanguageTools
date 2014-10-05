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
using System.Windows.Shapes;
using TransliterationEditor.BusinessCollections;
using Oggy.Transliterator;

namespace TransliterationEditor
{
    /// <summary>
    /// Interaction logic for ReverseTransliteration.xaml
    /// </summary>
    public partial class ReverseTransliteration : Window
    {
        RuleCollection oldRules;
        
        public ReverseTransliteration()
        {
            InitializeComponent();
        }

        public ReverseTransliteration(RuleCollection rules, RuleCollection oldRules)
            : this()
        {
            this.oldRules = oldRules;
            dataGridExamples.ItemsSource = rules;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            RuleCollection newRules = (RuleCollection)(dataGridExamples.ItemsSource);
            
            foreach (var rule in newRules)
                oldRules.Add(rule);
            
            this.DialogResult = true;
            this.Close();
        }
    }
}