using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Task10.Models;

namespace Task10.Infrastructure.FileGenerators
{
    internal class PDFGenerator
    {
        public static bool ExportGroupStudentsToPDF(List<Student> students, string path, string headerText = "")
        {
            try
            {
                using (PdfDocument document = new())
                {
                    PdfPage page = CreateNewPage(document);
                    XGraphics graphics = XGraphics.FromPdfPage(page);
                    XFont font = new("Arial", 12, XFontStyleEx.Regular);
                    XRect contentRect = GetContentRect(page);
                    XPoint position = new(contentRect.X, contentRect.Y);
                    int studentNumber = 1;

                    if (!string.IsNullOrEmpty(headerText))
                        AddHeader(page, headerText, graphics);

                    foreach (Student student in students)
                    {
                        string studentText = $"{studentNumber}. {student.FullName}";
                        position = DrawText(studentText, headerText, font, position, contentRect, ref graphics, ref page, document);
                        studentNumber++;
                    }

                    document.Save(path);
                }

                return File.Exists(path) && new FileInfo(path).Length > 0;
            }
            catch (Exception ex)
            {
                string caption = "PDF generator";
                string errorMassage = $"Error exporting to PDF: {ex.Message}";
                MessageBox.Show(errorMassage, caption, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private static PdfPage CreateNewPage(PdfDocument document, int widthInMm = 210, int heigthInMm = 297)
        {
            PdfPage page = document.AddPage();
            page.Width = XUnit.FromMillimeter(widthInMm);
            page.Height = XUnit.FromMillimeter(heigthInMm);
            return page;
        }

        private static XRect GetContentRect(PdfPage page, int pageMarginInMm = 25)
        {
            XUnit margin = XUnit.FromMillimeter(pageMarginInMm);
            XRect contentRect = new(margin, margin, page.Width - margin, page.Height - margin);
            return contentRect;
        }

        private static bool TextFitsOnPage(XPoint position, XFont font, XRect contentRect)
        {
            return position.Y + font.GetHeight() <= contentRect.Y + contentRect.Height;
        }

        private static XPoint DrawText(string text, string headerText, XFont font, XPoint position, XRect contentRect, ref XGraphics graphics, ref PdfPage page, PdfDocument document)
        {
            if (!TextFitsOnPage(position, font, contentRect))
            {
                page = CreateNewPage(document);
                graphics = XGraphics.FromPdfPage(page);
                position = new XPoint(contentRect.X, contentRect.Y);

                AddHeader(page, headerText, graphics);
            }

            XTextFormatter formatter = new(graphics);
            XRect layoutRect = new(position, contentRect.Size);
            formatter.DrawString(text, font, XBrushes.Black, layoutRect, XStringFormats.TopLeft);

            position.Y += font.GetHeight();
            return position;
        }

        private static void AddHeader(PdfPage page, string headerText, XGraphics graphics, int headerHeigth = 40)
        {
            XFont font = new("Arial", 10, XFontStyleEx.Regular);
            XRect headerRect = new(0, 0, page.Width, headerHeigth);

            graphics.DrawString(headerText, font, XBrushes.Black, headerRect, XStringFormats.Center);
        }
    }
}
