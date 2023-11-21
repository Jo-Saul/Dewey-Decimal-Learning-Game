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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BitmapLibrary;
using PROG7312_POE.Classes;

namespace PROG7312_POE.SharedWindows
{
    /// <summary>
    /// Interaction logic for CountDownWindow.xaml
    /// </summary>
    public partial class CountDownWindow : Window
    {
        #region Declaration
        //---------------------------------------------------------------------------------------//
        //class for getting file paths
        private readonly FilePaths filePaths = new FilePaths();

        //effect media player
        private readonly MediaPlayer effectPlayer = new MediaPlayer();

        //dictionary for number images
        private Dictionary<int, BitmapImage> numberImages = new Dictionary<int, BitmapImage>();
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Constructor
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// constructor of CountDownWindow
        /// </summary>
        public CountDownWindow()
        {
            try
            {
                InitializeComponent();

                //set effect player sound
                effectPlayer.Open(new Uri(filePaths.TimeTickAudio()));

                //get images of numbers
                numberImages = new Dictionary<int, BitmapImage>
                {
                    { 1, BitmapCreator.CreateBitmap(filePaths.OneImage()) },
                    { 2, BitmapCreator.CreateBitmap(filePaths.TwoImage()) },
                    { 3, BitmapCreator.CreateBitmap(filePaths.ThreeImage()) }
                };

                //play animation
                CountDownAnimation();
            }
            catch (Exception ex)
            { 
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Animation
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to play countdown animation
        /// </summary>
        private async void CountDownAnimation()
        {
            try
            {
                //wait for window to load
                await Task.Delay(500);

                //create fade in animation
                var fadeIn = new DoubleAnimation
                {
                    From = 0.0,
                    To = 1.0,
                    Duration = new Duration(TimeSpan.FromSeconds(0.5))
                };

                //create fade out animation
                var fadeOut = new DoubleAnimation
                {
                    From = 1.0,
                    To = 0.0,
                    Duration = new Duration(TimeSpan.FromSeconds(0.5))
                };

                //count down loop
                for (int i = 3; i >= 1; i--)
                {
                    await Task.Delay(500);
                    //set image source
                    imgNumber.Source = numberImages[i];
                    //play sound effect
                    effectPlayer.Play();
                    //play animation
                    imgNumber.BeginAnimation(Image.OpacityProperty, fadeIn);
                    //wait
                    await Task.Delay(500);
                    effectPlayer.Stop();
                }
                //play go effect
                effectPlayer.Open(new Uri(filePaths.GoAudio()));
                effectPlayer.Play();
                //set go image
                imgNumber.Source = BitmapCreator.CreateBitmap(filePaths.GoImage());
                imgNumber.BeginAnimation(Image.OpacityProperty, fadeIn);
                await Task.Delay(500);
                imgNumber.BeginAnimation(Image.OpacityProperty, fadeOut);

                //wait for effect
                await Task.Delay(800);
                //close window
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
        #endregion
    }
}
//-----------------------------------------------oO END OF FILE Oo----------------------------------------------------------------------//