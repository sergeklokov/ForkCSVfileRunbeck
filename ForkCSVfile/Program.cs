using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ForkCSVfile
{
    public class Program
    {
        static void Main(string[] args)
        {
            string fileLocation;
            bool isCsv;
            int fieldsPerRecord;

            // use command line args like: fileData.tsv.txt tsv 3
            
            InitializeInput(args, out fileLocation, out isCsv, out fieldsPerRecord);
            ProcessFileVersion2(fileLocation, isCsv, fieldsPerRecord);

            Console.WriteLine("File forking done. Press any key..");
            Console.ReadKey();
        }

        private static void InitializeInput(string[] args, out string fileLocation, out bool isCsv, out int fieldsPerRecord)
        {
            if (args.Length == 3)
            {
                // command line arguments set
                fileLocation = args[0];
                GetFormat(args[1], out isCsv);
                GetFieldsPerRecord(args[2], out fieldsPerRecord);
            }
            else
            {
                Console.WriteLine("File Location?");
                fileLocation = Console.ReadLine();

                Console.WriteLine("Input CSV or TSV for format:");
                var fileFormat = Console.ReadLine();
                GetFormat(fileFormat, out isCsv);

                Console.WriteLine("Fields per record?");
                GetFieldsPerRecord(Console.ReadLine(), out fieldsPerRecord);
            }
        }

        public static void GetFormat(string fileFormat, out bool isCsv)
        {
            isCsv = true;

            if (fileFormat.ToUpper() == "CSV")
                isCsv = true;
            else if (fileFormat.ToUpper() == "TSV")
                isCsv = false;
            else
            {
                Console.WriteLine("invalid file format");
                Environment.Exit(0);
            }
        }

        private static void GetFieldsPerRecord(string inputString, out int fieldsPerRecord)
        {
            if (!int.TryParse(inputString, out fieldsPerRecord))
            {
                Console.WriteLine("invalid FieldsPerRecord format");
                Environment.Exit(0);
            }
        }

        private static void ProcessFileVersion2(string fileLocation, bool isCsv, int fieldsPerRecord)
        {
            //TODO: check file existanse.. well let it throw

            var isHeader = true;
            int separatorCount;
            string line;
            char separator = '\t';

            if (isCsv)
                separator = ',';

            var good = new List<string>();  // list with good lines
            var bad = new List<string>();   // list with bad lines

            using (var sr = new StreamReader(fileLocation))
            {
                while(sr.Peek() >= 0)
                {
                    if (isHeader)
                    {
                        sr.ReadLine();
                        isHeader = false;
                    }
                    else
                    {
                        line = sr.ReadLine();
                        separatorCount = line.Count(c => c == separator);

                        // example of self descriptive code:
                        bool isCorrectAmountOfFields = separatorCount == fieldsPerRecord - 1;

                        if (isCorrectAmountOfFields)
                        {
                            good.Add(line);
                            Console.WriteLine(line); // TODO: we may remove later output to the screen, leave it for demo purpose
                        }
                        else
                        {
                            bad.Add(line);
                        }
                    }
                }
            }


            // it would be faster do not save lines in list, but delete empty files

            if (good.Count > 0)
                WriteListToFile(good, fileLocation + ".good.txt");

            if (bad.Count > 0)
                WriteListToFile(bad, fileLocation + ".bad.txt");
        }

        private static void WriteListToFile(List<string> lst, string fileLocation)
        {
            using (var sw = new StreamWriter(fileLocation))
                lst.ForEach(line => sw.WriteLine(line));   // let's use LINQ to iterate list
            
        }
    }
}
