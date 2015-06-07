using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace Dictionary
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			CrashReporterWindow window = new CrashReporterWindow(e);
			if (window.ShowDialog().Value)
			{
				var assembly = System.Reflection.Assembly.GetExecutingAssembly().GetName();
				string filename = @"\\OGGY-PC\CrashReports\" + Environment.MachineName + "-" + e.Exception.Source + "-" + assembly.Version.ToString() + ".crp";
				File.WriteAllText(filename, window.txtMessage.Text);
			}
			e.Handled = true;
		}
	}
}
