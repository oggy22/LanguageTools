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
using Oggy.Repository.Entities;

namespace TransliterationEditor
{
    /// <summary>
    /// Interaction logic for AddExamples.xaml
    /// </summary>
    public partial class AddExamples : Window
    {
        #region Variables

        /// <summary>
        /// The main examples set
        /// </summary>
        ExampleCollection examples;
        
        /// <summary>
        /// The new examples to be added
        /// </summary>
        ExampleCollection newExamples;

        #endregion

        public AddExamples()
        {
            InitializeComponent();
        }
        
        public AddExamples(IEnumerable<string> words, ExampleCollection examples)
            : this()
        {
            this.examples = examples;
            
            newExamples = new ExampleCollection(
                words.Select(word => new TransliterationExample() { Source = word }));
            dataGridExamples.ItemsSource = newExamples;
        }

        private void dataGridExamples_CurrentCellChanged(object sender, EventArgs e)
        {
            dataGridExamples.Items.Refresh();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            foreach (var example in newExamples)
                examples.Add(example);
            this.DialogResult = true;
            this.Close();
        }
    }
}