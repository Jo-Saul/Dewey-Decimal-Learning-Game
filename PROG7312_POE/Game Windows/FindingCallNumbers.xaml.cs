using DeweyLibrary;
using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using PROG7312_POE.SlotClasses;
using PROG7312_POE.Classes;
using PROG7312_POE.SharedWindows;

namespace PROG7312_POE.GameWindows
{
    /// <summary>
    /// Interaction logic for FindingCallNumbers.xaml
    /// </summary>
    public partial class FindingCallNumbers : Window
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
        //dewey handler for dewey operations
        private readonly DeweyDecimal deweyHandler = new DeweyDecimal();

        //---------------------------- Lists ----------------------------
        //list of slot spaces
        private List<HolderSlots> holderSlots = new List<HolderSlots>();
        //list of dewey for each lable of the holders
        private List<DeweyDecimalClass> HolderLables = new List<DeweyDecimalClass>();
        //list of holder models
        private List<ModelVisual3D> holderlist = new List<ModelVisual3D>();

        //---------------------------- Media Players ----------------------------
        //sound effects
        private MediaPlayer effectPlayer = new MediaPlayer();

        //---------------------------- Visuals ----------------------------
        //3D viewport variable
        private Viewport3D viewPort = new Viewport3D();
        //book visual
        private ModelVisual3D BookVisual3D = new ModelVisual3D();
        //bool to check if a button is active
        private bool buttonPressed = false;
        //book area geometry
        private GeometryModel3D bookSpaceGeo = new GeometryModel3D();

        //---------------------------- Dewey ----------------------------
        //list of dewey trees - 1 per level
        private List<RBDeweyTree> deweyTrees = new List<RBDeweyTree>();
        //variable for holder the current corret answer/question
        private DeweyDecimalClass CorrectDewey = new DeweyDecimalClass();
        
