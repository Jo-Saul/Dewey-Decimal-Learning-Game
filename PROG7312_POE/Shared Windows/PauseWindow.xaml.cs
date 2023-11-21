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
using PROG7312_POE.Classes;

namespace PROG7312_POE.SharedWindows
{
    /// <summary>
    /// Interaction logic for PauseWindow.xaml
    /// </summary>
    public partial class PauseWindow : Window
    {
        #region Declarations
        //---------------------------------------------------------------------------------------//
        //media player
        private MediaPlayer effectPlayer = new MediaPlayer();

        //class to get files paths
        private readonly FilePaths filePaths = new FilePaths();
        //---------------------------------------------------------------------------------------//
        #endregion


        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// constructor
        /// </summary>
        public PauseWindow()
        {
            InitializeComponent();
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// event handler for play button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                effectPlayer.Stop();
                effectPlayer.Open(new Uri(filePaths.ButtonAudio()));
                effectPlayer.Play();
                await Task.Delay(1000);
                Close();
            }
            catch (Exception ex) 
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//

    }
}
//-----------------------------------------------oO END OF FILE Oo----------------------------------------------------------------------//
