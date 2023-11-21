using HelixToolkit.Wpf;
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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Windows.Threading;
using BitmapLibrary;
using DeweyLibrary;
using System.IO.Ports;
using PROG7312_POE.Classes;
using PROG7312_POE.SlotClasses;
using PROG7312_POE.SharedWindows;

namespace PROG7312_POE.GameWindows
{
    /// <summary>
    /// Interaction logic for IdentifyingAreas.xaml
    /// </summary>
    public partial class IdentifyingAreas : Window
    {
        #region Declarations
        //---------------------------------------------------------------------------------------//

        //---------------------------- Classes ----------------------------
        //model importer
        private readonly ModelImporter importer = new ModelImporter();
        //class to get filepahts
        private readonly FilePaths filePaths = new FilePaths();
        //text hander class
        private readonly TextHandler textHandler = new TextHandler();
        //calculator class
        private readonly Calculator calculator = new Calculator();


        //---------------------------- Lists ----------------------------
        //list to hold book visuals 
        private List<ModelVisual3D> bookVisualList = new List<ModelVisual3D>();
        //list to hold shelf slots
        private List<DrawerSlot> drawerSlots = new List<DrawerSlot>();
        //list of text on books
        List<string> booklist = new List<string>();
        //list of text on labels of drawer
        List<string> labels = new List<string>();
        //dictionary for dewey call numbers and description
        Dictionary<int, string> dewey = new Dictionary<int, string>();


        //---------------------------- Media Players ----------------------------
        //sound effects
        private MediaPlayer effectPlayer = new MediaPlayer();

        //---------------------------- Visuals ----------------------------
        //3D viewport variable
        private Viewport3D viewPort = new Viewport3D();

        private ModelVisual3D drawerVisual3D = new ModelVisual3D();

        //bool to check if a button is active
        bool buttonPressed = false;

