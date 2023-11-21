using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Security.Cryptography;
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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using PROG7312_POE.Classes;
using PROG7312_POE.SharedWindows;
using PROG7312_POE.GameWindows;


namespace PROG7312_POE
{
    /// <summary>
    /// Interaction logic for StartUp.xaml
    /// </summary>
    public partial class StartUp : Window
    {
        #region Declarations
        //---------------------------------------------------------------------------------------//
        //model importer
        private readonly ModelImporter importer = new ModelImporter();

        //class to access file paths
        private readonly FilePaths filePaths = new FilePaths();

        //3D view port
        private Viewport3D viewPort = new Viewport3D();

        //Media Players
        private MediaPlayer backgroundPlayer = new MediaPlayer();
        private MediaPlayer effectPlayer = new MediaPlayer();
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Constructor
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// window constructor
        /// </summary>
        public StartUp()
        {
            try
            {
                InitializeComponent();
                LightandCamera();
                BookShelfSetup();
                AudioSetUp();

                //add viewport to window gird
                startGrid.Children.Insert(0, viewPort);
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Visuals
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to setup light and camera of viewport
        /// </summary>
        private void LightandCamera()
        {
            try
            {
                //camera
                PerspectiveCamera camera = new PerspectiveCamera();
                camera.Position = new Point3D(0, 3, 40);
                viewPort.Camera = camera;

                //Create first DirectionalLight object.
                DirectionalLight directionalLight1 = new DirectionalLight 
                { 
                    Color = Colors.LightYellow, 
                    Direction = new Vector3D(-0.3, -0.2, -0.61) 
                };
                viewPort.Children.Add(new ModelVisual3D { Content = directionalLight1 });

                //Create second DirectionalLight object.
                DirectionalLight directionallight2 = new DirectionalLight 
                { 
                    Color = Colors.CornflowerBlue, 
                    Direction = new Vector3D(7, 0.2, 0.61) 
                };
                viewPort.Children.Add(new ModelVisual3D { Content = directionallight2 });
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to set up the bookself object
        /// </summary>
        private void BookShelfSetup()
        {
            try
            {
                //create visual for shelf
                ModelVisual3D ShelveVisual3D = new ModelVisual3D();
                //load .obj file
                Model3DGroup shelvemodel = importer.Load(filePaths.FullShelf());
                //set Content property of ModelVisual3D object to the loaded model
                ShelveVisual3D.Content = shelvemodel;
                //set start position
                TranslateTransform3D translate = new TranslateTransform3D(0, 3, -4.5);

                //create animation
                DoubleAnimation animation = new DoubleAnimation
                {
                    From = -7, //starting position
                    To = 15, //ending position (downwards)
                    Duration = new Duration(TimeSpan.FromSeconds(25)), //duration
                    AutoReverse = true, //reverse automatically
                    RepeatBehavior = RepeatBehavior.Forever //loop indefinitely
                };

                //apply animation to the Y property of the bookselfs transform
                translate.BeginAnimation(TranslateTransform3D.OffsetYProperty, animation);

                ShelveVisual3D.Transform = translate;
                //add visual to the Viewport3D.
                viewPort.Children.Add(ShelveVisual3D);
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
        /// method to set up the media players
        /// </summary>
        private void AudioSetUp()
        {
            try
            {
                backgroundPlayer.Open(new Uri(filePaths.StartScreenAudio()));
                backgroundPlayer.MediaEnded += new EventHandler(backgroundPlayer_MediaEnded);
                backgroundPlayer.Play();
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// event handler to loop background audio
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


        #region Methods
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to remove the visual/audio assests and close window
        /// </summary>
        private void CloseWindow()
        {
            try
            {
                //close media players
                effectPlayer.Close();
                backgroundPlayer.Close();
                //clear viewport
                viewPort.Children.Clear();
                //close window
                Close();
                //garbage collection
                GC.Collect();
                GC.WaitForPendingFinalizers();
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
        /// event handler for Replacing Books button - takes user to regular page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnReplace_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                effectPlayer.Stop();
                effectPlayer.Open(new Uri(filePaths.StartButtonAudio()));
                effectPlayer.Play();
                await Task.Delay(1500);
                ReplacingBooks window = new ReplacingBooks();
                window.Show();
                CloseWindow();
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// event handler for identifying areas button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnIdentify_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                effectPlayer.Stop();
                effectPlayer.Open(new Uri(filePaths.StartButtonAudio()));
                effectPlayer.Play();
                await Task.Delay(1500);
                IdentifyingAreas window = new IdentifyingAreas();
                window.Show();
                CloseWindow();
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// event handler for exit button - closes application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                effectPlayer.Stop();
                effectPlayer.Open(new Uri(filePaths.ButtonAudio()));
                effectPlayer.Play();
                await Task.Delay(1000);
                CloseWindow();
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
        /// event handler for finding call numbers button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnFind_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                effectPlayer.Stop();
                effectPlayer.Open(new Uri(filePaths.StartButtonAudio()));
                effectPlayer.Play();
                await Task.Delay(1500);
                FindingCallNumbers window = new FindingCallNumbers();
                window.Show();
                CloseWindow();
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
