namespace Bars.GkhGji.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;
    using Bars.B4.Application;
    using Bars.GkhGji.Contracts;
    using System.Security.Cryptography.X509Certificates;
    using System.Drawing;
    using System.IO;
    using System.Drawing.Drawing2D;

    public static class Utils
    {
        /// <summary>
        /// Метод получения названия документа по Типу
        /// </summary>
        public static string GetDocumentName(TypeDocumentGji type)
        {
            var dispText = ApplicationContext.Current.Container.Resolve<IDisposalText>();
            var docName = string.Empty;
            switch (type)
            {
                case TypeDocumentGji.Disposal:
                    docName = dispText.SubjectiveCase;
                    break;
                default:
                    docName = type.GetDisplayName();
                    break;
                case TypeDocumentGji.ProtocolRSO:
                    docName = "Протокол РСО";
                    break;
            }

            return docName;
        }

        /// <summary>
        /// Метод присвоения номера к основанию проверки
        /// </summary>
        public static void CreateInspectionNumber(IWindsorContainer container, InspectionGji inspection, int year)
        {
            if (year == 1)
            {
                year = DateTime.Now.Year;
            }

            inspection.InspectionYear = year;

            var service = container.Resolve<IDomainService<InspectionGji>>();
            var maxNum = service.GetAll()
                .Where(x => x.InspectionYear == year && x.Id != inspection.Id && x.TypeBase == inspection.TypeBase)
                .Select(x => x.InspectionNum).Max();

            inspection.InspectionNum = maxNum.ToInt() + 1;

            // теперь получаем номер формата 12-0001 (12 - год, 0001 - номер в 4х значном виде )
            inspection.InspectionNumber = year.ToString(CultureInfo.InvariantCulture).Substring(2) + "-";

            var numStr = inspection.InspectionNum.ToLong().ToString(CultureInfo.InvariantCulture);
            switch (numStr.Length)
            {
                case 1:
                    numStr = "000" + numStr;
                    break;
                case 2:
                    numStr = "00" + numStr;
                    break;
                case 3:
                    numStr = "0" + numStr;
                    break;
            }

            inspection.InspectionNumber += numStr;
        }

        /// <summary>
        /// Получение первого родительского документа с указанным типом
        /// </summary>
        /// <param name="service">DomainService</param>
        /// <param name="document">Дочерний документ</param>
        /// <param name="type">Тип документа, который необходимо получить</param>
        /// <returns></returns>
        public static DocumentGji GetParentDocumentByType(IDomainService<DocumentGjiChildren> service, DocumentGji document, TypeDocumentGji type)
        {
            if (document != null && document.TypeDocumentGji != type)
            {
                var parentDocs = service.GetAll()
                        .Where(x => x.Children.Id == document.Id)
                        .Select(x => x.Parent)
                        .ToList();

                foreach (var doc in parentDocs)
                {
                    document = GetParentDocumentByType(service, doc, type);
                }
            }

            if (document == null || document.TypeDocumentGji != type)
                return null;
            return document;
        }

        /// <summary>
        /// Получение первого дочернего документа с указанным типом
        /// </summary>
        /// <param name="service">DomainService</param>
        /// <param name="document">Родительский документ</param>
        /// <param name="type">Тип документа, который необходимо получить</param>
        /// <returns></returns>
        public static DocumentGji GetChildDocumentByType(IDomainService<DocumentGjiChildren> service, DocumentGji document, TypeDocumentGji type)
        {
            if (document != null && document.TypeDocumentGji != type)
            {
                var parentDocs = service.GetAll()
                    .Where(x => x.Parent.Id == document.Id)
                    .Select(x => x.Children)
                    .ToList();

                foreach (var doc in parentDocs)
                {
                    document = GetChildDocumentByType(service, doc, type);
                }
            }

            if (document == null || document.TypeDocumentGji != type)
                return null;
            return document;
        }

        private static List<DynamicDictionary> GetAllDictionaries(DynamicDictionary dictionary)
        {
            var dictList = new List<DynamicDictionary>();
            if (dictionary == null) return dictList;
            dictList.Add(dictionary);
            foreach (var elem in dictionary.Where(elem => elem.Value is DynamicDictionary))
            {
                dictList.AddRange(GetAllDictionaries((DynamicDictionary)elem.Value));
            }
            return dictList;
        }

        private static DynamicDictionary GetDynamicDictionaryWithValue(DynamicDictionary dictionary, string value)
        {
            var dictList = GetAllComplexFiltersValues(dictionary);
            return dictList.FirstOrDefault(dict => dict.Values.Contains(value));
        }

        private static List<DynamicDictionary> GetAllComplexFiltersValues(DynamicDictionary dictionary)
        {
            var dictList = GetAllDictionaries(dictionary);
            return dictList.Where(d => d.Values.All(v => !(v is DynamicDictionary))).ToList();
        }

        /// <summary>
        /// получает любой параметр для complexFilter из недр динамических словарей
        /// </summary>
        /// <param name="baseParams"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetValueFromComplexFilter(this BaseParams baseParams, string key)
        {
            var complexFilter = baseParams.Params.GetAs<DynamicDictionary>("complexFilter");

            var dict = GetDynamicDictionaryWithValue(complexFilter, key);
            if (dict != null && dict.Any(elem => elem.Value is string && (string)elem.Value == key && elem.Key == "left"))
            {
                return dict.GetValue("right");
            }
            return String.Empty;
        }

        public static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            // top left arc  
            path.AddArc(arc, 180, 90);

            // top right arc  
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc  
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc 
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, Rectangle bounds, int cornerRadius)
        {
            if (graphics == null)
                throw new ArgumentNullException("graphics");
            if (pen == null)
                throw new ArgumentNullException("pen");

            using (GraphicsPath path = RoundedRect(bounds, cornerRadius))
            {
                graphics.DrawPath(pen, path);
            }
        }

        public static void FillRoundedRectangle(this Graphics graphics, Brush brush, Rectangle bounds, int cornerRadius)
        {
            if (graphics == null)
                throw new ArgumentNullException("graphics");
            if (brush == null)
                throw new ArgumentNullException("brush");

            using (GraphicsPath path = RoundedRect(bounds, cornerRadius))
            {
                graphics.FillPath(brush, path);
            }
        }

        private static string SubjParse(string subject, string delimiter)
        {
            string result = string.Empty;
            try
            {
                if (subject == null || subject == "") return result;

                if (!delimiter.EndsWith("=")) delimiter = delimiter + "=";

                if (!subject.Contains(delimiter)) return result;

                int start = subject.IndexOf(" " + delimiter) + delimiter.Length + 1;
                if (start == delimiter.Length)
                {
                    start = subject.IndexOf("," + delimiter) + delimiter.Length + 1;
                    if (start == delimiter.Length)
                    {
                        start = subject.IndexOf(delimiter) + delimiter.Length;
                    }
                    if (start != delimiter.Length && (subject[start - 1] != ' ' || subject[start - 1] != ','))
                    {
                        return result;
                    }
                }

                string e = subject.Substring(start, subject.IndexOf('=', start) == -1 ? subject.Length - start : subject.IndexOf('=', start) - start);
                int tt = e.Length - e.LastIndexOf(", ");

                int length = subject.IndexOf('=', start) == -1 ? subject.Length - start : subject.IndexOf('=', start) - start - tt;

                if (length == 0) return result;
                if (length > 0)
                {
                    result = subject.Substring(start, length);
                }
                else
                {
                    result = subject.Substring(start);
                }
                if (result[0] == '"')
                {
                    result = result.Substring(1, result.Length - 2);
                }
                result = result.Replace("\"\"", "\"");
                result = result.Replace("\'\'", "\'");
                return result;

            }
            catch (Exception)
            {
                return result;
            }
        }

        private static string GetCertInfo(X509Certificate2 cert)
        {
            string CertNotBefore = cert.NotBefore.ToShortDateString();
            string CertNotAfter = cert.NotAfter.ToShortDateString();
            string SerialNumber = cert.SerialNumber;
            string CN = SubjParse(cert.Subject, "CN");
            string SN = SubjParse(cert.Subject, "SN");
            string G = SubjParse(cert.Subject, "G");
            string C = SubjParse(cert.Subject, "C");
            string S = SubjParse(cert.Subject, "S");
            string L = SubjParse(cert.Subject, "L");
            string E = SubjParse(cert.Subject, "E");
            string T = SubjParse(cert.Subject, "T");
            string OU = SubjParse(cert.Subject, "OU");
            string STREET = SubjParse(cert.Subject, "STREET");
            string O = SubjParse(cert.Subject, "O");
            string OGRN = SubjParse(cert.Subject, "ОГРН");
            string INN = SubjParse(cert.Subject, "ИНН");
            string UN = SubjParse(cert.Subject, "OID.1.2.840.113549.1.9.2");
            //            return 
            //$@"  Организация: {O}
            //  Структурное подразделение: {OU}
            //  Должность: {T}
            //  ФИО: {SN} {G}
            //  E-Mail: {E}
            //  Серийный номер: {SerialNumber}
            //  Действителен с: {CertNotBefore}
            //  Действителен по: {CertNotAfter}
            //  Доп. информация: {UN}";
            return
$@"
Сертификат: {SerialNumber}
Владелец: {SN} {G}
Действителен с {CertNotBefore} до {CertNotAfter}";
        }

        public static Image GetFullStamp(Bitmap stamp, string certificate)
        {
            Image bmp = new Bitmap(1004, 444);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                //g.FillRoundedRectangle(Brushes.White, new Rectangle(2, 2, 1000, 440), 20);
                g.DrawRoundedRectangle(new Pen(Color.FromName("Black"), 4), new Rectangle(2, 2, 1000, 440), 20);
                MemoryStream strm = new MemoryStream();
                stamp.Save(strm, System.Drawing.Imaging.ImageFormat.Png);
                g.DrawImage(Image.FromStream(strm), 12, 25, 976, 216);
                Rectangle textRect = new Rectangle(40, 250, 960, 200);
                X509Certificate2 cert = new X509Certificate2(Convert.FromBase64String(certificate));
                Font font = new Font("Arial", 24, FontStyle.Bold);
                g.DrawString(GetCertInfo(cert), font, Brushes.Black, textRect);
            }
            return bmp;
        }
    }
}