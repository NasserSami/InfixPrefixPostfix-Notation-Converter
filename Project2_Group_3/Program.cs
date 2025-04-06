using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace ExpressionConverter
{
    /// <summary>
    /// Main program class
    /// </summary>
    class Program
    {
        static void Main( string[] args )
        {
            Console.Title = "Expression Converter - INFO-5101 Project";
            Console.WindowWidth = 100;
            Console.WindowHeight = 40;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================================================================");
            Console.WriteLine("                            Expression Converter                               ");
            Console.WriteLine("                Converting Infix to Prefix and Postfix notations               ");
            Console.WriteLine("===============================================================================");
            Console.ResetColor();
            Console.WriteLine();

            // Create Data directory if it doesn't exist
            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
                Console.WriteLine("Created 'Data' directory for file storage");
            }

            // Define file paths
            string csvFilePath = Path.Combine("Data", "Project 2_INFO_5101.csv");
            string xmlFilePath = Path.Combine("Data", "expressions.xml");

            // Check if CSV file exists
            if (!File.Exists(csvFilePath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: CSV file not found at path: {csvFilePath}");
                Console.ResetColor();
                Console.WriteLine("Please place the CSV file in the Data directory and try again.");
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
                return;
            }

            // Initialize required objects
            CSVFile csvFile = new CSVFile();
            InfixToPrefix infixToPrefix = new InfixToPrefix();
            InfixToPostfix infixToPostfix = new InfixToPostfix();
            ExpressionEvaluation evaluator = new ExpressionEvaluation();
            CompareExpressions comparer = new CompareExpressions();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Reading CSV file...");
            Console.ResetColor();

            // Deserialize CSV to get infix expressions
            var infixExpressions = csvFile.CSVDeserialize(csvFilePath);

            if (infixExpressions.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: No expressions found in the CSV file.");
                Console.ResetColor();
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
                return;
            }

            // Lists to store conversion and evaluation results
            List<int> snoList = new List<int>();
            List<string> infixList = new List<string>();
            List<string> prefixList = new List<string>();
            List<string> postfixList = new List<string>();
            List<double> prefixResultList = new List<double>();
            List<double> postfixResultList = new List<double>();
            List<bool> matchList = new List<bool>();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nProcessing {infixExpressions.Count} expressions...");
            Console.ResetColor();

            // Show processing animation
            Console.Write("Processing: ");
            for (int i = 0; i < 20; i++)
            {
                Console.Write("▓");
                System.Threading.Thread.Sleep(100);
            }
            Console.WriteLine(" Done!");
            Console.WriteLine();

            // Process each infix expression
            foreach (var (sno, infix) in infixExpressions)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nExpression {sno}: {infix}");
                Console.ResetColor();

                try
                {
                    // Convert infix to prefix
                    string prefix = infixToPrefix.Convert(infix);
                    Console.WriteLine($"Prefix notation: {prefix}");

                    // Convert infix to postfix
                    string postfix = infixToPostfix.Convert(infix);
                    Console.WriteLine($"Postfix notation: {postfix}");

                    // Evaluate prefix expression
                    double prefixResult = 0;
                    try
                    {
                        prefixResult = evaluator.EvaluatePrefix(prefix);
                        Console.WriteLine($"Prefix evaluation: {prefixResult}");
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Error evaluating prefix expression: {ex.Message}");
                        Console.ResetColor();
                        prefixResult = double.NaN;
                    }

                    // Evaluate postfix expression
                    double postfixResult = 0;
                    try
                    {
                        postfixResult = evaluator.EvaluatePostfix(postfix);
                        Console.WriteLine($"Postfix evaluation: {postfixResult}");
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Error evaluating postfix expression: {ex.Message}");
                        Console.ResetColor();
                        postfixResult = double.NaN;
                    }

                    // Compare results
                    bool match = comparer.Compare(prefixResult, postfixResult) == 0;
                    Console.Write("Results match: ");

                    if (match)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("True");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("False");
                    }
                    Console.ResetColor();

                    // Store results
                    snoList.Add(sno);
                    infixList.Add(infix);
                    prefixList.Add(prefix);
                    postfixList.Add(postfix);
                    prefixResultList.Add(prefixResult);
                    postfixResultList.Add(postfixResult);
                    matchList.Add(match);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error processing expression: {ex.Message}");
                    Console.ResetColor();
                }
            }

            // Display summary report
            DisplaySummaryReport(snoList, infixList, prefixList, postfixList, prefixResultList, postfixResultList, matchList);

            // Generate XML file
            GenerateXmlFile(xmlFilePath, snoList, infixList, prefixList, postfixList, prefixResultList, matchList);

            // Prompt user to open XML file
            Console.WriteLine("\nXML file has been generated. Would you like to open it? (Y/N)");
            string response = Console.ReadLine();

            if (response?.ToUpper() == "Y")
            {
                try
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = xmlFilePath,
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error opening XML file: {ex.Message}");
                }
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Displays a formatted summary report of all expression conversions and evaluations
        /// </summary>
        static void DisplaySummaryReport( List<int> snoList, List<string> infixList, List<string> prefixList, List<string> postfixList,
                                        List<double> prefixResultList, List<double> postfixResultList, List<bool> matchList )
        {
            Console.WriteLine("\n" + "=".PadRight(80, '='));
            Console.WriteLine("Summary Report".PadLeft(40 + "Summary Report".Length / 2));
            Console.WriteLine("=".PadRight(80, '='));

            // Column headers with proper spacing to match the sample output
            Console.WriteLine($"| {"Sno",-4}| {"Infix",-20}| {"PostFix",-20}| {"Prefix",-20}| {"Prefix Res",-10}|{"PostFix Res",-10}| {"Match",-5}|");
            Console.WriteLine($"|{"-".PadRight(4, '-')}|{"-".PadRight(20, '-')}|{"-".PadRight(20, '-')}|{"-".PadRight(20, '-')}|{"-".PadRight(10, '-')}|{"-".PadRight(10, '-')}|{"-".PadRight(5, '-')}|");

            for (int i = 0; i < snoList.Count; i++)
            {
                Console.WriteLine($"| {snoList[i],-4}| {infixList[i],-20}| {postfixList[i],-20}| {prefixList[i],-20}| " +
                                  $"{prefixResultList[i],-10}| {postfixResultList[i],-10}| {matchList[i],-5}|");
            }

            Console.WriteLine("=".PadRight(80, '='));
        }

        /// <summary>
        /// Generates an XML file with the expression data
        /// </summary>
        static void GenerateXmlFile( string filePath, List<int> snoList, List<string> infixList, List<string> prefixList,
                                   List<string> postfixList, List<double> resultList, List<bool> matchList )
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartRootElement();

                    for (int i = 0; i < snoList.Count; i++)
                    {
                        writer.WriteStartElement();

                        // Write each attribute according to the required format (sno, infix, prefix, postfix, evaluation, comparison)
                        writer.WriteAttribute("sno", snoList[i].ToString());
                        writer.WriteAttribute("infix", infixList[i]);
                        writer.WriteAttribute("prefix", prefixList[i]);
                        writer.WriteAttribute("postfix", postfixList[i]);
                        writer.WriteAttribute("evaluation", resultList[i].ToString());
                        writer.WriteAttribute("comparison", matchList[i].ToString());

                        writer.WriteEndElement();
                    }

                    writer.WriteEndRootElement();
                }

                Console.WriteLine($"XML file created successfully at: {filePath}");
                Console.WriteLine($"File location: {Path.GetFullPath(filePath)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating XML file: {ex.Message}");
            }
        }
    }
}