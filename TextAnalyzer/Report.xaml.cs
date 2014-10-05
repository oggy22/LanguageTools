using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TextAnalyzer
{
    [ValueConversion(typeof(char), typeof(string))]
    public class CharToUnicodeCategory : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            char c = (char)(value);
            return char.GetUnicodeCategory(c).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public string convert(char c)
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class Report : Window
    {
        public Report(TextAnalyzerWorker textAnalyzer)
        {
            InitializeComponent();

            this.dataLetters.ItemsSource = textAnalyzer.letters;
            this.dataWords.ItemsSource = textAnalyzer.all_words[0];
            this.dataWords2.ItemsSource = textAnalyzer.all_words[1];
            this.dataWords3.ItemsSource = textAnalyzer.all_words[2];
            this.dataWords4.ItemsSource = textAnalyzer.all_words[3];
            this.dataWords5.ItemsSource = textAnalyzer.capitalWords;
            
            foreach (DataGridColumn c in dataWords.Columns)
            {
                c.SortDirection = null;
            }
            dataWords.Columns[1].SortDirection = System.ComponentModel.ListSortDirection.Descending; 
            
            this.dataWords.Items.Refresh();
        }
    }
}
