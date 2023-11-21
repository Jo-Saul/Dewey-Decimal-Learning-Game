using BitmapLibrary;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using PROG7312_POE.Classes;
using PROG7312_POE.SharedWindows;

namespace PROG7312_POE
{
    /// <summary>
    /// Interaction logic for UIUserControl.xaml
    /// </summary>
    /// 
    public partial class UIUserControl : UserControl
    {
        #region Declarations
        //---------------------------------------------------------------------------------------//

        //---------------------------- Classes ----------------------------
        //class to get filepahts
        private readonly FilePaths filePaths = new FilePaths();

        //---------------------------- Time ----------------------------
        //timer
        public DispatcherTimer timer = new DispatcherTimer();
        //variable to hold time value
        public TimeSpan time;

        //---------------------------- Media Players ----------------------------
        //background sound
        public MediaPlayer backgroundPlayer = new MediaPlayer();
        //sound effects
        private MediaPlayer effectPlayer = new MediaPlayer();
        
        //---------------------------- Events ----------------------------
        //event for no hearts left
        public event Action NoHeartsLeft;
        //evnet for home button clicked
        public event Action HomeButtonClicked;

        //---------------------------- Hearts ----------------------------
        public int heartCount = 3;
        private Dictionary<int, Image> heartImages;

        //---------------------------- General ----------------------------
        //string for storing what game has been selected
        private string gameSelected = "";
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Constructor
        //---------------------------------------------------------------------------------------//
        public UIUserControl()
        {
            InitializeComponent();

            //map heart images
            heartImages = new Dictionary<int, Image>
            {
                { 3, imgHeart3 },
                { 2, imgHeart2 },
                { 1, imgHeart1 }
            };

            //set up timer
            TimeSetUp();
            //set up audio
            AudioSetup();
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Adjustmenters
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to adjust UI for replaceing books game
        /// </summary>
        public void AdjustForReplace()
        {
            imgHeart1.Margin = new Thickness(42, 25, 678, 339);
            imgHeart2.Margin = new Thickness(146, 25, 0, 0);
            imgHeart3.Margin = new Thickness(251, 25, 0, 0);
            btnExit.Margin = new Thickness(0, 10, 10, 0);
            btnHome.Margin = new Thickness(0, 10, 75, 0);
            btnPause.Margin = new Thickness(0, 10, 140, 0);
            btnHelp.Margin = new Thickness(0, 10, 205, 0);
            timerBorder.HorizontalAlignment = HorizontalAlignment.Center;
            gameSelected = "Replacing Books";
            backgroundPlayer.Open(new Uri(filePaths.ReplaceBooksAudio()));
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to adjust UI for identifying areas game
        /// </summary>
        public void AdjustForIdentifyArea()
        {
            imgHeart1.Margin = new Thickness(10, 25, 0, 0);
            imgHeart2.Margin = new Thickness(108, 25, 0, 0);
            imgHeart3.Margin = new Thickness(212, 25, 0, 0);
            btnExit.Margin = new Thickness(0, 10, 10, 0);
            btnHome.Margin = new Thickness(0, 75, 10, 0);
            btnPause.Margin = new Thickness(0, 140, 10, 0);
            btnHelp.Margin = new Thickness(0, 210, 10, 0);
            timerBorder.HorizontalAlignment = HorizontalAlignment.Left;
            gameSelected = "Identifying Areas";
            backgroundPlayer.Open(new Uri(filePaths.IdentifyAreasAudio()));
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to adjust UI for finding call numbers game
        /// </summary>
        public void AdjustForFindCallNumber()
        {
            imgHeart1.Margin = new Thickness(42, 25, 678, 339);
            imgHeart2.Margin = new Thickness(146, 25, 0, 0);
            imgHeart3.Margin = new Thickness(251, 25, 0, 0);
            btnExit.Margin = new Thickness(0, 10, 10, 0);
            btnHome.Margin = new Thickness(0, 10, 75, 0);
            btnPause.Margin = new Thickness(0, 10, 140, 0);
            btnHelp.Margin = new Thickness(0, 10, 205, 0);
            timerBorder.HorizontalAlignment = HorizontalAlignment.Center;
            gameSelected = "Finding Call Numbers";
            backgroundPlayer.Open(new Uri(filePaths.FindCallNumberAudio()));
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Methods
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to close elements of User Control
        /// </summary>
        public void CloseController()
        {
            try
            {
                timer.Stop();
                backgroundPlayer.Close();
                effectPlayer.Close();
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to reset elements of user control
        /// </summary>
        public void ResetController()
        {
            try
            {
                //resset time
                txtTime.Text = "00:00:00";
                time = new TimeSpan();
                //reset hearst
                heartCount = 3;
                BitmapImage heartImage = BitmapCreator.CreateBitmap(filePaths.HeartFullImage());
                for (int i = 1; i <= 3; i++)
                {
                    heartImages[i].Source = heartImage;
                }
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to manage hearts
        /// </summary>
        /// <returns></returns>
        public async Task HeartManager()
        {
            try
            {
                //play sound effect
                effectPlayer.Open(new Uri(filePaths.LifeLossAudio()));
                effectPlayer.Play();

                //get image corresponding to the current heart count
                Image heartImage = heartImages[heartCount];

                //update heart count
                heartCount--;
                //update image
                heartImage.Source = BitmapCreator.CreateBitmap(filePaths.HeartBrokenImage());
                await Task.Delay(1000);
                heartImage.Source = BitmapCreator.CreateBitmap(filePaths.HeartGreyImage());

                //if no hearts left, show score popup in parent window
                if (heartCount == 0)
                {
                    NoHeartsLeft?.Invoke();
                }
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Time
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to set up timer
        /// </summary>
        private void TimeSetUp()
        {
            try
            {
                time = TimeSpan.Zero;
                timer.Interval = new TimeSpan(0, 0, 1);
                timer.Tick += Timer_Tick;
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// event handler for timer tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                txtTime.Text = time.ToString("c");
                time = time.Add(TimeSpan.FromSeconds(1));
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Audio
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to set up media players
        /// </summary>
        private void AudioSetup()
        {
            try
            {
                //set effect volume to max
                effectPlayer.Volume = 1;
                //set back ground music player
                //backgroundPlayer.Open(new Uri(filePaths.IdentifyAreasAudio()));
                //add loop event
                backgroundPlayer.MediaEnded += new EventHandler(backgroundPlayer_MediaEnded);
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Event handler for looping the background audio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundPlayer_MediaEnded(object sender, EventArgs e)
        {
            try
            {
                backgroundPlayer.Stop();
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


        #region Buttons
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to play button sound effect
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
                //play sound effect
                ButtonSound();
                await Task.Delay(1000);

                //shut down application
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
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //play sound effect
                ButtonSound();
                //go to home window
                StartUp window = new StartUp();
                window.Show();

                // Raise the HomeButtonClicked event
                HomeButtonClicked?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// event handler for pause button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //play sound effect
                ButtonSound();

                //stop timer
                timer.Stop();
                //show pause popup
                PauseWindow pauseWindow = new PauseWindow();
                pauseWindow.ShowDialog();
                //resume timer
                timer.Start();
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// event handler for help button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //play sound effect
                ButtonSound(); 
                //stop timer
                timer.Stop();
                //show help popup
                HelpWindow helpWindow = new HelpWindow(gameSelected);
                helpWindow.ShowDialog();
                //resume timer
                timer.Start();
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        #endregion
    }
}
//-----------------------------------------------oO END OF FILE Oo----------------------------------------------------------------------//