using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NWT_JSONify
{
    public static class Program
    {
        public const string chTxt = "Chapter ";
        public const string jsonFileName = "nwt_02_Ex_E";

        static void Main(string[] args)
        {
            Console.WriteLine("JSON-ify New World Translation (NWT)\n");

            // Locate TXT File
            string path = @"nwt_02_Ex_E.txt";
            Console.WriteLine("path:" + path + "\n");
            // Create new .json file

            // Assign .json name

            // If .json exists already, quit program

            // Parse through txt and save to a class
            parseBook(path);
            return;
        }

        static void saveChaptersToJSON(string fileName, Chapters[] chapters)
        {
            Book book = new Book()
            {
                Name = jsonFileName,
                Abbrev = new List<string>() { "gen", "ge" },
                Chapters = chapters.ToList()
            };
            using (StreamWriter file = File.CreateText(fileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, book);
            }
        }

        public static IList<int> AllIndexOf(this string text, string str, StringComparison comparisonType)
        {
            IList<int> allIndexOf = new List<int>();
            int index = text.IndexOf(str, comparisonType);
            while(index != -1)
            {
                allIndexOf.Add(index);
                index = text.IndexOf(str, index + 1, comparisonType);
            }
            return allIndexOf;
        }

        static unsafe void parseBook(string txtPathName)
        {
            IList<int> cIx1 = new List<int>(); // list of ix for all chapters (beginning index)
            IList<int> cIx2 = new List<int>(); // list of ix for all chapters (ending index)

            string text = System.IO.File.ReadAllText(txtPathName);  

            // Find all chapters and save the index of each chapter
            cIx1 = AllIndexOf(text, chTxt, StringComparison.CurrentCulture);

            Console.WriteLine(string.Join(", ", cIx1));
            Console.WriteLine("Number of Chapters in " + txtPathName + ": " + cIx1.Count());

            parseChapters(cIx1, text);
        }

        public static void parseChapters(IList <int> txtIx, string text)
        {

             var chapters = new List<Chapters>();

            for (int i = 0; i < txtIx.Count; i++) // loop through each chapter
            {
                int currCh = i + 1;
                int nVerse = 1;
                int chStartIx, chEndIx; // start and end to each chapter

                chStartIx = txtIx[i] + chTxt.Length + (currCh.ToString()).Length;   // ix of first space after "Chapter 12 "

                if (currCh == txtIx.Count)
                    chEndIx = text.Length - 1;
                else
                    chEndIx = txtIx[i + 1] - 1;                                       // ix of end of chapter

                string oneChapter = text.Substring(chStartIx, chEndIx - chStartIx);
                string oneVerse;

                var verses = new List<Verses>();

                int vrsStartIx, vrsEndIx;
                vrsStartIx = chStartIx;

                while (oneChapter.Contains(nVerse.ToString() + " ")) // loop through each verse
                {
                    int nVerseNext = nVerse + 1;
                    vrsStartIx = oneChapter.IndexOf(nVerse.ToString() + " ");

                    if (oneChapter.Contains(nVerseNext.ToString() + " "))
                        vrsEndIx = oneChapter.IndexOf(nVerseNext.ToString() + " ") - nVerseNext.ToString().Length;
                    else
                    {
                        vrsEndIx = chEndIx - 1;
                        oneVerse = oneChapter.Substring(vrsStartIx + nVerse.ToString().Length);
                        Console.WriteLine("Verse " + nVerse.ToString() + ":" + oneVerse.TrimEnd());
                        verses.Add(new Verses { Verse = nVerse, Text = oneVerse.TrimEnd() });
                        break;
                    }

                    oneVerse = oneChapter.Substring(vrsStartIx + nVerse.ToString().Length, vrsEndIx - vrsStartIx);
                    vrsStartIx = vrsEndIx;
                    oneChapter = oneChapter.Substring(vrsStartIx);
                    Console.WriteLine(currCh.ToString() + ":" + nVerse.ToString() + ":" + oneVerse.TrimEnd());
                    verses.Add(new Verses { Verse = nVerse, Text = oneVerse.TrimEnd() });
                    nVerse++;
                }

                chapters.Add(new Chapters { Chapter = currCh, Verses = verses });
            }

            saveChaptersToJSON(jsonFileName + ".json", chapters.ToArray());
/*
            for(int i = 0; i<txtIx.Count; i++) // loop thru each chapter
            {
                int currCh = i+1;
                int nVerse = 1;
                int chStartIx, chEndIx; // start and end to each chapter

                chStartIx = txtIx[i] + chTxt.Length + (currCh.ToString()).Length;   // ix of first space after "Chapter 12 "
                
                if(currCh == txtIx.Count)
                    chEndIx = text.Length - 1;
                else
                    chEndIx = txtIx[i+1] - 1;                                       // ix of end of chapter
                
                string oneChapter = text.Substring(chStartIx,chEndIx-chStartIx);
                string oneVerse;

                int vrsStartIx, vrsEndIx;
                vrsStartIx = chStartIx;
                while ( oneChapter.Contains(nVerse.ToString() + " ") ) // loop thru each verse
                {
                    int nVerseNext = nVerse + 1;
                    vrsStartIx = oneChapter.IndexOf(nVerse.ToString() + " ") ;

                    if(oneChapter.Contains(nVerseNext.ToString() + " "))
                        vrsEndIx = oneChapter.IndexOf(nVerseNext.ToString() + " ") - nVerseNext.ToString().Length;
                    else
                    {
                        vrsEndIx = chEndIx-1;
                        oneVerse = oneChapter.Substring(vrsStartIx + nVerse.ToString().Length);
                        Console.WriteLine("Verse " + nVerse.ToString() + ":" + oneVerse.TrimEnd());
                        break;
                    }

                    oneVerse = oneChapter.Substring(vrsStartIx + nVerse.ToString().Length, vrsEndIx - vrsStartIx);
                    vrsStartIx = vrsEndIx;
                    oneChapter = oneChapter.Substring(vrsStartIx);
                    Console.WriteLine(currCh.ToString()+ ":" + nVerse.ToString() + ":" + oneVerse.TrimEnd());
                    nVerse++;
                }
            }

            List<Chapters> chapters = new List<Chapters>();
            for (int i = 0; i < txtIx.Count; i++)
            {
                int currCh = i+1;
                int nVerse = 1;
                int chStartIx, chEndIx; // start and end to each chapter

                chStartIx = txtIx[i] + chTxt.Length + (currCh.ToString()).Length;   // ix of first space after "Chapter 12 "
                
                if(currCh == txtIx.Count)
                    chEndIx = text.Length - 1;
                else
                    chEndIx = txtIx[i+1] - 1;                                       // ix of end of chapter
                
                string oneChapter = text.Substring(chStartIx,chEndIx-chStartIx);
                string oneVerse = "";

                // ...
                IList<Verses> verses = new List<Verses>();
                while (oneChapter.Contains(nVerse.ToString() + " "))
                {
                    // ...
                    Verses verse = new Verses()
                    {
                        Verse = nVerse,
                        Text = oneVerse.TrimEnd()
                    };
                    verses.Add(verse);
                    nVerse++;
                }
                Chapters chapter = new Chapters()
                {
                    Chapter = currCh,
                    Verses = verses
                };
                chapters.Add(chapter);
            }
            saveChaptersToJSON(jsonFileName + ".json", chapters.ToArray());
*/
        }
        

        public class Book
        {
            public string? Name { get ; set; }
            public IList<string>? Abbrev { get ; set; }

            public IList<Chapters>? Chapters { get ; set; }
        }

        public class Chapters
        {
            public int Chapter;
            public IList<Verses>? Verses { get ; set; }
        }

        public class Verses
        {
            public int Verse;
            public string? Text;
        }
    }
}
