using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;
using PROG7312_POE.SharedWindows;

namespace PROG7312_POE.Classes
{
    /// <summary>
    /// class for handling the addition of text to image files
    /// </summary>
    public class TextHandler
    {
        //class for getting filepaths
        FilePaths filePaths = new FilePaths();
        //variable for random generation
        Random rand = new Random();


        #region Replacing Call Numbsers
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to add call numbers to the book cover image files
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string AddTextToImage(int index)
        {
            string callNumber = "";
            try
            {
                //get original book cover image
                System.Drawing.Image pngImage = System.Drawing.Image.FromFile(filePaths.OriginalCover(index));

                //Initialize Bitmap with same size as the image
                Bitmap bitmap = new Bitmap(pngImage.Width, pngImage.Height, pngImage.PixelFormat);

                //Create a new graphic with the bitmpa
                Graphics graphic = Graphics.FromImage(bitmap);

                //add the image to the graphic
                graphic.DrawImage(pngImage, new Rectangle(0, 0, bitmap.Width, bitmap.Height));

                //generate call number
                double number = GenerateNumbers();
                string letter = GenerateLetters();
                string text = number.ToString("000.000") + "\n  " + letter;

                //adjust font size based on DPI (source ChatGPT).
                float dpiY = graphic.DpiY;
                float fontSize = 16 * 96f / dpiY;

                //create text and add to graphic
                Font font = new Font("Arial", fontSize);
                PointF point = new PointF(120, 285f);
                SizeF textSize = graphic.MeasureString(text, font);
                graphic.TranslateTransform(point.X, point.Y);
                graphic.RotateTransform(-90);
                graphic.ScaleTransform(-1, 1);
                graphic.FillRectangle(Brushes.White, new RectangleF(new PointF(0, 0), textSize));
                graphic.DrawString(text, font, Brushes.Black, new PointF(0, 0));

                //Save the file
                bitmap.Save(filePaths.TextCover(index));

                //dispose of the bitmap and image
                bitmap.Dispose();
                pngImage.Dispose();

                //return generated call number
                callNumber = number.ToString("000.000") + " " + letter;
                return callNumber;
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
            return callNumber;
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Identifying Areas
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to add labels to drawer
        /// </summary>
        /// <param name="index"></param>
        public void AddTextToDrawer(int index, string text)
        {
            try
            {
                if (int.TryParse(text, out int result))
                {
                    text += "00";
                }

                //get original book cover image
                System.Drawing.Image pngImage = System.Drawing.Image.FromFile(filePaths.OriginalLabel());

                //Initialize Bitmap with same size as the image
                Bitmap bitmap = new Bitmap(pngImage.Width, pngImage.Height, pngImage.PixelFormat);

                //Create a new graphic with the bitmpa
                Graphics graphic = Graphics.FromImage(bitmap);

                //add the image to the graphic
                graphic.DrawImage(pngImage, new Rectangle(0, 0, bitmap.Width, bitmap.Height));

                //adjust font size based on DPI (source ChatGPT).
                float dpiY = graphic.DpiY;
                float fontSize = 32 * 96f / dpiY;

                //create text and add to graphic
                Font font = new Font("Arial", fontSize);
                PointF point = new PointF(95, 470);
                SizeF textSize = graphic.MeasureString(text, font);
                graphic.TranslateTransform(point.X, point.Y);
                graphic.RotateTransform(-90);
                graphic.ScaleTransform(1, 3);
                //graphic.FillRectangle(Brushes.White, new RectangleF(new PointF(0, 0), textSize));
                graphic.DrawString(text, font, Brushes.Black, new PointF(0, 0));

                //Save the file
                bitmap.Save(filePaths.LabelTexture(index));

                //dispose of the bitmap and image
                bitmap.Dispose();
                pngImage.Dispose();
                pngImage.Dispose();
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to add labels to the book cover image files
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public void AddTextToDrawerBook(int colour, int cover, string text)
        {
            try
            {
                if (int.TryParse(text, out int result))
                {
                    text += "00";
                }

                //get original book cover image
                System.Drawing.Image pngImage = System.Drawing.Image.FromFile(filePaths.OriginalCover(colour));

                //Initialize Bitmap with same size as the image
                Bitmap bitmap = new Bitmap(pngImage.Width, pngImage.Height, pngImage.PixelFormat);

                //Create a new graphic with the bitmpa
                Graphics graphic = Graphics.FromImage(bitmap);

                //add the image to the graphic
                graphic.DrawImage(pngImage, new Rectangle(0, 0, bitmap.Width, bitmap.Height));

                //string text = "Philosophy \nand psychology";

                //adjust font size based on DPI (source ChatGPT).
                float dpiY = graphic.DpiY;
                float fontSize = 13 * 96f / dpiY;

                //create text and add to graphic
                Font font = new Font("Arial", fontSize);
                PointF point = new PointF(190, 305);
                SizeF textSize = graphic.MeasureString(text, font);
                graphic.TranslateTransform(point.X, point.Y);
                //graphic.RotateTransform(-90);
                graphic.ScaleTransform(-1, 1);
                graphic.FillRectangle(Brushes.White, new RectangleF(new PointF(0, 0), textSize));
                graphic.DrawString(text, font, Brushes.Black, new PointF(0, 0));

                //Save the file
                bitmap.Save(filePaths.DrawerBookCover(cover));

                //dispose of the bitmap and image
                bitmap.Dispose();
                pngImage.Dispose();
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Finding Call Numbsers
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to add text to label of book holder
        /// </summary>
        public void AddTextToHolder(string text, int num)
        {
            text = FormatText(text, 20);

            //get original book cover image
            System.Drawing.Image pngImage = System.Drawing.Image.FromFile(filePaths.OriginalLabel());

            //Initialize Bitmap with same size as the image
            Bitmap bitmap = new Bitmap(pngImage.Width, pngImage.Height, pngImage.PixelFormat);

            //Create a new graphic with the bitmap
            Graphics graphic = Graphics.FromImage(bitmap);

            //add the image to the graphic
            graphic.DrawImage(pngImage, new Rectangle(0, 0, bitmap.Width, bitmap.Height));

            //adjust font size based on DPI (source ChatGPT).
            float dpiY = graphic.DpiY;
            float fontSize = 36 * 96f / dpiY;

            Font font = new Font("Arial", fontSize);
            SizeF textSize = graphic.MeasureString(text, font);

            //Adjust the X-coordinate based on the width of the text
            PointF point = new PointF(500 - textSize.Width / 2, 260);

            //Translate to the point where you want to draw the text
            graphic.TranslateTransform(point.X, point.Y);

            //Scale the text to prevent it from stretching
            float scaleX = 1.0f;
            float scaleY = 3.0f; 
            graphic.ScaleTransform(scaleX, scaleY);

            //Rotate the text 180 degrees
            graphic.RotateTransform(180);

            graphic.DrawString(text, font, Brushes.Black, new PointF(-textSize.Width / 2, -textSize.Height / 2));

            //Reset the transformation
            graphic.ResetTransform();

            //Save the file
            bitmap.Save(filePaths.LabelTexture(num));

            //dispose of the bitmap and image
            bitmap.Dispose();
            pngImage.Dispose();
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to add text to holder book
        /// </summary>
        public void AddTestToHolderBook(int colour, string text)
        {
            text = FormatText(text, 17);

            //get original book cover image
            System.Drawing.Image pngImage = System.Drawing.Image.FromFile(filePaths.OriginalCover(colour));

            //Initialize Bitmap with same size as the image
            Bitmap bitmap = new Bitmap(pngImage.Width, pngImage.Height, pngImage.PixelFormat);

            //Create a new graphic with the bitmpa
            Graphics graphic = Graphics.FromImage(bitmap);

            //add the image to the graphic
            graphic.DrawImage(pngImage, new Rectangle(0, 0, bitmap.Width, bitmap.Height));

            //adjust font size based on DPI (source ChatGPT).
            float dpiY = graphic.DpiY;
            float fontSize = 13 * 96f / dpiY;

            //create text and add to graphic
            Font font = new Font("Arial", fontSize);
            PointF point = new PointF(60, 365);
            SizeF textSize = graphic.MeasureString(text, font);
            graphic.TranslateTransform(point.X, point.Y);
            graphic.RotateTransform(-90);
            graphic.ScaleTransform(-1, 1);
            graphic.FillRectangle(Brushes.White, new RectangleF(new PointF(0, 0), textSize));
            graphic.DrawString(text, font, Brushes.Black, new PointF(0, 0));

            //Save the file
            bitmap.Save(filePaths.HolderBookCover());

            //dispose of the bitmap and image
            bitmap.Dispose();
            pngImage.Dispose();
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Format
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to format text to specified length
        /// ChatGPT used to assist creation of method
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        private string FormatText(string text, int maxLength)
        {
            //Split the text into words
            string[] words = text.Split(' ');

            //Initialize a StringBuilder to hold the final text
            StringBuilder finalText = new StringBuilder();

            //Initialize a StringBuilder to hold the current line
            StringBuilder line = new StringBuilder();

            foreach (string word in words)
            {
                //If adding the next word would make the line too long,
                //add the current line to the final text, then start a new line
                if (line.Length + word.Length > maxLength)
                {
                    finalText.AppendLine(line.ToString().TrimEnd());
                    line.Clear();
                }

                //Add the next word to the current line
                line.Append(word);
                line.Append(' ');
            }

            //Add the last line to the final text
            finalText.AppendLine(line.ToString().TrimEnd());

            return finalText.ToString();
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Generation
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method for generating random numbers in 000.000 format
        /// </summary>
        /// <returns></returns>
        private double GenerateNumbers()
        {
            double randomDouble = rand.NextDouble() * (999.999);
            randomDouble = Math.Round(randomDouble, 3);
            return randomDouble;
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method for generating 3 random letters
        /// </summary>
        /// <returns></returns>
        private string GenerateLetters()
        {
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string randomString = "";
            for (int i = 0; i < 3; i++)
            {
                int index = rand.Next(alphabet.Length);
                randomString += alphabet[index];
            }
            return randomString;
        }
        //---------------------------------------------------------------------------------------//
        #endregion
    }
}
//-----------------------------------------------oO END OF FILE Oo----------------------------------------------------------------------//