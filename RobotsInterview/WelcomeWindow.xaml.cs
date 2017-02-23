using System;
using System.Collections.Generic;
using System.IO;
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

namespace RobotsInterview
{
	/// <summary>
	/// Interaction logic for WelcomeWindow.xaml
	/// </summary>
	public partial class WelcomeWindow : Window
	{
		public WelcomeWindow()
		{
			InitializeComponent();
			Owner = Application.Current.MainWindow;
			System.Windows.Xps.Packaging.XpsDocument d = new System.Windows.Xps.Packaging.XpsDocument(".\\Welcome.xps", FileAccess.Read);
			rd.Document = d.GetFixedDocumentSequence();
		}


		private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.DragMove();
		}

		private void btnContinue_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
