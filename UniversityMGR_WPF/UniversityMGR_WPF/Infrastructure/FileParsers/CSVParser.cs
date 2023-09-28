using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using UniversityMGR_WPF.Models;

namespace UniversityMGR_WPF.Infrastructure.FileParsers
{
    internal class CSVParser
    {
        public static List<Student> ParseStudentsFromCSV(string path, bool hasHeader = true)
        {
            List<Student> students = new();

            try
            {
                using (StreamReader reader = new(path))
                {
                    if (hasHeader)
                        reader.ReadLine();

                    string? line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] values = line.Split(',');

                        if (values.Length < 2)
                            continue;

                        Student student = new()
                        {
                            Name = values[0],
                            Surname = values[1]
                        };

                        students.Add(student);
                    }
                }
                return students;
            }
            catch (Exception ex)
            {
                string caption = "CSV parser";
                string errorMassage = $"Error parsing from CSV: {ex.Message}\nFile: {path}";
                MessageBox.Show(errorMassage, caption, MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<Student>();
            }
        }
    }
}