        //current dewey catefgory level
        private int currentLevel = 1;
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Constructor
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// constructor for finding call numbers game
        /// </summary>
        public FindingCallNumbers()
        {
            InitializeComponent();
            
            //populate trees
            deweyTrees = deweyHandler.GetDeweyTree();
            
            //visual set up
            LightandCamera();
            BookSetup();
            HolderSlotSetup();
            HolderSetup();
            BookRowSpace();
            BackGroundSetUp();
            BackgroundFade();

            //set effect volume to max
            effectPlayer.Volume = 1;

            //Set up UI controller
            UIController.NoHeartsLeft += () => ScorePopup(false);
            UIController.HomeButtonClicked += () => CloseWindow();
            UIController.AdjustForFindCallNumber();

            //setup mouse movement
            MouseMovement();

            //play countdown
            StartCountDown();

            //add viewport to window gird
            this.FindCallGrid.Children.Insert(0, viewPort);
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
        /// method to set up book holders
        /// </summary>
        private void HolderSetup()
        {
            try
            {
                //make holder lables
                if (currentLevel == 1)
                {
                    FirstLevelLabes();
                }
                else if (currentLevel == 2)
                {
                    SecondLevelLables();
                }
                else if (currentLevel == 3)
                {
                    ThirdLevelLabel();
                }

                //order lable list by number
                HolderLables = HolderLables.OrderBy(a => a.Number).ToList();

                for (int i = 1; i <= 4; i++)
                {
                    string text = HolderLables[i - 1].Number.ToString("000") + " " + HolderLables[i - 1].Description;
                    textHandler.AddTextToHolder(text, i);
                    ModelVisual3D holderVisual3D = new ModelVisual3D
                    {
                        Content = importer.Load(filePaths.Holder(i)), //import objetc
                        Transform = new TranslateTransform3D(-25 + (i * 10), 5, -20) //location
                    };
                    holderlist.Add(holderVisual3D);
                    //add visual to view port
                    viewPort.Children.Add(holderVisual3D);
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
        /// method to set up shelf slots
        /// </summary>
        private void HolderSlotSetup()
        {
            try
            {
                // Set start parameters
                double slotXLeft = -18.5;
                double slotXRight = -11.5;

                // Set the distance between the slots
                double distanceBetweenSlots = 10;

                // Create 10 slots
                for (int i = 0; i < 4; i++)
                {
                    // Create new slot
                    holderSlots.Add(new HolderSlots
                    {
                        SlotXRight = slotXRight,
                        SlotXLeft = slotXLeft,
                        bookIn = false
                    });

                    slotXLeft += distanceBetweenSlots;
                    slotXRight += distanceBetweenSlots;
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
        /// method to set up 4 book objects
        /// </summary>
        private void BookSetup()
        {
            try
            {
                //create a list of cover indices and shuffle it
                List<int> coverIndices = Enumerable.Range(1, 10).OrderBy(x => Guid.NewGuid()).ToList();

                //get dewey entry
                CorrectDewey = deweyHandler.GetRandom3rdDewey(deweyTrees[2]);

                //set text of book
                textHandler.AddTestToHolderBook(coverIndices[0], CorrectDewey.Description);


                //Create a new ModelVisual3D object and load the .obj file
                BookVisual3D = new ModelVisual3D
                {
                    Content = importer.Load(filePaths.HolderBook()),
                    //set start position and add visual to viewport
                    Transform = new TranslateTransform3D(new Vector3D(0, -3.5, -10))
                };
                //add visual to viewport
                viewPort.Children.Add(BookVisual3D);
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
                        new Point3D(-3, -7.5, -10),
                        new Point3D(3.7, -7.5, -10),
                        new Point3D(3.7, 0.5, -10),
                        new Point3D(-3, 0.5, -10)
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
        /// method to set up background object
        /// Note: The transformation are not actually overwritten when the code is executed
        /// </summary>
        private void BackGroundSetUp()
        {
            try
            {
                ModelVisual3D shelveVisual3D = new ModelVisual3D
                {
                    Content = importer.Load(filePaths.Shelf()), //import objetc
                    Transform = new TranslateTransform3D(0, -1.45, 25) //location
                };

                // Create a transformation group to apply multiple transformations
                Transform3DGroup transformGroup = new Transform3DGroup();
                transformGroup.Children.Add(shelveVisual3D.Transform);

                // Create and add a rotation transformation for y-axis
                AxisAngleRotation3D rotationY = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 180); // 45 degree rotation around y-axis
                RotateTransform3D rotateTransformY = new RotateTransform3D(rotationY);
                transformGroup.Children.Add(rotateTransformY);

                // Create and add a rotation transformation for z-axis
                AxisAngleRotation3D rotationZ = new AxisAngleRotation3D(new Vector3D(0, 0, 1), 90); // 45 degree rotation around x-axis
                RotateTransform3D rotateTransformZ = new RotateTransform3D(rotationZ);
                transformGroup.Children.Add(rotateTransformZ);

                // Apply the transformation group to the 3D object
                shelveVisual3D.Transform = transformGroup;

                // Create and add a scale transformation
                ScaleTransform3D scaleTransform = new ScaleTransform3D(1.46, 1.46, 1.46); // scale up by a factor of 2
                transformGroup.Children.Add(scaleTransform);

                //add visual to view port
                viewPort.Children.Add(shelveVisual3D);
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to add backgeound fade
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
                Material material = new DiffuseMaterial(new SolidColorBrush(System.Windows.Media.Color.FromArgb(150, 54, 20, 3)));

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
                
                //Calculate the current scale factor
                //ChatGPT was used to calculate the scale
                Window window = Application.Current.MainWindow;
                var source = PresentationSource.FromVisual(window);
                Matrix m = source.CompositionTarget.TransformToDevice;
                double dpiX = m.M11 * 0.96;
                double dpiY = m.M22 * 0.96;
                double _scaleFactor = (dpiX / 9.6)/4;

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
                        if (BookVisual3D == meshResult.VisualHit)
                        {
                            //record the starting position of the mouse and capture it
                            _startPos = e.GetPosition(viewPort);
                            viewPort.CaptureMouse();
                            _isDragging = true;

                            //store the clicked object
                            _clickedObject = meshResult.VisualHit as ModelVisual3D;
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
                        translateTransform.OffsetZ = -10;
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

                        // Find current book location and remove
                        var currentSlot = holderSlots.FirstOrDefault(slot => slot.bookIn);
                        if (currentSlot != null)
                        {
                            currentSlot.bookIn = false;
                        }

                        bool bookInSlot = false;

                        //if book in holder section
                        if (translateTransform.OffsetY >= 1.5 && translateTransform.OffsetY <= 11)
                        {
                            // Check for empty slot to place book in
                            var holderSlot = holderSlots.FirstOrDefault(slot => translateTransform.OffsetX >= slot.SlotXLeft && translateTransform.OffsetX <= slot.SlotXRight);

                            //if shelf slot is found
                            if (holderSlot != null)
                            {
                                // Set location
                                translateTransform.OffsetX = ((holderSlot.SlotXLeft + holderSlot.SlotXRight) / 2) - 0.2;
                                translateTransform.OffsetY = 5;
                                translateTransform.OffsetZ = -19;

                                // Set book slot
                                holderSlot.bookIn = true;
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
                            translateTransform.OffsetX = 0;
                            translateTransform.OffsetY = -3.5;
                            translateTransform.OffsetZ = -10;
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


        #region General Methods
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to close window - remove visual/audio assests and closes window
        /// NB: removing any line will break the program
        /// </summary>
        private void CloseWindow()
        {
            try
            {
                //close controller (time)
                UIController.CloseController();

                //clear holders
                for (int i = 0; i < holderlist.Count; i++)
                {
                    viewPort.Children.Remove(holderlist[i]);
                    holderlist[i].Content = null;
                }
                holderlist.Clear();

                //clear book
                BookVisual3D.Content = null;
                viewPort.Children.Remove(BookVisual3D);

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
                int score = calculator.CalcFindNumberScore((int)UIController.time.TotalSeconds, UIController.heartCount, currentLevel);

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
        /// method to reset the game
        /// </summary>
        private void Reset()
        {
            try
            {
                //reset level 
                currentLevel = 1;

                //set back rectangle to hidden
                rectBackdrop.Visibility = Visibility.Visible;
                //stop timer
                UIController.timer.Stop();

                //clear book
                viewPort.Children.Remove(BookVisual3D);
                BookVisual3D.Content = null;

                //clear holders list
                for (int i = 0; i < holderlist.Count; i++)
                {
                    holderSlots[i].bookIn = false;
                    viewPort.Children.Remove(holderlist[i]);
                    holderlist[i].Content = null;
                }
                holderlist.Clear();
                holderlist = new List<ModelVisual3D>();

                //garbage collet
                GC.Collect();
                GC.WaitForPendingFinalizers();

                //reset book
                BookSetup();

                //reset holders
                HolderSetup();

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


        #region Generate Holder Lables
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to generate first level lables for the holders
        /// </summary>
        private void FirstLevelLabes()
        {
            //remake lists
            HolderLables = new List<DeweyDecimalClass>();
            HashSet<int> labelNumbers = new HashSet<int>();

            //get correct lable
            int cat = CorrectDewey.Number / 100;
            DeweyDecimalClass temp = deweyTrees[0].FindByCallNumber(cat * 100);
            HolderLables.Add(temp);
            labelNumbers.Add(temp.Number);

            //add 3 incorrect lables
            for (int i = 0; i < 3; i++)
            {
                do
                {
                    temp = deweyHandler.GetDeweyByLevel(deweyTrees[0], 1);
                } 
                while (labelNumbers.Contains(temp.Number));

                HolderLables.Add(temp);
                labelNumbers.Add(temp.Number);
            }
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to generate second level lables for the holders
        /// </summary>
        private void SecondLevelLables()
        {
            //remake lists
            HolderLables = new List<DeweyDecimalClass>();
            HashSet<int> labelNumbers = new HashSet<int>();

            //get current second level category
            int cat = CorrectDewey.Number / 10;

            //get correct lable
            DeweyDecimalClass temp = deweyTrees[1].FindByCallNumber(cat * 10);
            HolderLables.Add(temp);
            labelNumbers.Add(temp.Number);

            //generate 3 incorrect lables
            cat = CorrectDewey.Number / 100;
            for (int i = 0; i < 3; i++)
            {
                do
                {
                    temp = deweyHandler.GetDeweyByLevel(deweyTrees[1], 2, cat);
                } while (labelNumbers.Contains(temp.Number));

                HolderLables.Add(temp);
                labelNumbers.Add(temp.Number);
            }
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to generate thrid level lables for the holders
        /// </summary>
        private void ThirdLevelLabel()
        {
            //remake lists
            HolderLables = new List<DeweyDecimalClass>();
            HashSet<int> labelNumbers = new HashSet<int>();

            //get correct 3rd level
            HolderLables.Add(CorrectDewey);
            labelNumbers.Add(CorrectDewey.Number);

            //get current 3rd level cateogry
            int cat = CorrectDewey.Number / 10;

            //get 3 incorrect
            for (int i = 0; i < 3; i++)
            {
                DeweyDecimalClass temp;
                do
                {
                    temp = deweyHandler.GetDeweyByLevel(deweyTrees[2], 3, cat);
                } while (labelNumbers.Contains(temp.Number));

                HolderLables.Add(temp);
                labelNumbers.Add(temp.Number);
            }
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Next Level
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to load next level
        /// </summary>
        private async void NextLevel()
        {
            try
            {
                //play sound effect
                effectPlayer.Stop();
                effectPlayer.Open(new Uri(filePaths.SlideAudio()));
                effectPlayer.Play();

                //return book to orignal position
                ReturnBookToStart();

                //move holders
                MoveHoldersUp();

                //wait for the duration of the animation
                await Task.Delay(500);

                //reset holder
                for (int i = 0; i < holderlist.Count; i++)
                {
                    viewPort.Children.Remove(holderlist[i]);
                    holderlist[i].Content = null;
                }
                holderlist.Clear();

                //garbage collet
                GC.Collect();
                GC.WaitForPendingFinalizers();

                //set up next holders
                HolderSetup();

                //move holders down
                MoveHoldersDown();

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
        /// method to return the book to the start position
        /// </summary>
        private void ReturnBookToStart()
        {
            try
            {
                //Get the existing TranslateTransform3D or create a new one if it doesn't exist
                TranslateTransform3D translateTransform = BookVisual3D.Transform as TranslateTransform3D;
                if (translateTransform == null)
                {
                    translateTransform = new TranslateTransform3D();
                    BookVisual3D.Transform = translateTransform;
                }

                //Create a DoubleAnimation to animate the OffsetX property
                DoubleAnimation animationX = new DoubleAnimation
                {
                    From = translateTransform.OffsetX, //current position
                    To = 0, // target position
                    Duration = TimeSpan.FromSeconds(1), //duration of animation
                    FillBehavior = FillBehavior.Stop //animation stops affecting the property when completed
                };
                translateTransform.BeginAnimation(TranslateTransform3D.OffsetXProperty, animationX);

                //annimation for offsetY
                DoubleAnimation animationY = new DoubleAnimation
                {
                    From = translateTransform.OffsetY,
                    To = -3.5,
                    Duration = TimeSpan.FromSeconds(1),
                    FillBehavior = FillBehavior.Stop // animation stops affecting the property when completed
                };
                translateTransform.BeginAnimation(TranslateTransform3D.OffsetYProperty, animationY);

                //animation for offsetZ
                DoubleAnimation animationZ = new DoubleAnimation
                {
                    From = translateTransform.OffsetZ,
                    To = -10,
                    Duration = TimeSpan.FromSeconds(1),
                    FillBehavior = FillBehavior.Stop // animation stops affecting the property when completed
                };
                translateTransform.BeginAnimation(TranslateTransform3D.OffsetZProperty, animationZ);

                //set position after animation
                translateTransform.OffsetX = 0;
                translateTransform.OffsetY = -3.5;
                translateTransform.OffsetZ = -10;
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to animate holders upwards
        /// </summary>
        private void MoveHoldersUp()
        {
            try
            {
                //for each holders
                for (int i = 0; i < holderlist.Count; i++)
                {
                    //Get the existing TranslateTransform3D or create a new one if it doesn't exist
                    TranslateTransform3D translateTransform = holderlist[i].Transform as TranslateTransform3D;
                    if (translateTransform == null)
                    {
                        translateTransform = new TranslateTransform3D();
                        BookVisual3D.Transform = translateTransform;
                    }

                    //create animation
                    DoubleAnimation animation = new DoubleAnimation
                    {
                        From = 5, //starting position
                        To = 24, //ending position (downwards)
                        Duration = new Duration(TimeSpan.FromSeconds(0.5)), //duration
                    };

                    //apply animation to the Y property of the holder transform
                    translateTransform.BeginAnimation(TranslateTransform3D.OffsetYProperty, animation);
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
        /// method to animate holders downwards
        /// </summary>
        private void MoveHoldersDown()
        {
            try
            {
                //for each holder
                for (int i = 0; i < holderlist.Count; i++)
                {
                    //Get the existing TranslateTransform3D or create a new one if it doesn't exist
                    TranslateTransform3D translateTransform = holderlist[i].Transform as TranslateTransform3D;
                    if (translateTransform == null)
                    {
                        translateTransform = new TranslateTransform3D();
                        BookVisual3D.Transform = translateTransform;
                    }

                    //create animation
                    DoubleAnimation animation = new DoubleAnimation
                    {
                        From = 24, //starting position
                        To = 5, //ending position (downwards)
                        Duration = new Duration(TimeSpan.FromSeconds(0.5)), //duration
                    };

                    //apply animation to the Y property of the bookselfs transform
                    translateTransform.BeginAnimation(TranslateTransform3D.OffsetYProperty, animation);
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


        #region Buttons
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// event handler for done button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnDone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (buttonPressed) return; //check if button press already
                buttonPressed = true;

                //Check if book in holder
                bool inholder = holderSlots.Any(slot => slot.bookIn);

                if (!inholder)
                {
                    //If not in holder - flash book area
                    await FlashBooksAsync();
                    return;
                }

                //Get holder book placed in
                int holderindex = holderSlots.FindIndex(holder => holder.bookIn);

                //Check correct
                bool correct = false;
                int temp = CorrectDewey.Number;
                switch (currentLevel)
                {
                    case 1: //if level on
                        temp /= 100;
                        temp *= 100;
                        break;
                    case 2: //if level 2
                        temp /= 10;
                        temp *= 10;
                        break;
                }
                //check if correct answer match current holder
                correct = HolderLables[holderindex].Number == temp;

                if (correct)
                {
                    //if last level complete
                    if (currentLevel == 3)
                    {
                        //Victory
                        ScorePopup(true);
                    }
                    else
                    {
                        //Move down a level
                        currentLevel++;
                        NextLevel();
                    }
                }
                else
                {
                    //Lose life
                    await UIController.HeartManager();
                    buttonPressed = false;
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
    }
}
//-----------------------------------------------oO END OF FILE Oo----------------------------------------------------------------------//