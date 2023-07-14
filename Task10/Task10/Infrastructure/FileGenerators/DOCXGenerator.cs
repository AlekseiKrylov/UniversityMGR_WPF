using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Task10.Models;

namespace Task10.Infrastructure.FileGenerators
{
    internal class DOCXGenerator
    {
        public static bool ExportGroupStudentsToDOCX(List<Student> students, string path, string headerText = "")
        {
            try
            {
                using (WordprocessingDocument document = WordprocessingDocument.Create(path, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = document.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    Body body = mainPart.Document.AppendChild(new Body());

                    RunProperties runProperties = new();
                    RunFonts runFonts = new() { Ascii = "Arial", HighAnsi = "Arial" };
                    FontSize fontSize = new() { Val = "24" };
                    runProperties.Append(runFonts);
                    runProperties.Append(fontSize);

                    if (!string.IsNullOrEmpty(headerText))
                    {
                        HeaderPart headerPart = mainPart.AddNewPart<HeaderPart>();
                        Header header = new();

                        Paragraph paragraph = new();
                        Run headerRun = new(new Text(headerText));
                        headerRun.RunProperties = runProperties;

                        ParagraphProperties paragraphProperties = new();
                        Justification justification = new() { Val = JustificationValues.Center };

                        paragraphProperties.Append(justification);
                        paragraph.Append(paragraphProperties);
                        paragraph.Append(headerRun);

                        header.Append(paragraph);

                        headerPart.Header = header;
                        string headerPartId = mainPart.GetIdOfPart(headerPart);

                        SectionProperties sectionProperties = new(new HeaderReference() { Type = HeaderFooterValues.Default, Id = headerPartId });
                        body.Append(sectionProperties);
                    }

                    Paragraph studentsParagraph = new();
                    int studentNumber = 1;
                    foreach (Student student in students)
                    {
                        Run studentNumberRun = new(new Text($"{studentNumber}.\u00A0"));
                        studentNumberRun.RunProperties = (RunProperties)runProperties.CloneNode(true);
                        studentsParagraph.Append(studentNumberRun);
                        Run studentRun = new(new Text(student.FullName));
                        studentRun.RunProperties = (RunProperties)runProperties.CloneNode(true);
                        studentsParagraph.Append(studentRun);
                        studentsParagraph.Append(new Break());
                        studentNumber++;
                    }
                    body.Append(studentsParagraph);

                    document.Save();
                }

                return File.Exists(path) && new FileInfo(path).Length > 0;
            }
            catch (Exception ex)
            {
                string caption = "DOCX generator";
                string errorMassage = $"Error exporting to DOCX: {ex.Message}";
                MessageBox.Show(errorMassage, caption, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
