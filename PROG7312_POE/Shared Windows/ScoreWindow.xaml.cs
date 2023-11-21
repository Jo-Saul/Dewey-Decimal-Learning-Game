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
    /// Interaction logic for ScoreWindow.xaml
    /// </summary>
    public partial class ScoreWindow : Window
    {
        #region Declarations
        //---------------------------------------------------------------------------------------//
        //class for getting file paths
        private readonly FilePaths filePaths = new FilePaths();

        //bool for storing if the parent window needs to reset the game
        public bool replay = false;

        //Media Players
        private MediaPlayer backgroundPlayer = new MediaPlayer();
        private MediaPlayer effectPlayer = new MediaPlayer();
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Constructor
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="victory"></param>
        /// <param name="score"></param>
        public ScoreWindow(bool victory, string score, string time, int lives)
        {
            try
            {
                InitializeComponent();

                //assign event handler to media playser
                backgroundPlayer.MediaEnded += new EventHandler(backgroundPlayer_MediaEnded);
            
                //set score text
                txtScore.Text = score;
                txtTime.Text = time;
                txtLives.Text = lives.ToString();

                //if user was victorious
                if (victory)
                {
                    Victory();
                }
                else
                { 
                    Defeat();
                }

                //play background music
                backgroundPlayer.Play();
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Methods
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Method for victory state
        /// </summary>
        private void Victory()
        {
            try
            {
                txtTitle.Text = "Victory!";
                txtTitle.Foreground = new SolidColorBrush(Colors.Green);
                backgroundPlayer.Open(new Uri(filePaths.VictoryAudio()));
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        /// <summary>
        /// method for defeat/game over state
        /// </summary>
        private void Defeat()
        {
            try
            {
                txtTitle.Text = "Game Over";
                txtTitle.Foreground = new SolidColorBrush(Colors.Red);
                backgroundPlayer.Open(new Uri(filePaths.DefeatAudio()));
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Event handler for looping the audio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundPlayer_MediaEnded(object sender, EventArgs e)
        {
            backgroundPlayer.Stop();
            backgroundPlayer.Play();
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Buttons
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method for button sound effect
        /// </summary>
        private void ButtonSound()
        {
            try
            {
                effectPlayer.Stop();
                effectPlayer.Open(new Uri(filePaths.ButtonAudio()));
                effectPlayer.Play();
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// event handler for exit button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ButtonSound();
                await Task.Delay(1000);
                Close();
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// event handler for home button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnHome_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                replay = false;
                ButtonSound();
                await Task.Delay(1000);
                backgroundPlayer.Close();
                effectPlayer.Close();
                Close();
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// event handler for replay button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnReplay_Click(object sender, RoutedEventArgs e)
        {
            replay = true;
            ButtonSound();
            await Task.Delay(1000);
            backgroundPlayer.Close();
            effectPlayer.Close();
            Close();
        }
        //---------------------------------------------------------------------------------------//
        #endregion
    }
}
//-----------------------------------------------oO END OF FILE Oo----------------------------------------------------------------------//