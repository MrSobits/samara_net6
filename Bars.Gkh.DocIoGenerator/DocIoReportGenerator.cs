namespace Bars.Gkh.DocIoGenerator
{
    using System;
    using System.IO;
    using B4.Modules.Reports;
    using B4.Modules.Reports;

    public class DocIoReportGenerator : IReportGenerator
    {
        private Stream template;
        
        public void Open(Stream reportTemplate)
        {
            template = reportTemplate;
        }

        public void Generate(Stream result, ReportParams reportParams)
        {
            if (template == null)
            {
                throw new Exception("Не задан шаблон отчета");
            }

            if (reportParams == null)
            {
                throw new Exception("Не заданы параметры отчета");
            }

            if (result == null)
            {
                throw new Exception("Не задан выходной файл отчета");
            }

            var docIo = new DocIo();

            try
            {
                docIo.OpenTemplate(template);
                
                foreach (var param in reportParams.SimpleReportParams.СписокПараметров)
                {
                    if (param.Value is PictureParam)
                    {
                        var pictureParam = param.Value as PictureParam;
                        if (pictureParam.ChangeSize)
                        {
                            docIo.SetPicture(param.Key, pictureParam.Value, pictureParam.Width, pictureParam.Height);
                        }
                        else
                        {
                            docIo.SetPicture(param.Key, pictureParam.Value);
                        }
                    }
                    else if (param.Value is TableParam)
                    {
                        var tableParam = param.Value as TableParam;
                        if (tableParam.Table != null)
                        {
                            docIo.AddTable(tableParam.Table, tableParam.Name, tableParam.ColumnWidth);
                        }
                    }
                    else
                    {
                        docIo.SetValue(param.Key, param.Value);
                    }
                }

                docIo.SaveDocument(result);
            }
            catch (Exception exc)
            {
                throw new Exception("Неудалось сгенерировать отчет", exc);
            }
            finally
            {
                docIo.CloseTemplate();
            }
        }
    }
}