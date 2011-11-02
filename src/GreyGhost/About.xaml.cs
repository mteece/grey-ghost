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

namespace GreyGhost
{
	/// <summary>
	/// Interaction logic for about.xaml
	/// </summary>
	public partial class About : Window
	{
		public About()
		{
			InitializeComponent();
		}

		#region Events
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CloseWindowClickEvent(object sender, RoutedEventArgs e)
		{
			CloseWindowEventHandler();
		}

		/// <summary>
		/// 
		/// </summary>
		private void CloseWindowEventHandler()
		{
			this.Close();
		}
		#endregion Events
		
	}
}
