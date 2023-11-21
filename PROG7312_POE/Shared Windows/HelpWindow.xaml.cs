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
using PROG7312_POE.SharedWindows;
using PROG7312_POE.Classes;

namespace PROG7312_POE.SharedWindows
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        #region Declarations
        //---------------------------------------------------------------------------------------//
        //effect player
        private MediaPlayer effectPlayer = new MediaPlayer();

        //class for file paths
        private FilePaths filePaths = new FilePaths();
        //---------------------------------------------------------------------------------------//
        #endregion
        

        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="parent"></param>
        public HelpWindow(string parent)
        {
            InitializeComponent();

            switch (parent)
            {
                case "Replacing Books":
                    txtHelp.Text = Properties.Resources.ReplaceBooksHelp;
                    break;
                case "Identifying Areas":
                    txtHelp.Text = Properties.Resources.IdentifyingAreasHelp;
                    break;
                case "Finding Call Numbers":
                    txtHelp.Text = Properties.Resources.FindingCallNumbersHelp;
                    break;
                default:
                    txtHelp.Text = "no info found";
                    break;
            }
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// event handler for Ok button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                //play button sound effect
                effectPlayer.Stop();
                effectPlayer.Open(new Uri(filePaths.ButtonAudio()));
                effectPlayer.Play();
                await Task.Delay(1000);
            }
            catch (Exception ex) 
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
            //close window
            Close();
        }
        //---------------------------------------------------------------------------------------//
    }
}
//-----------------------------------------------oO END OF FILE Oo----------------------------------------------------------------------//