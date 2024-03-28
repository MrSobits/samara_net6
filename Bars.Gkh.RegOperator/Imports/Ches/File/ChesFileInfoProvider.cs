namespace Bars.Gkh.RegOperator.Imports.Ches
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Import;
    using Bars.Gkh.RegOperator.Domain;

    /// <summary>
    /// Поставщик описания импортируемых файлов ЧЭС
    /// </summary>
    public static class ChesFileInfoProvider
    {
        /// <summary>
        /// Вернуть описание файла
        /// </summary>
        /// <param name="fileType">Тип файла</param>
        /// <param name="fileData">Данные файла</param>
        /// <param name="logImport">Лог импорта</param>
        /// <param name="indicate">Метод индикации</param>
        public static ImportFileInfo GetFileInfo(FileType fileType, FileData fileData, ILogImport logImport, Action<int, string> indicate)
        {
            var regex = new Regex(@"_(?<year>\d{2})(?<month>\d{2})");
            
            if (fileType == FileType.Unknown || !regex.IsMatch(fileData.FileName))
            {
                throw new ImportException($"Файл {fileData.FileName} имеет неверное наименование");
            }

            var year = regex.Match(fileData.FileName).Result("${year}").ToInt();
            var month = regex.Match(fileData.FileName).Result("${month}").ToInt();

            return ChesFileInfoProvider.GetFileInfo(fileType, 2000 + year, month, null, null, fileData, logImport, indicate);
        }

        /// <summary>
        /// Вернуть описание файла в новом формате
        /// </summary>
        /// <param name="fileType">Тип файла</param>
        /// <param name="fileData">Данные файла</param>
        /// <param name="logImport">Лог импорта</param>
        /// <param name="indicate">Метод индикации</param>
        public static ImportFileInfo GetFileInfoEx(FileType fileType, FileData fileData, ILogImport logImport, Action<int, string> indicate)
        {
            var regex = new Regex(@"_(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})(?<version>\d{2})");

            if (fileType == FileType.Unknown || !regex.IsMatch(fileData.FileName))
            {
                return ChesFileInfoProvider.GetFileInfo(fileType, fileData, logImport, indicate);
            }

            var year = regex.Match(fileData.FileName).Result("${year}").ToInt();
            var month = regex.Match(fileData.FileName).Result("${month}").ToInt();
            var day = regex.Match(fileData.FileName).Result("${day}").ToInt();
            var version = regex.Match(fileData.FileName).Result("${version}").ToInt();

            return ChesFileInfoProvider.GetFileInfo(fileType, year, month, day, version, fileData, logImport, indicate);
        }

        /// <summary>
        /// Вернуть описание файла
        /// </summary>
        /// <param name="fileType">Тип файла</param>
        /// <param name="year">Год</param>
        /// <param name="month">Месяц</param>
        /// <param name="day">День</param>
        /// <param name="version">Версия</param>
        /// <param name="fileData">Данные файла</param>
        /// <param name="logImport">Лог импорта</param>
        /// <param name="indicate">Метод индикации</param>
        public static ImportFileInfo GetFileInfo(
            FileType fileType,
            int year,
            int month,
            int? day = null,
            int? version = null,
            FileData fileData = null,
            ILogImport logImport = null,
            Action<int, string> indicate = null)
        {
            ImportFileInfo importFileInfo;
            var currentPeriod = ChargePeriodProvider.Repository.GetPeriodByDate(new DateTime(year, month, 1), true);

            if (currentPeriod.IsNull())
            {
                var date = new DateTime(year, month, 1);
                throw new ValidationException($"Отсутствует период в системе на указанный год и месяц ({date.ToString("yyyy MMMM", CultureInfo.GetCultureInfo("ru-RU"))})");
            }

            switch (fileType)
            {
                case FileType.Account:
                    importFileInfo = new AccountFileInfo(fileType, fileData, logImport, currentPeriod, indicate);
                    break;

                case FileType.Calc:
                    importFileInfo = new CalcFileInfo(fileType, fileData, logImport, currentPeriod, indicate);
                    break;

                case FileType.CalcProt:
                    importFileInfo = new CalcProtFileInfo(fileType, fileData, logImport, currentPeriod, indicate);
                    break;

                case FileType.SaldoChange:
                    importFileInfo =
                        new SaldoChangeFileInfo(fileType, fileData, logImport, currentPeriod, indicate);
                    break;

                case FileType.Recalc:
                    importFileInfo = new RecalcFileInfo(fileType, fileData, logImport, currentPeriod, indicate);
                    break;

                case FileType.Pay:
                    importFileInfo = new PayFileInfo(fileType, fileData, logImport, currentPeriod, indicate)
                    {
                        Version = version,
                        PaymentDay = day.HasValue ? new DateTime(year, month, day.Value) : (DateTime?) null
                    };
                    break;
                default:
                    throw new ImportException($"Файл {fileData?.FileName} имеет неверное наименование");
            }
            return importFileInfo;
        }
    }
}