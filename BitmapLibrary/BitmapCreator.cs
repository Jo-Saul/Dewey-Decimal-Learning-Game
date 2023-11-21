using System;
using System.Windows.Media.Imaging;

namespace BitmapLibrary
{
    /// <summary>
    /// class to create bitmaps
    /// </summary>
    public class BitmapCreator
    {
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method for creating bitmap images from specified filepath
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static BitmapImage CreateBitmap(string filePath)
        {
            BitmapImage bitmap = new BitmapImage();
            try
            {
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(filePath, UriKind.Absolute);
                bitmap.EndInit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return bitmap;
        }
        //---------------------------------------------------------------------------------------//
    }
}
//-----------------------------------------------oO END OF FILE Oo----------------------------------------------------------------------//