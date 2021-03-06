﻿using System;
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
				string location = @"\\OGGY-PC\CrashReports\";
				string filename = Environment.MachineName
					+ "-" + e.Exception.Source
					+ "-" + assembly.Version.ToString()
					+ "-" + DateTime.Now.ToString("yyyyMMddHHmmss")
					+ ".crp";
				try
				{
					File.WriteAllText(location + filename, window.txtMessage.Text);
				}
				catch(Exception)
				{
					string newLocation = @"C:\CrashReports\";
					MessageBox.Show("The location " + location + " is unavailable. The crash report will be saved at " + newLocation);
					File.WriteAllText(newLocation + filename, window.txtMessage.Text);
				}
			}
			e.Handled = true;
		}
	}
}
