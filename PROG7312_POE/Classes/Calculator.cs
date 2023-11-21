using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using DeweyLibrary;
using PROG7312_POE.SharedWindows;

namespace PROG7312_POE.Classes
{
    /// <summary>
    /// class for calculating scores and checking answers
    /// </summary>
    internal class Calculator
    {
        #region Replacing Books
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to check if the current list of books on the self matches the correct list
        /// </summary>
        /// <param name="bookData"></param>
        /// <returns></returns>
        public bool CheckBookList(List<string> bookData)
        {
            bool isValid = false;
            try
            {
                //sort book list
                List<string> sortedList = new List<string>();
                sortedList.AddRange(bookData);
                sortedList.Sort();

                //check if current list matches sorted list
                isValid = sortedList.SequenceEqual(bookData);
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
            return isValid;
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// unused bubble sort to sort call numbers
        /// source: https://www.tutorialspoint.com/Bubble-Sort-program-in-Chash
        /// </summary>
        /// <param name="stringList"></param>
        /// <returns></returns>
        private List<string> BubbleSort(List<string> stringList)
        {
            string temp = "";
            for (int i = 0; i < stringList.Count(); i++)
            { 
                for (int j = 0; j < stringList.Count() - 1; j++) 
                {
                    if (string.Compare(stringList[j], stringList[j + 1]) < 0)
                    { 
                        temp = stringList[j + 1];
                        stringList[j + 1] = stringList[j];
                        stringList[j] = temp;
                    }
                }
            }
            return stringList;
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to calucate the score for the replacing books game
        /// </summary>
        /// <param name="bookData"></param>
        /// <param name="totalTime"></param>
        /// <param name="livesLeft"></param>
        /// <returns></returns>
        public int CalcReplaceScore(List<string> bookData, int totalTime, int livesLeft)
        {
            int score = 0;
            try
            {
                //define the weighing for each factor
                const int bookWeight = 100;
                const int timeWeight = 10;
                const int lifeWeight = 500;

                //calculate score
                int bookScore = CorrectBooks(bookData) * bookWeight;
                int timeScore = totalTime * timeWeight;
                int lifeScore = livesLeft * lifeWeight;
                score = bookScore + lifeScore - timeScore;
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
            return score;
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method for calculating how many books are in the correct position on the shelf
        /// </summary>
        /// <param name="bookData"></param>
        /// <returns></returns>
        private int CorrectBooks(List<string> bookData)
        {
            int correct = 0;
            try
            {
                //sort book list
                List<string> sortedList = new List<string>(bookData);
                sortedList.Sort();
                //count the number of correct books
                correct = bookData.Zip(sortedList, (b, s) => b == s).Count(match => match);
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
            return correct;
        }
        //---------------------------------------------------------------------------------------//
        #endregion

        #region Identifying Areas
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to get amount of correct books in drawer
        /// </summary>
        /// <param name="lables"></param>
        /// <param name="slots"></param>
        /// <returns></returns>
        private int DrawerCorrectCount(List<string> labels, List<string> slots)
        {
            Dictionary<int, string> dewey = DeweyDecimal.GetDewey();
            int correct = 0;

            for (int i = 0; i < labels.Count; i++)
            {
                if (string.IsNullOrEmpty(slots[i]))
                    continue;

                if (int.TryParse(labels[i], out int labelNum) && dewey.TryGetValue(labelNum, out string labelValue) && slots[i].Equals(labelValue))
                {
                    correct++;
                }
                else if (int.TryParse(slots[i], out int slotNum) && dewey.TryGetValue(slotNum, out string slotValue) && labels[i].Equals(slotValue))
                {
                    correct++;
                }
            }
            return correct;
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to check if the books in the drawer are all correct
        /// </summary>
        /// <param name="lables"></param>
        /// <param name="slots"></param>
        /// <returns></returns>
        public bool DrawerCorrect(List<string> lables, List<string> slots)
        { 
            bool correct = false;
            int correctCount = DrawerCorrectCount(lables, slots);
            if (correctCount == 4)
            { 
                correct = true;
            }
            return correct;
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to calculate the score for the identifying areas game
        /// </summary>
        /// <param name="lables"></param>
        /// <param name="slots"></param>
        /// <param name="totalTime"></param>
        /// <param name="livesLeft"></param>
        /// <returns></returns>
        public int CalcIdentifyAreaScore(List<string> lables, List<string> slots, int totalTime, int livesLeft)
        {
            int score = 0;
            try
            {
                //define the weighing for each factor
                const int bookWeight = 200;
                const int timeWeight = 10;
                const int lifeWeight = 500;

                //calculate score
                int bookScore = DrawerCorrectCount(lables, slots) * bookWeight;
                int timeScore = totalTime * timeWeight;
                int lifeScore = livesLeft * lifeWeight;
                score = bookScore + lifeScore - timeScore;
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
            return score;
        }
        //---------------------------------------------------------------------------------------//
        #endregion

        #region Finding Call Numbers
        //---------------------------------------------------------------------------------------//
        public int CalcFindNumberScore(int totalTime, int livesLeft, int level)
        {
            int score = 0;
            try
            {
                //define the weighing for each factor
                const int levelWeight = 200;
                const int timeWeight = 10;
                const int lifeWeight = 500;

                //calculate score
                int bookScore = level * levelWeight;
                int timeScore = totalTime * timeWeight;
                int lifeScore = livesLeft * lifeWeight;
                score = bookScore + lifeScore - timeScore;
            }
            catch (Exception ex)
            {
                ErrorWindow errorWindow = new ErrorWindow(ex.Message);
                errorWindow.ShowDialog();
            }
            return score;
        }
        //---------------------------------------------------------------------------------------//
        #endregion
    }
}
//-----------------------------------------------oO END OF FILE Oo----------------------------------------------------------------------//