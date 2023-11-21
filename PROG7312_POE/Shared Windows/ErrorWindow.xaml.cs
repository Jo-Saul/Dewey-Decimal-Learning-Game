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

namespace PROG7312_POE.SharedWindows
{
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// constructor for error window
        /// </summary>
        /// <param name="Error"></param>
        public ErrorWindow(string Error)
        {
            InitializeComponent();
            txtError.Text = Error + "\nPlease close the application and restart."; 
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// event handler for close button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //shut down program
            Application.Current.Shutdown();
        }
        //---------------------------------------------------------------------------------------//
    }
}
//-----------------------------------------------oO END OF FILE Oo----------------------------------------------------------------------//