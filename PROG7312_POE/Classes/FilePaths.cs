using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PROG7312_POE.Classes
{
    /// <summary>
    /// Class for filepath locations
    /// all objects were custom created in Blender 3.4
    /// all images were custom created using Sketchbook Pro
    /// </summary>
    class FilePaths
    {
        //string to get current directory of application
        private readonly string currentDirectory = System.IO.Directory.GetCurrentDirectory();


        //3D Assests
        //====================================================================================================================
        
        #region Shelves + Shelve Books
        //---------------------------------------------------------------------------------------//
        private readonly string ShelfPath = @"\3D Assests\shelve.obj";
        /// <summary>
        /// method to get empty shelf object
        /// </summary>
        /// <returns></returns>
        public string Shelf()
        {
            return currentDirectory + ShelfPath;
        }
        //---------------------------------------------------------------------------------------//
        private readonly string FullShelfPath = @"\3D Assests\fullshelf.obj";
        /// <summary>
        /// method to get full shelf object
        /// </summary>
        /// <returns></returns>
        public string FullShelf()
        {
            return currentDirectory + FullShelfPath;
        }
        //---------------------------------------------------------------------------------------//
        private readonly string BookPath = @"\3D Assests\book";
        /// <summary>
        /// method to get shelf book object
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string Book(int num)
        {
            return currentDirectory + BookPath + num + ".obj";
        }
        //---------------------------------------------------------------------------------------//
        private readonly string TextCoverPath = @"\3D Assests\CoverBase";
        /// <summary>
        /// method to get specified book cover image with text
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string TextCover(int num)
        {
            return currentDirectory + TextCoverPath + num + ".png";
        }
        //---------------------------------------------------------------------------------------//
        #endregion

        #region Drawer + Drawer Books
        //---------------------------------------------------------------------------------------//
        private readonly string DrawerPath = @"\3D Assests\drawer.obj";
        /// <summary>
        /// method to get drawer object
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string Drawer()
        {
            return currentDirectory + DrawerPath;
        }
        //---------------------------------------------------------------------------------------//
        private readonly string DrawerBookPath = @"\3D Assests\drawerbook";
        /// <summary>
        /// method to get drawer book object
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string DrawerBook(int num)
        {
            return currentDirectory + DrawerBookPath + num + ".obj";
        }
        //---------------------------------------------------------------------------------------//
        private readonly string DrawerBookCoverPath = @"\3D Assests\DrawerCover";
        /// <summary>
        /// method to get drawer book cover image
        /// </summary>
        /// <returns></returns>
        public string DrawerBookCover(int num)
        {
            return currentDirectory + DrawerBookCoverPath + num + ".png";
        }
        //---------------------------------------------------------------------------------------//





        #endregion

        #region Labels
        //---------------------------------------------------------------------------------------//
        private readonly string OriginalLabelPath = @"\3D Assests\OGLabelTexture.png";
        /// <summary>
        /// method to get original lable texture image for drawer
        /// </summary>
        /// <returns></returns>
        public string OriginalLabel()
        {
            return currentDirectory + OriginalLabelPath;
        }
        //---------------------------------------------------------------------------------------//
        private readonly string LabelPath = @"\3D Assests\LabelTexture";
        /// <summary>
        /// method to get label texture image for drawer
        /// </summary>
        /// <returns></returns>
        public string LabelTexture(int num)
        {
            return currentDirectory + LabelPath + num + ".png";
        }
        //---------------------------------------------------------------------------------------//
        #endregion

        #region Holder + Holder Book
        //---------------------------------------------------------------------------------------//
        private readonly string HolderPath = @"\3D Assests\holder";
        /// <summary>
        /// method to get holder object
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string Holder(int num)
        {
            return currentDirectory + HolderPath + num + ".obj";
        }
        //---------------------------------------------------------------------------------------//
        private readonly string HolderBookPath = @"\3D Assests\holderbook.obj";
        /// <summary>
        /// method to get holder object
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string HolderBook()
        {
            return currentDirectory + HolderBookPath;
        }
        //---------------------------------------------------------------------------------------//
        private readonly string HolderBookCoverPath = @"\3D Assests\HolderCover.png";
        /// <summary>
        /// method to get holder object
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string HolderBookCover()
        {
            return currentDirectory + HolderBookCoverPath;
        }
        //---------------------------------------------------------------------------------------//
        #endregion

        #region Book Covers
        //All Books
        //---------------------------------------------------------------------------------------//
        private readonly string OriginalCoverPath = @"\3D Assests\OGCoverBase";
        /// <summary>
        /// method to get specified original book cover image
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string OriginalCover(int num) 
        {
            return currentDirectory + OriginalCoverPath + num + ".png";
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        //Image Assests
        //====================================================================================================================
        #region Hearts
        //---------------------------------------------------------------------------------------//
        private readonly string HeartFullPath = @"\Image Assests\Heart Full.png";
        /// <summary>
        /// method to get heart full image
        /// </summary>
        /// <returns></returns>
        public string HeartFullImage()
        {
            return currentDirectory + HeartFullPath;
        }
        //---------------------------------------------------------------------------------------//
        private readonly string HeartBrokenPath = @"\Image Assests\Heart Broken.png";
        /// <summary>
        /// method to get heart broken image
        /// </summary>
        /// <returns></returns>
        public string HeartBrokenImage()
        {
            return currentDirectory + HeartBrokenPath;
        }
        //---------------------------------------------------------------------------------------//
        private readonly string HeartGreyPath = @"\Image Assests\Heart Grey.png";
        /// <summary>
        /// method to get heart grey image
        /// </summary>
        /// <returns></returns>
        public string HeartGreyImage()
        {
            return currentDirectory + HeartGreyPath;
        }
        //---------------------------------------------------------------------------------------//
        #endregion

        #region Countdown
        //---------------------------------------------------------------------------------------//
        private readonly string OnePath = @"\Image Assests\One image.png";
        /// <summary>
        /// method to get one number image
        /// </summary>
        /// <returns></returns>
        public string OneImage()
        {
            return currentDirectory + OnePath;
        }
        //---------------------------------------------------------------------------------------//
        private readonly string TwoPath = @"\Image Assests\Two image.png";
        /// <summary>
        /// method to get two number image
        /// </summary>
        /// <returns></returns>
        public string TwoImage()
        {
            return currentDirectory + TwoPath;
        }
        //---------------------------------------------------------------------------------------//
        private readonly string ThreePath = @"\Image Assests\Three image.png";
        /// <summary>
        /// method to get three number image
        /// </summary>
        /// <returns></returns>
        public string ThreeImage()
        {
            return currentDirectory + ThreePath;
        }
        //---------------------------------------------------------------------------------------//
        private readonly string GoIamgePath = @"\Image Assests\Go image.png";
        /// <summary>
        /// method to get go text image
        /// </summary>
        /// <returns></returns>
        public string GoImage()
        {
            return currentDirectory + GoIamgePath;
        }
        //---------------------------------------------------------------------------------------//
        #endregion

        //Sound Assests
        //====================================================================================================================
        #region Background Music
        //---------------------------------------------------------------------------------------//
        private readonly string StartScreenPath = @"\Sound Assests\Start Screen.wav";
        /// <summary>
        /// method to get start screen background audio
        /// source :https://youtu.be/34-Im71tqpI?list=PLWJhE2gHTuXLq1HtinYhnDo-C8VW9vKvk
        /// </summary>
        /// <returns></returns>
        public string StartScreenAudio() 
        { 
            return currentDirectory + StartScreenPath;
        }
        //---------------------------------------------------------------------------------------//
        private readonly string ReplaceBooksPath = @"\Sound Assests\Replacing Books.wav";
        /// <summary>
        /// method to get replacing books background audio
        /// source: https://youtu.be/kvi0nyk5fMc?list=PLWJhE2gHTuXLq1HtinYhnDo-C8VW9vKvk
        /// </summary>
        /// <returns></returns>
        public string ReplaceBooksAudio()
        {
            return currentDirectory + ReplaceBooksPath;
        }
        //---------------------------------------------------------------------------------------//
        private readonly string DefeatPath = @"\Sound Assests\Defeat.wav";
        /// <summary>
        /// method to get defeat background audio
        /// source: https://youtu.be/DwXUWL9Kqhs?list=PLWJhE2gHTuXLq1HtinYhnDo-C8VW9vKvk
        /// </summary>
        /// <returns></returns>
        public string DefeatAudio()
        {
            return currentDirectory + DefeatPath;
        }
        //---------------------------------------------------------------------------------------//
        private readonly string VictoryPath = @"\Sound Assests\Victory.wav";
        /// <summary>
        /// method to get vicotry background audio
        /// source: https://youtu.be/usj0k7aXtq8?list=PLWJhE2gHTuXLq1HtinYhnDo-C8VW9vKvk
        /// </summary>
        /// <returns></returns>
        public string VictoryAudio()
        {
            return currentDirectory + VictoryPath;
        }
        //---------------------------------------------------------------------------------------//
        private readonly string IdentifyAreasPath = @"\Sound Assests\Identifying Areas.wav";
        /// <summary>
        /// method to get identifying areas background audio
        /// source: https://www.youtube.com/watch?v=dMjp4ERQ20M
        /// </summary>
        /// <returns></returns>
        public string IdentifyAreasAudio()
        {
            return currentDirectory + IdentifyAreasPath;
        }
        //---------------------------------------------------------------------------------------//
        private readonly string FindCallNumebrPath = @"\Sound Assests\Find Call Number.wav";
        /// <summary>
        /// method to get identifying areas background audio
        /// source: https://www.youtube.com/watch?v=dMjp4ERQ20M
        /// </summary>
        /// <returns></returns>
        public string FindCallNumberAudio()
        {
            return currentDirectory + FindCallNumebrPath;
        }
        //---------------------------------------------------------------------------------------//
        #endregion

        #region Effects
        //---------------------------------------------------------------------------------------//
        private readonly string StartButtonPath = @"\Sound Assests\Start Button.wav";
        /// <summary>
        /// method to get start screen button audio
        /// source: https://www.youtube.com/watch?v=-D2iL1GYdx0
        /// </summary>
        /// <returns></returns>
        public string StartButtonAudio()
        {
            return currentDirectory + StartButtonPath;
        }
        //---------------------------------------------------------------------------------------//
        private readonly string BookPlacePath = @"\Sound Assests\Book Place.wav";
        /// <summary>
        /// method to get book placing effect audio
        /// source: https://www.youtube.com/watch?v=22N8Rf6rSVg
        /// </summary>
        /// <returns></returns>
        public string BookPlaceAudio()
        {
            return currentDirectory + BookPlacePath;
        }
        //---------------------------------------------------------------------------------------//
        private readonly string BeepPath = @"\Sound Assests\Beep.wav";
        /// <summary>
        /// method to get beep audio
        /// custom made with Chrome Music Lab
        /// </summary>
        /// <returns></returns>
        public string BeepAudio()
        {
            return currentDirectory + BeepPath;
        }
        //---------------------------------------------------------------------------------------//
        private readonly string LifeLossPath = @"\Sound Assests\Life Loss.wav";
        /// <summary>
        /// method to get life loss audio
        /// custom made with Chrome Music Lab
        /// </summary>
        /// <returns></returns>
        public string LifeLossAudio()
        {
            return currentDirectory + LifeLossPath;
        }
        //---------------------------------------------------------------------------------------//
        private readonly string ButtonPath = @"\Sound Assests\Button Press.wav";
        /// <summary>
        /// method to get button click audio
        /// source: https://www.youtube.com/watch?v=-D2iL1GYdx0
        /// </summary>
        /// <returns></returns>
        public string ButtonAudio()
        {
            return currentDirectory + ButtonPath;
        }
        //---------------------------------------------------------------------------------------//
        private readonly string TickPath = @"\Sound Assests\Time Tick.wav";
        /// <summary>
        /// method to get time tick audio
        /// custom made with Chrome Music Lab
        /// </summary>
        /// <returns></returns>
        public string TimeTickAudio()
        {
            return currentDirectory + TickPath;
        }
        //---------------------------------------------------------------------------------------//
        private readonly string GoAudioPath = @"\Sound Assests\go audio.wav";
        /// <summary>
        /// method to get time tick audio
        /// custom made with Chrome Music Lab
        /// </summary>
        /// <returns></returns>
        public string GoAudio()
        {
            return currentDirectory + GoAudioPath;
        }
        //---------------------------------------------------------------------------------------//
        private readonly string SlidePath = @"\Sound Assests\Holder Slide.wav";
        /// <summary>
        /// method to get wood slide audio
        /// source: https://www.youtube.com/watch?v=aVcvSv-YKDk
        /// </summary>
        /// <returns></returns>
        public string SlideAudio()
        {
            return currentDirectory + SlidePath;
        }
        //---------------------------------------------------------------------------------------//
        #endregion
    }
}
//-----------------------------------------------oO END OF FILE Oo----------------------------------------------------------------------//