using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace Dictionary
{
	/// <summary>
	/// Interaction logic for CrashReporterWindow.xaml
	/// </summary>
	public partial class CrashReporterWindow : Window
	{
		public CrashReporterWindow(DispatcherUnhandledExceptionEventArgs e)
		{
			InitializeComponent();
			Exception exp = e.Exception;
			this.Title = exp.Source + " Crash Report";
			lblMessage.Content = exp.Source + " has failed. Please report this error by clicking on Report.";
			txtMessage.Text += "Message:\n" + exp.Message + "\n\n";
			txtMessage.Text += "Stack:\n" + exp.TargetSite + "\n\n";
			txtMessage.Text += "Exception:\n" + exp.ToString() + "\n\n";
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
			this.Close();
		}

		private void btnReport_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
			this.Close();
		}
	}
}