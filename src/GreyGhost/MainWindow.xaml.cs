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

using MarkdownSharp;

namespace GreyGhost
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{

			this.btnMarkDown.Click += new RoutedEventHandler(btnMarkDown_Click);

		}

		public void btnMarkDown_Click(object sender, RoutedEventArgs e)
		{
			if (this.tbInput.Text.Length > 0)
			{
				MarkdownOptions opts = new MarkdownOptions();
				opts.AutoNewlines = true;
				opts.AutoHyperlink = true;
				opts.EncodeProblemUrlCharacters = true;

				// Strings are heavy and immutable
				StringBuilder output = new StringBuilder();

				MarkdownSharp.Markdown maker = new Markdown(opts);
				output.Append(maker.Transform(this.tbInput.Text));

				this.tbOutput.Text = output.ToString();
				this.wbPreview.NavigateToString(output.ToString());


			}
		}
	}
}
