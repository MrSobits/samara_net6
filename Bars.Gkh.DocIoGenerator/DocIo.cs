namespace Bars.Gkh.DocIoGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    using B4.Modules.Reports;
    using Syncfusion.DocIO;
    using Syncfusion.DocIO.DLS;

    public class DocIo : IDocIo
    {
        private WordDocument wordDocument;

        /// <summary>Открыть шаблон</summary>
        /// <param name="stream">Шаблон</param>
        public void OpenTemplate(Stream stream)
        {
            try
            {
                stream.Seek(0, SeekOrigin.Begin);
                wordDocument = new WordDocument(stream, FormatType.Doc);
            }
            catch (Exception exc)
            {
                throw new ReportProviderException("Не удалось открыть шаблон", exc);
            }
        }
        
        /// <inheritdoc />
        public Stream ConvertToDoc(Stream stream, string newFileName = null)
        {
            stream.Seek(0, SeekOrigin.Begin);

            string pathToDoc;

            try
            {
                pathToDoc = this.GenerateTempDocFile(stream, newFileName);
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при преобразовании файла в формат DOC", e);
            }

            return new FileStream(pathToDoc, FileMode.Open);
        }
        
        private string GenerateTempDocFile(Stream stream, string newFileName = null)
        {
           /*var tempFileName = Path.Combine(Path.GetTempPath(), newFileName ?? Path.GetRandomFileName());
            var docFileName = tempFileName + ".doc";
            if (File.Exists(docFileName))
            {
                return docFileName;
            }

            using (var fileWriter = new FileStream(tempFileName, FileMode.Create))
            {
                stream.CopyTo(fileWriter);
            }

            object fileName = tempFileName;
            object saveChanges = Word.WdSaveOptions.wdDoNotSaveChanges;
            lock (DocIo.locker)
            {
                var wordApplication = new Word.Application
                {
                    Visible = false,
                    ScreenUpdating = false
                };
                try
                {
                    var doc = wordApplication.Documents.Open(ref fileName);
                    try
                    {
                        object outputFileName = docFileName;
                        object fileFormat = Word.WdSaveFormat.wdFormatDocument97;
                        doc.SaveAs(ref outputFileName, ref fileFormat);
                        return (string)outputFileName;
                    }
                    finally
                    {
                        doc.Close(ref saveChanges);
                    }
                }
                finally
                {
                    wordApplication.Quit();
                    File.Delete(tempFileName);
                }
            }*/
           return "";
        }
        
        /// <inheritdoc />
        public Stream ConvertToPdf(Stream stream, string newFileName = null)
        {
            stream.Seek(0, SeekOrigin.Begin);

            string pathToPdf;

            try
            {
                pathToPdf = this.GenerateTempPdfFile(stream, newFileName);
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при преобразовании файла в формат PDF", e);
            }

            return new FileStream(pathToPdf, FileMode.Open);
        }
        
        private string GenerateTempPdfFile(Stream stream, string newFileName = null)
        {
            // TODO: Найти замену Word?
           /* var tempFileName = Path.Combine(Path.GetTempPath(), newFileName ?? Path.GetRandomFileName());
            var pdfFileName = tempFileName + ".pdf";
            if (File.Exists(pdfFileName))
            {
                return pdfFileName;
            }

            using (var fileWriter = new FileStream(tempFileName, FileMode.Create))
            {
                stream.CopyTo(fileWriter);
            }
            object fileName = tempFileName;
            object saveChanges = Word.WdSaveOptions.wdDoNotSaveChanges;
            lock (DocIo.locker)
            {
                var wordApplication = new Word.Application
                {
                    Visible = false,
                    ScreenUpdating = false
                };
                try
                {
                    var doc = wordApplication.Documents.Open(ref fileName);
                    try
                    {
                        object outputFileName = pdfFileName;
                        object fileFormat = Word.WdSaveFormat.wdFormatPDF;
                        doc.SaveAs(ref outputFileName, ref fileFormat);
                        return (string)outputFileName;
                    }
                    finally
                    {
                        doc.Close(ref saveChanges);
                    }
                }
                finally
                {
                    wordApplication.Quit();
                    File.Delete(tempFileName);
                }
            }*/
           return "";
        }

        /// <summary>Открыть шаблон</summary>
        /// <param name="binaryData">Бинарный шаблон</param>
        public void OpenTemplate(byte[] binaryData)
        {
            try
            {
                var stream = new MemoryStream();
                stream.Write(binaryData, 0, binaryData.Length);

                stream.Seek(0, SeekOrigin.Begin);
                wordDocument = new WordDocument(stream, FormatType.Doc);
            }
            catch (Exception exc)
            {
                throw new ReportProviderException("Не удалось открыть шаблон", exc);
            }
        }

        public void CloseTemplate()
        {
            if (wordDocument != null)
            {
                //wordDocument.Close();
            }
        }

        /// <summary>Установить значение по ключу</summary>
        /// <param name="key">Ключ</param>
        /// <param name="value">Значение</param>
        public void SetValue(string key, object value)
        {
            try
            {
                var val = value == null ? string.Empty : value.ToString();
                wordDocument.Replace(GetKey(key), val, false, false);
            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Не удалось установить значение \"{0}\" по ключу \"{1}\"", value, key), exc);
            }
        }

        public void SetPicture(string key, Stream stream)
        {
            SetPicture(key, stream, 0, 0);
        }

        public void SetPicture(string key, Stream stream, int width, int height)
        {
            try
            {
                var textSelection = wordDocument.Find(key, false, false);
                if (textSelection != null)
                {
                    var paragraph = textSelection.GetAsOneRange().OwnerParagraph;
                    paragraph.Items.Clear();
                    var picture = (WPicture)paragraph.AppendPicture(stream);
                    if (width > 0 && height > 0)
                    {
                        picture.Width = width;
                        picture.Height = height;
                    }
                }
            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Не удалось установить картинку по ключу \"{0}\"", key), exc);
            }
        }
        
        public bool TrySetPicture(string key, Stream stream, int width, int height)
        {
            try
            {
                var textSelection = wordDocument.Find(key, false, false);
                if (textSelection != null)
                {
                    var paragraph = textSelection.GetAsOneRange().OwnerParagraph;
                    paragraph.Items.Clear();
                    var picture = (WPicture)paragraph.AppendPicture(stream);
                    if (width > 0 && height > 0)
                    {
                        picture.Width = width;
                        picture.Height = height;
                    }

                    return true;
                }

                return false;
            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Не удалось установить картинку по ключу \"{0}\"", key), exc);
            }
        }

        public void AddTable(DataTable dataTable)
        {
            AddTable(dataTable, null);
        }

        public void AddTable(DataTable dataTable, string name)
        {
            AddTable(dataTable, null, null);
        }

        public void AddTable(DataTable dataTable, string name, List<int?> columnWidth)
        {
            if (dataTable == null)
            {
                return;
            }

            var section = wordDocument.AddSection();
            var paragraph = section.AddParagraph();
            if (!string.IsNullOrEmpty(name))
            {
                paragraph.AppendText(name).CharacterFormat.FontSize = 12;
            }

            var table = section.Body.AddTable();
            table.ResetCells(dataTable.Rows.Count + 1, dataTable.Columns.Count);

            // Вычисляем ширину каждого столбца
            var widthColumns = new float[dataTable.Columns.Count];
            var tableWidth = table.Width;
            var clmnCount = columnWidth != null ? columnWidth.Count(x => x.HasValue) : 0;
            var clmnWith = columnWidth != null ? (int)columnWidth.Where(x => x.HasValue).Sum(x => x) : 0;
            var averageWidth = (clmnCount > 0 && clmnCount != dataTable.Columns.Count) 
                ? ((tableWidth - clmnWith) / (dataTable.Columns.Count - clmnCount)) 
                : (tableWidth / dataTable.Columns.Count);
            for (var i = 0; i < dataTable.Columns.Count; i++)
            {
                if (columnWidth != null && columnWidth.Count >= i && columnWidth[i].HasValue)
                {
                    widthColumns[i] = columnWidth[i].Value;
                }
                else
                {
                    widthColumns[i] = averageWidth;
                }
            }

            for (var i = 0; i < dataTable.Columns.Count; i++)
            {
                var column = dataTable.Columns[i];
                var row = table.Rows[0];
                var cell = row.Cells[i];
                var cellParagraph = cell.AddParagraph();
                cellParagraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                cellParagraph.AppendText(column.ColumnName);
                cell.Width = widthColumns[i];
            }

            for (var i = 0; i < dataTable.Rows.Count; i++)
            {
                var dataRow = dataTable.Rows[i];
                var row = table.Rows[i + 1];
                for (var j = 0; j < dataRow.ItemArray.Length; j++)
                {
                    var value = dataRow.ItemArray[j];
                    var cell = row.Cells[j];
                    cell.Width = widthColumns[j];
                    cell.AddParagraph().AppendText(value != null ? value.ToString() : string.Empty);
                }
            }
        }

        /// <summary>Получить документ</summary>
        public void SaveDocument(Stream document)
        {
            if (wordDocument != null)
            {
                ClearValuesDocument();
                wordDocument.Save(document, FormatType.Word2010);
            }
            else
            {
                throw new ReportProviderException("Не создан документ");
            }
        }
        
        /// <summary>Очистить не заполненные значения документа</summary>
        private void ClearValuesDocument()
        {
            if (wordDocument != null)
            {
                /*
                #warning падает в случае если переменная объявлена более одного раза в документе
                var data = wordDocument.FindAll(new Regex(@"[$][^$]*?[$]", RegexOptions.Compiled | RegexOptions.IgnoreCase));
                if (data != null)
                {
                    var listEmptyParams = new List<string>();
                    foreach (var value in data)
                    {
                        if (value != null && !string.IsNullOrEmpty(value.SelectedText))
                        {
                            var str = value.SelectedText.ToLower();

                            if (!listEmptyParams.Contains(str))
                            {
                                wordDocument.Replace(value.SelectedText, string.Empty, false, false);
                                listEmptyParams.Add(str);
                            }
                        }
                    }
                }
                */
                wordDocument.Replace(new Regex(@"[$][^$]*?[$]", RegexOptions.Compiled | RegexOptions.IgnoreCase), string.Empty);
            }
        }

        /// <summary>Получить ключ</summary>
        private string GetKey(string key)
        {
            return string.Format("${0}$", key);
        }
    }
}