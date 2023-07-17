using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using Task10.Models;

namespace Task10.Infrastructure.FileGenerators
{
    internal class CSVGenerator
    {
        public static bool ExportListOfStudentsToCSV(List<Student> students, string path)
        {
            try
            {
                if (students == null || students.Count == 0)
                    return false;

                using (StreamWriter writer = new(path, false, Encoding.UTF8))
                {
                    writer.WriteLine("Name,Surname");

                    foreach (var student in students)
                    {
                        string name = student.Name ?? string.Empty;
                        string surname = student.Surname ?? string.Empty;
                        writer.WriteLine($"{EscapeCsvField(name)},{EscapeCsvField(surname)}");
                    }
                }

                return File.Exists(path) && new FileInfo(path).Length > 0;
            }
            catch (Exception ex)
            {
                string caption = "CSV generator";
                string errorMassage = $"Error exporting to CSV: {ex.Message}";
                MessageBox.Show(errorMassage, caption, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private static string EscapeCsvField(string field)
        {
            if (field.Contains(',') || field.Contains("\"") || field.Contains("\n"))
            {
                field = field.Replace("\"", "\"\"");
                field = $"\"{field}\"";
            }

            return field;
        }
    }
}
