using System;
using System.Collections.Generic;
using Task10.Infrastructure.FileGenerators;
using Task10.Models;
using Task10.Services.Interfaces;

namespace Task10.Services
{
    class FileService : IFileService
    {
        public bool ExportToFile(object item, string fileType, string path)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            
            if (fileType is null)
                throw new ArgumentNullException(nameof(fileType));

            var upperFileType = fileType.ToUpper();

            return (item, upperFileType) switch
            {
                (Group group, ".PDF") => PDFGenerator.ExportGroupStudentsToPDF(group, path),
                (Group group, ".DOCX") => DOCXGenerator.ExportGroupDetailToDOCX(group, path),
                (List<Student> students, ".CSV") => CSVGenerator.ExportListOfStudentsToCSV(students, path),
                _ => throw new NotSupportedException($"Export object of type {item.GetType().Name} to {fileType} not supported"),
            };
        }
    }
}
