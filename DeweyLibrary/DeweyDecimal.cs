using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DeweyLibrary
{
    public class DeweyDecimal
    {
        //variable used for random generation
        private Random random = new Random();


        #region Populate Data Structures
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to populate dictrionary with dewey data
        /// </summary>
        public static Dictionary<int, string> GetDewey()
        {
            //map dewey to corresponding dictionary
            Dictionary<int, string> dewey = new Dictionary<int, string>
                {
                    { 0, "General \nKnowledge" },
                    { 1, "Philosophy and \nPsychology" },
                    { 2, "Religion"},
                    { 3, "Social Sciences"},
                    { 4, "Language"},
                    { 5, "Science"},
                    { 6, "Technology"},
                    { 7, "Arts and \nRecreation"},
                    { 8, "Literature"},
                    { 9, "History and \nGeography"}
                };
            return dewey;
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to read dewey entries from file 
        /// </summary>
        /// <returns></returns>
        private static List<DeweyDecimalClass> ReadDeweyFromFile()
        {
            var records = new List<DeweyDecimalClass>();

            try
            {
                using (var reader = new StringReader(Properties.Resource.dewey_decimal_data))
                {
                    //reader configuration
                    var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = true,
                        Delimiter = ",",
                        Quote = '"',
                        IgnoreBlankLines = true
                    };
                    //read file
                    using (var csv = new CsvHelper.CsvReader(reader, config))
                    {
                        while (csv.Read())
                        {
                            //read as record
                            var record = new DeweyDecimalClass
                            {
                                Number = csv.GetField<int>(0),
                                Description = csv.GetField<string>(1),
                                Level = csv.GetField<int>(2)
                            };
                            records.Add(record);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return records;
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to create red black dewey tree
        /// </summary>
        /// <returns></returns>
        public List<RBDeweyTree> GetDeweyTree()
        {
            // Initialize with capacity of 3
            List<RBDeweyTree> deweyTree = new List<RBDeweyTree>(3)
            {
                new RBDeweyTree(),
                new RBDeweyTree(),
                new RBDeweyTree()
            };

            try
            {
                var records = ReadDeweyFromFile();

                // Use LINQ to iterate over records
                foreach (var record in records)
                {
                    // Check if Level is within expected range
                    if (record.Level >= 1 && record.Level <= 3)
                    {
                        deweyTree[record.Level - 1].Insert(record);
                    }
                }
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex.ToString());
            }
            return deweyTree;
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Get Dewey Entry
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to get random 3rd level dewey entry
        /// </summary>
        /// <param name="deweyTree"></param>
        /// <returns></returns>
        public DeweyDecimalClass GetRandom3rdDewey(RBDeweyTree deweyTree)
        {
            DeweyDecimalClass temp;
            while (true)
            {
                int num = random.Next(0, 1000);
                temp = deweyTree.FindByCallNumber(num);
                if (temp != null && temp.Level == 3)
                {
                    break;
                }
            }
            return temp;
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// method to get dewey entry form specified level and category
        /// </summary>
        /// <param name="deweyTree"></param>
        /// <param name="level"></param>
        /// <param name="cat"></param>
        /// <returns></returns>
        public DeweyDecimalClass GetDeweyByLevel(RBDeweyTree deweyTree, int level, int cat = 0)
        {
            DeweyDecimalClass temp;
            while (true)
            {
                int num = random.Next(0, 10); //number between 0-9
                int callnumber = level == 1 ? num * 100 : int.Parse($"{cat}{num}") * (level == 2 ? 10 : 1);

                temp = deweyTree.FindByCallNumber(callnumber);
                if (temp != null && temp.Level == level)
                {
                    break;
                }
            }
            return temp;
        }
        //---------------------------------------------------------------------------------------//
        #endregion
    }
}
//-----------------------------------------------oO END OF FILE Oo----------------------------------------------------------------------//