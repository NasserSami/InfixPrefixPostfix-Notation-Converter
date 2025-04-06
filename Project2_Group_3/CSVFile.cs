using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/// <summary>
/// Class to handle CSV file operations
/// </summary>
public class CSVFile
{
    /// <summary>
    /// Deserializes the CSV file content to a list of expression records
    /// </summary>
    /// <param name="filePath">Path to the CSV file</param>
    /// <returns>List of expression data (sno, infix expression)</returns>
    public List<(int sno, string infixExpression)> CSVDeserialize( string filePath )
    {
        List<(int, string)> expressions = new List<(int, string)>();

        try
        {
            // Read all lines from the CSV file
            string[] lines = File.ReadAllLines(filePath);

            // Skip header row if it exists
            int startIndex = 0;
            if (lines.Length > 0 && !int.TryParse(lines[0].Split(',')[0], out _))
            {
                startIndex = 1;
            }

            // Process each line of data
            for (int i = startIndex; i < lines.Length; i++)
            {
                string line = lines[i].Trim();

                // Skip empty lines
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                // Split the CSV line
                string[] parts = line.Split(',');

                // Parse the sequence number and infix expression
                if (parts.Length >= 2 && int.TryParse(parts[0], out int sno))
                {
                    // Get the infix expression (might contain commas, so join the remaining parts)
                    StringBuilder infixBuilder = new StringBuilder(parts[1]);
                    for (int j = 2; j < parts.Length; j++)
                    {
                        infixBuilder.Append(",").Append(parts[j]);
                    }

                    string infixExpression = infixBuilder.ToString().Trim();

                    // Remove any quotes if present
                    if (infixExpression.StartsWith("\"") && infixExpression.EndsWith("\""))
                    {
                        infixExpression = infixExpression.Substring(1, infixExpression.Length - 2);
                    }

                    expressions.Add((sno, infixExpression));
                }
            }

            Console.WriteLine($"Successfully read {expressions.Count} expressions from CSV file.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading CSV file: {ex.Message}");
        }

        return expressions;
    }
}