        //book area geometry
        GeometryModel3D bookSpaceGeo = new GeometryModel3D();
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Constructor
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// constructor for identifying areas game
        /// </summary>
        public IdentifyingAreas()
        {
            try
            {
                InitializeComponent();

                //populate data
                PopulateDeweyData();

                //visual set up
                LightandCamera();
                DrawerSetup();
                BookSetup();
                BookRowSpace();
                DrawerSlotSetup();
                BackgroundSetup();
                BackgroundFade();

                //audio setup
                //set effect volume to max
                effectPlayer.Volume = 1;

                //setup mouse movement
                MouseMovement();

                //Set up UI controller
                UIController.NoHeartsLeft += () => ScorePopup(false);
                UIController.HomeButtonClicked += () => CloseWindow();
                UIController.AdjustForIdentifyArea();

                //play countdown
                StartCountDown();

                //add viewport to window gird
                IdentifyGrid.Children.Insert(0, viewPort);
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
        /// method to set up light and camera of viewport
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
        /// method to set up drawer object
        /// ChatGPT used to get importer
        /// </summary>
        private void DrawerSetup()
        {
            try
            {
                //shuffle list of lables
                Random rand = new Random();
                labels = labels.OrderBy(l => rand.Next()).ToList();

                //create lables for drawer
                for (int i = 0; i < 7; i++)
                {
                    textHandler.AddTextToDrawer(i + 1, labels[i]);
                }
                //drawer visuals
                drawerVisual3D = new ModelVisual3D
                {
                    Content = importer.Load(filePaths.Drawer()), //import objetc
                    Transform = new TranslateTransform3D(7, 3, -20) //location
                };
                //add visual to view port
                viewPort.Children.Add(drawerVisual3D);
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to set up background shelfs
        /// </summary>
        private void BackgroundSetup()
        {
            try
            {
                //right shelf
                ModelVisual3D rightshelfVisual3D = new ModelVisual3D
                {
                    Content = importer.Load(filePaths.FullShelf()), //import objetc
                    Transform = new TranslateTransform3D(37, 3, -33) //location
                };
                viewPort.Children.Add(rightshelfVisual3D);

                //left shelf
                ModelVisual3D leftshelfVisual3D = new ModelVisual3D
                {
                    Content = importer.Load(filePaths.FullShelf()), //import objetc
                    Transform = new TranslateTransform3D(-12, 1, -33) //location
                };
                viewPort.Children.Add(leftshelfVisual3D);
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to set up drawer slots
        /// </summary>
        private void DrawerSlotSetup()
        {
            try
            {
                //set start parameters
                double slotTop = 15.5;
                double slotBottom = 12.5;
                //Set the distance between the slots
                double distanceBetweenSlots = 3.65;

                //create 7 slots
                for (int i = 0; i < 7; i++)
                {
                    //create new slot
                    drawerSlots.Add(new DrawerSlot
                    {
                        SlotTop = slotTop,
                        SlotBottom = slotBottom,
                        bookIndex = -1
                    });

                    //Update the left and right Y coordinates for the next slot
                    slotTop -= distanceBetweenSlots;
                    slotBottom -= distanceBetweenSlots;
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
        /// method to set up bookrow space
        /// source: https://learn.microsoft.com/en-us/dotnet/desktop/wpf/graphics-multimedia/how-to-create-a-3-d-scene?view=netframeworkdesktop-4.8
        /// </summary>
        private void BookRowSpace()
        {
            try
            {
                // Create the mesh for the book space
                MeshGeometry3D mesh = new MeshGeometry3D
                {
                    Positions = new Point3DCollection //create mesh points
                    {
                        new Point3D(-17.3, -4, -9),
                        new Point3D(-6, -4, -9),
                        new Point3D(-6, 7.5, -9),
                        new Point3D(-17.3, 7.5, -9)
                    },
                    TriangleIndices = new Int32Collection(new int[] { 0, 1, 2, 2, 3, 0 }) //set verticies
                };

                //Create a Material for the row space
                Material material = new DiffuseMaterial(new SolidColorBrush(System.Windows.Media.Color.FromArgb(200, 255, 100, 100)));

                //Create a GeometryModel3D for the row
                bookSpaceGeo = new GeometryModel3D(mesh, material);

                //Add the GeometryModel3D to the Model3DGroup
                Model3DGroup bookSpaceModel = new Model3DGroup();
                bookSpaceModel.Children.Add(bookSpaceGeo);

                //Create a ModelVisual3D for the row
                ModelVisual3D bookSpaceVisual3D = new ModelVisual3D { Content = bookSpaceModel };

                // Add the ModelVisual3D to the Viewport3D
                viewPort.Children.Add(bookSpaceVisual3D);

                //null material of row - this makes it appear invisible
                bookSpaceGeo.Material = bookSpaceGeo.Material == material ? null : material;
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to set up 4 book objects
        /// </summary>
        private void BookSetup()
        {
            try
            {
                //create a list of cover indices and shuffle it
                List<int> coverIndices = Enumerable.Range(1, 10).OrderBy(x => Guid.NewGuid()).ToList();

                //create 4 books
                for (int i = 0; i < 4; i++)
                {
                    //set text of book
                    textHandler.AddTextToDrawerBook(coverIndices[i], i + 1, booklist[i]);

                    //Create a new ModelVisual3D object and load the .obj file
                    ModelVisual3D BookVisual3D = new ModelVisual3D
                    {
                        Content = importer.Load(filePaths.DrawerBook(i+1)),
                        //set start position and add visual to viewport
                        Transform = new TranslateTransform3D(new Vector3D(-12, -2.5 + (i * 2.8), -11))
                    };
                    //add visual to viewport
                    viewPort.Children.Add(BookVisual3D);
                    //add visual to book list
                    bookVisualList.Add(BookVisual3D);
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
        /// method to set up the background fade
        /// </summary>
        private void BackgroundFade()
        {
            try
            {
                // Create the mesh for the book space
                MeshGeometry3D mesh = new MeshGeometry3D
                {
                    Positions = new Point3DCollection //create mesh points
                    {
                        new Point3D(-28, -13, -25),
                        new Point3D(28, -13, -25),
                        new Point3D(28, 33, -25),
                        new Point3D(-28, 33, -25)
                    },
                    TriangleIndices = new Int32Collection(new int[] { 0, 1, 2, 2, 3, 0 }) //set verticies
                };

                //Create a Material for the row space
                Material material = new DiffuseMaterial(new SolidColorBrush(System.Windows.Media.Color.FromArgb(170, 54, 20, 3)));

                //Create a GeometryModel3D for the row
                GeometryModel3D backspace = new GeometryModel3D(mesh, material);

                //Add the GeometryModel3D to the Model3DGroup
                Model3DGroup bookSpaceModel = new Model3DGroup();
                bookSpaceModel.Children.Add(backspace);

                //Create a ModelVisual3D for the row
                ModelVisual3D bookSpaceVisual3D = new ModelVisual3D { Content = bookSpaceModel };

                // Add the ModelVisual3D to the Viewport3D
                viewPort.Children.Add(bookSpaceVisual3D);
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Mouse Movement
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to setup move movement handlers
        /// Chat GPT used to help create HitTestResultMethod
        /// </summary>
        private void MouseMovement()
        {
            try
            {
                //starting postion of mouth
                Point _startPos = new Point();
                //bool for if object is being dragged
                bool _isDragging = false;
                //visual for clicked object
                ModelVisual3D _clickedObject = null;
                //index of clicked object
                int _clickedIndex = -1;

                //Calculate the current scale factor
                //ChatGPT was used to calculate the scale
                Window window = Application.Current.MainWindow;
                var source = PresentationSource.FromVisual(window);
                Matrix m = source.CompositionTarget.TransformToDevice;
                double dpiX = m.M11 * 96.0;
                double dpiY = m.M22 * 96.0;
                double _scaleFactor = dpiX / 96.0;
                _scaleFactor /= 50;
               

                //---------------------------------------------------------------------------------------//
                // Add a MouseLeftButtonDown event handler to the Viewport3D
                viewPort.MouseLeftButtonDown += (s, e) =>
                {
                    //Perform a hit test to determine which object was clicked on
                    VisualTreeHelper.HitTest(viewPort, null, result => HitTestResultMethod(result, e), new PointHitTestParameters(e.GetPosition(viewPort)));
                };
                //---------------------------------------------------------------------------------------//
                // Create a HitTestResultCallback
                HitTestResultBehavior HitTestResultMethod(HitTestResult result, MouseButtonEventArgs e)
                {
                    // Check if the result is a RayMeshGeometry3DHitTestResult
                    if (result is RayMeshGeometry3DHitTestResult meshResult)
                    {
                        // Check if the hit visual is one of the books
                        if (bookVisualList.Contains(meshResult.VisualHit))
                        {
                            //record the starting position of the mouse and capture it
                            _startPos = e.GetPosition(viewPort);
                            viewPort.CaptureMouse();
                            _isDragging = true;

                            //store the clicked object
                            _clickedObject = meshResult.VisualHit as ModelVisual3D;

                            //get the index of the clicked object in the BookVisual3DList
                            _clickedIndex = bookVisualList.IndexOf(_clickedObject);
                        }
                    }
                    // Return HitTestResultBehavior.Continue to continue hit testing
                    return HitTestResultBehavior.Continue;
                }
                //---------------------------------------------------------------------------------------//
                // Add a MouseMove event handler to the Viewport3D
                viewPort.MouseMove += (s, e) =>
                {
                    //if mouse is dragging and book was clicked on
                    if (_isDragging && _clickedObject != null)
                    {
                        // Calculate the change in position of the mouse
                        Point currentPos = e.GetPosition(viewPort);
                        Vector delta = currentPos - _startPos;

                        //get position of clicked object
                        TranslateTransform3D translateTransform = _clickedObject.Transform as TranslateTransform3D;
                        if (translateTransform == null)
                        {
                            translateTransform = new TranslateTransform3D();
                            _clickedObject.Transform = translateTransform;
                        }


                        //apply position to object
                        translateTransform.OffsetZ = -11;
                        translateTransform.OffsetX += delta.X * _scaleFactor;
                        translateTransform.OffsetY -= delta.Y * _scaleFactor;

                        //update the starting position of the mouse
                        _startPos = currentPos;

                    }
                };
                //---------------------------------------------------------------------------------------//
                // Add a MouseLeftButtonUp event handler to the Viewport3D
                viewPort.MouseLeftButtonUp += (s, e) =>
                {
                    // Release the mouse capture and reset variables
                    viewPort.ReleaseMouseCapture();
                    _isDragging = false;

                    //if book is clicked
                    if (_clickedObject != null)
                    {
                        //get the tranformation of the moved object
                        TranslateTransform3D translateTransform = _clickedObject.Transform as TranslateTransform3D;

                        //find current book location and remove
                        int currentIndex = drawerSlots.FindIndex(slot => slot.bookIndex == _clickedIndex);
                        if (currentIndex != -1)
                        {
                            drawerSlots[currentIndex].bookIndex = -1;
                        }

                        bool bookInSlot = false;
                        //if book in drawer section
                        if (translateTransform.OffsetX >= 5.2 && translateTransform.OffsetX <= 16.6)
                        {
                            //check for empty slot to place book in
                            var drawerSlot = drawerSlots.FirstOrDefault(slot => translateTransform.OffsetY <= slot.SlotTop && translateTransform.OffsetY >= slot.SlotBottom && slot.bookIndex == -1);
                            //if shelf slot is found
                            if (drawerSlot != null)
                            {
                                // Set location
                                translateTransform.OffsetX = 10.9;
                                translateTransform.OffsetY = (drawerSlot.SlotTop + drawerSlot.SlotBottom)/2;
                                translateTransform.OffsetZ = -20;

                                // Set book slot
                                drawerSlot.bookIndex = _clickedIndex;
                                bookInSlot = true;

                                // Play sound effect
                                effectPlayer.Open(new Uri(filePaths.BookPlaceAudio()));
                                effectPlayer.Stop();
                                effectPlayer.Play();
                            }
                        }

                        //if book is not in a slot
                        if (bookInSlot == false)
                        {
                            // Return the object to its original position
                            translateTransform.OffsetX = -12;
                            translateTransform.OffsetY = -2.5 + (_clickedIndex * 2.8);
                            translateTransform.OffsetZ = -11;
                        }
                    }
                    // Reset the clicked object variable
                    _clickedObject = null;
                };
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
        /// method to populate lists with dewey decimal data
        /// </summary>
        private void PopulateDeweyData()
        {
            try
            {
                dewey = DeweyDecimal.GetDewey();

                // Create a list of dewy indices and shuffle it
                List<int> deweyIndices = Enumerable.Range(0, 9).OrderBy(x => Guid.NewGuid()).ToList();

                Random random = new Random();
                bool addCallNumber = random.Next(2) == 1;
                // Process the first 4 indices
                for (int i = 0; i < 4; i++)
                {
                    //Random random = new Random();
                    int index = deweyIndices[i];
                    //bool addCallNumber = random.Next(2) == 1;

                    // If true, add call number to book. If false, add to drawer
                    booklist.Add(addCallNumber ? index.ToString() : dewey[index]);
                    labels.Add(addCallNumber ? dewey[index] : index.ToString());
                }

                // Process the next 3 indices
                for (int i = 4; i < 7; i++)
                {
                    //Random random = new Random();
                    int index = deweyIndices[i];
                    //bool addCallNumber = random.Next(2) == 1;

                    // If true, add call number. If false, add description
                    labels.Add(addCallNumber ? dewey[index] : index.ToString());
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
        /// method to close window - remove visual/audio assests and closes window
        /// NB: removing any line will break the program
        /// </summary>
        private void CloseWindow()
        {
            try
            {
                //close controller (time + background music)
                UIController.CloseController();
                //clear booklist
                for (int i = 0; i < bookVisualList.Count; i++)
                {
                    viewPort.Children.Remove(bookVisualList[i]);
                    bookVisualList[i].Content = null;
                }
                bookVisualList.Clear();
                //clear drawer
                viewPort.Children.Remove(drawerVisual3D);
                drawerVisual3D.Content = null;
                //close media players
                effectPlayer.Close();
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
        /// <summary>
        /// method to display countdown window
        /// </summary>
        private async void StartCountDown()
        {
            try
            {
                //give time for window to load
                await Task.Delay(800);
                //show countdown popup
                CountDownWindow countDownWindow = new CountDownWindow();
                countDownWindow.ShowDialog();
                //set rectangle  area to hidden
                rectBackdrop.Visibility = Visibility.Hidden;
                UIController.backgroundPlayer.Play();
                //start timer
                UIController.timer.Start();
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to flash area that marks the book row
        /// </summary>
        /// <returns></returns>
        private async Task FlashBooksAsync()
        {
            try
            {
                Material material = new DiffuseMaterial(new SolidColorBrush(System.Windows.Media.Color.FromArgb(200, 255, 21, 21)));
                effectPlayer.Open(new Uri(filePaths.BeepAudio()));
                //flash area 3 times
                for (int i = 0; i < 3; i++)
                {
                    bookSpaceGeo.Material = material;
                    effectPlayer.Stop();
                    effectPlayer.Play();
                    await Task.Delay(500);
                    bookSpaceGeo.Material = bookSpaceGeo.Material == material ? null : material;
                    await Task.Delay(500);
                }
                buttonPressed = false;
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to get the list of the books in the drawer
        /// </summary>
        /// <returns></returns>
        private List<string> GetCurrentList()
        {
            return drawerSlots.Select(slot => slot.bookIndex == -1 ? "" : booklist[slot.bookIndex]).ToList();
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to call score pop up window
        /// </summary>
        /// <param name="victory"></param>
        private void ScorePopup(bool victory)
        {
            try
            {
                //stop timer
                UIController.timer.Stop();
                //stop audio
                UIController.backgroundPlayer.Stop();
                effectPlayer.Stop();
                //set back rectangle to visible
                rectBackdrop.Visibility = Visibility.Visible;

                //calculate score
                int score = calculator.CalcIdentifyAreaScore(labels, GetCurrentList(), (int)UIController.time.TotalSeconds, UIController.heartCount);

                //show popup
                ScoreWindow scoreWindow = new ScoreWindow(victory, score.ToString(), UIController.txtTime.Text, UIController.heartCount);
                scoreWindow.ShowDialog();

                //if replay bool is true
                if (scoreWindow.replay)
                {
                    //reset game
                    Reset();
                }
                else
                {
                    //go to home screen
                    StartUp startUp = new StartUp();
                    startUp.Show();
                    CloseWindow();
                }
                buttonPressed = false;
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to reset the game
        /// </summary>
        private void Reset()
        {
            try
            {
                //set back rectangle to hidden
                rectBackdrop.Visibility = Visibility.Visible;
                //stop timer
                UIController.timer.Stop();

                viewPort.Children.Remove(drawerVisual3D);
                drawerVisual3D.Content = null;

                //clear booklist
                for (int i = 0; i < bookVisualList.Count; i++)
                {
                    drawerSlots[i].bookIndex = -1;
                    viewPort.Children.Remove(bookVisualList[i]);
                    bookVisualList[i].Content = null;
                }
                bookVisualList.Clear();
                bookVisualList = new List<ModelVisual3D>();

                //clear lists
                booklist.Clear();
                labels.Clear();

                //clear book positions in drawer
                for (int j = 0; j < drawerSlots.Count; j++)
                {
                    drawerSlots[j].bookIndex = -1;
                }
                

                //garbage collet
                GC.Collect();
                GC.WaitForPendingFinalizers();

                //reset data
                PopulateDeweyData();


                //reset drawer
                DrawerSetup();

                //reset books
                BookSetup();

                //reset hearts and time using Control method
                UIController.ResetController();

                buttonPressed = false;
                //show count down
                StartCountDown();
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
        /// event handler for done button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnDone_Click(object sender, RoutedEventArgs e)
        {
            if (buttonPressed == false)
            {
                buttonPressed = true;

                //check if all books on shelf
                int bookcount = drawerSlots.Count(slot => slot.bookIndex != -1);
                if (bookcount != 4)
                {
                    //if not on shelf - flash book area
                    await FlashBooksAsync();
                }
                else 
                {
                    //check if books are all correct
                    if (calculator.DrawerCorrect(labels, GetCurrentList()))
                    {
                        //if correct order - show score window
                        ScorePopup(true);
                    }
                    else
                    {
                        //if incorrect - mange hearts
                        await UIController.HeartManager();
                        buttonPressed = false;
                    }
                }
            }
        }
        //---------------------------------------------------------------------------------------//
        #endregion
    }
}
//-----------------------------------------------oO END OF FILE Oo----------------------------------------------------------------------//