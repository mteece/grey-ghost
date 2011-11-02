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
using Microsoft.Win32;
using System.IO;
using System.ComponentModel;

namespace GreyGhost
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private bool _dataChanged = false;
		private string _filePath = null;

		/// <summary>
		/// MainWindow init.
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();
		}

		#region Events

		/// <summary>
		/// Event handler when New is selected from the Menu.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NewItemClickEvent(object sender, RoutedEventArgs e)
		{
			HandleNewCommand();
		}

		/// <summary>
		/// Event handler when Open is selected from the Menu.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OpenItemClickEvent(object sender, RoutedEventArgs e)
		{
			HandleOpenCommand();
		}

		/// <summary>
		/// Event handler when Save is selected from the Menu.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SaveItemClickEvent(object sender, RoutedEventArgs e)
		{
			HandleSaveCommand();
		}

		/// <summary>
		/// Event handler when Save As is selected from the Menu.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SaveAsItemClickEvent(object sender, RoutedEventArgs e)
		{
			ShowSaveDialog();
		}

		/// <summary>
		/// Event handler when Save As Markdown is selected from the Menu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SaveAsMarkdownItemClickEvent(object sender, RoutedEventArgs e)
		{
			ShowSaveAsMarkdownDialog();
		}

		/// <summary>
		/// Event handler when Exit is selected from the Menu.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ExitItemClickEvent(object sender, RoutedEventArgs e)
		{
			if (this._dataChanged)
			{
				SaveFirst();
				this.Close();
			}
			else
			{
				this.Close();
			}
		}

		/// <summary>
		/// Save document before closing application
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AppClosing(object sender, CancelEventArgs e)
		{
			if (this._dataChanged)
			{
				SaveFirst();
			}
		}

		/// <summary>
		/// Whenever a key is pressed inside the text field, take care of it
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ContentChangedEvent(object sender, KeyEventArgs e)
		{
			this._dataChanged = true;
		}

		/// <summary>
		/// This is done to check whether our content had changed or not.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FocusChangedEvent(object sender, RoutedEventArgs e)
		{
			this._dataChanged = true;
		}

		/// <summary>
		/// Handle Key Combinations for Saving, New, and Open
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void KeyCombinations(object sender, KeyEventArgs e)
		{
			// Ctrl + M
			if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.M))
			{
				HandleMarkdownCommand();
			}

			// Ctrl + N
			if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.N))
			{
				HandleNewCommand();
			}

			// Ctrl + O
			if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.O))
			{
				HandleOpenCommand();
			}

			// Ctrl + S
			if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.S))
			{
				HandleSaveCommand();
			}
		}

		#endregion Events
		
		#region Dialogs
		
		/// <summary>
		/// Display Open File dialog window.
		/// </summary>
		private void ShowOpenDialog()
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Text Files|*.txt|All|*.*";

			//
			// If user selected a file and pressed OK, Handle it.
			//
			if (ofd.ShowDialog() == true)
			{
				this._filePath = ofd.FileName;
				ReadFile(this._filePath);
				SetTitle(ofd.SafeFileName);
				this._dataChanged = false;
			}
		}

		/// <summary>
		/// Display the Save Dialog window to specify the location to save your file as text.
		/// </summary>
		private void ShowSaveDialog()
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "Text Document (.txt)|*.txt";
			sfd.AddExtension = true;
			sfd.DefaultExt = "txt";

			bool? saveResult = sfd.ShowDialog();

			if (saveResult == true)
			{
				string s = sfd.FileName;
				this._filePath = s;
				SaveFile(s, false);
				SetTitle(sfd.SafeFileName);
			}
		}

		/// <summary>
		///  Display the Save Dialog window to specify the location to save your file as formatted Markdown.
		/// </summary>
		private void ShowSaveAsMarkdownDialog()
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "Markdown Document (.md)|*.md";
			sfd.AddExtension = true;
			sfd.DefaultExt = "md";

			bool? saveResult = sfd.ShowDialog();

			if (saveResult == true)
			{
				string s = sfd.FileName;
				this._filePath = s;
				SaveFile(s, true);
				SetTitle(sfd.SafeFileName);
			}
		}

		/// <summary>
		/// Ensure that your save your changes before doing anything that might alter your textbox contents or close the program
		/// </summary>
		/// <returns></returns>
		private string SaveFirst()
		{
			MessageBoxResult mbr = MessageBox.Show("Do you want to save your changes first?", "Save File?", MessageBoxButton.YesNoCancel);

			if (mbr == MessageBoxResult.Yes)
			{
				if (this._filePath != null)
				{
					SaveFile(this._filePath, false);
				}
				else
				{
					HandleSaveCommand();
				}
			}
			else if (mbr == MessageBoxResult.Cancel)
			{
				return "Cancel";
			}
			return "Nothing";
		}

		/// <summary>
		/// Display the About Dialog window.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AboutItemClickEvent(object sender, RoutedEventArgs e)
		{
			ShowAboutDialog();
		}

		/// <summary>
		/// Display the Syntax Dialog window.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SyntaxItemClickEvent(object sender, RoutedEventArgs e)
		{
			ShowSyntaxDialog();
		}

		/// <summary>
		/// Handle the details about where the About Dialog window will actually show.
		/// </summary>
		private void ShowAboutDialog()
		{
			About a = new About();
			a.WindowStartupLocation = WindowStartupLocation.Manual;
			a.Left = this.Left + 75;
			a.Top = this.Top + 75;
			a.ShowDialog();
		}

		/// <summary>
		/// Handle the details about where the Syntax Dialog window will actually show.
		/// </summary>
		private void ShowSyntaxDialog()
		{
			Syntax s = new Syntax();
			s.WindowStartupLocation = WindowStartupLocation.Manual;
			s.Left = this.Left + 75;
			s.Top = this.Top + 75;
			s.ShowDialog();
		}

		#endregion Dialogs
		
		#region IO
		
		/// <summary>
		///  When given a file path, read that information and load it into the textfield.
		/// </summary>
		/// <param name="path"></param>
		private void ReadFile(string path)
		{
			StreamReader reader = new StreamReader(path);
			StringBuilder sb = new StringBuilder();

			string r = reader.ReadLine();
			while (r != null)
			{
				sb.Append(r);
				sb.Append(Environment.NewLine);
				r = reader.ReadLine();
			}
			reader.Close();
			//
			// Sending text data to the text box
			//
			txtBoxContent.Text = sb.ToString();
		}

		/// <summary>
		///  Given a path to a file, write the contents of the textbox into it
		/// </summary>
		/// <param name="path"></param>
		private void SaveFile(string path, bool asMarkdown)
		{
			StreamWriter writer = new StreamWriter(path);

			if (asMarkdown)
			{
				writer.Write(this.ConvertToMarkup(txtBoxContent.Text));
			}
			else 
			{
				writer.Write(txtBoxContent.Text);				
			}

			writer.Close();
			this._dataChanged = false;
			
			
		}

		#endregion IO

		#region Conversion
		
		/// <summary>
		/// Converts Markdown syntax to markup.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		private string ConvertToMarkup(string markdown) {

			MarkdownOptions opts = new MarkdownOptions();
			opts.AutoNewlines = true;
			opts.AutoHyperlink = true;
			opts.EncodeProblemUrlCharacters = true;

			// Strings are heavy and immutable
			StringBuilder output = new StringBuilder();

			MarkdownSharp.Markdown maker = new Markdown(opts);
			output.Append(maker.Transform(markdown));

			return output.ToString();
		}

		#endregion Conversion

		#region State
		
		/// <summary>
		///  Clears all of the information from the screen.
		/// </summary>
		private void ClearState()
		{
			this._filePath = null;
			this._dataChanged = false;

			txtBoxContent.Text = "";

			ResetTitle();
		}

		#endregion State

		#region Commands

		/// <summary>
		/// Save markdown document.
		/// </summary>
		private void HandleMarkdownCommand()
		{
			if (this._filePath == null)
			{
				ShowSaveAsMarkdownDialog();
			}
			else
			{
				SaveFile(this._filePath, true);
			}
		}

		/// <summary>
		/// Save text document.
		/// </summary>
		private void HandleSaveCommand()
		{
			if (this._filePath == null)
			{
				ShowSaveDialog();
			}
			else
			{
				SaveFile(this._filePath, false);
			}
		}
		
		/// <summary>
		/// Determine best way to setup New document.
		/// </summary>
		private void HandleNewCommand()
		{
			if (this._dataChanged)
			{
				string sf = SaveFirst();
				if (sf != "Cancel")
				{
					ClearState();
				}
			}
			else
			{
				ClearState();
			}
		}

		/// <summary>
		/// Determine best way to setup New document
		/// </summary>
		private void HandleOpenCommand()
		{
			if (this._dataChanged)
			{
				SaveFirst();
				ShowOpenDialog();
			}
			else
			{
				ShowOpenDialog();
			}
		}

		#endregion Commands

		#region Window
		
		/// <summary>
		/// Change the Window title to include the file's name when opened or saved.
		/// </summary>
		/// <param name="newTitle"></param>
		private void SetTitle(string newTitle)
		{
			Window.Title = "Grey Ghost - " + newTitle;
		}

		/// <summary>
		/// Reset the Window Title.
		/// </summary>
		private void ResetTitle()
		{
			Window.Title = "Grey Ghost";
		}
		#endregion Window
	}
}
