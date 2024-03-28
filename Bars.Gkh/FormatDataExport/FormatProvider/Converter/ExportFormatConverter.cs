namespace Bars.Gkh.FormatDataExport.FormatProvider.Converter
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;

    using Bars.B4.DataModels;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;

    /// <summary>
    /// Конвертер данных приведения к формату экспорта
    /// </summary>
    public class ExportFormatConverter : IExportFormatConverter
    {
        private const int DefaultDecimalPlaces = 2;
        private const string DefaultDocumentNumber = "б/н";
        private readonly Regex dateRegEx = new Regex(@"\D(\d{1,2}\.\d{1,2}\.\d{2,4})|\D(\d{1,2}\s\w+?\s\d{2,4})\D", RegexOptions.Compiled);
        private readonly Regex fullNameMatcher = new Regex(@"([А-ЯA-Z][а-яА-Яa-zA-Z\-]*)", RegexOptions.Compiled);

        /// <summary>
        /// Да (1)
        /// </summary>
        public string Yes => "1";

        /// <summary>
        /// Нет (2)
        /// </summary>
        public string No => "2";

        /// <summary>
        /// Не задано (0)
        /// </summary>
        public string NotSet => "0";

        /// <inheritdoc />
        public string GetDate(DateTime? date)
        {
            return date?.ToString("dd.MM.yyyy") ?? string.Empty;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetDate(string date)
        {
            DateTime dt;
            return DateTime.TryParse(date, out dt) ? dt.ToString("dd.MM.yyyy") : string.Empty;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetDateTime(DateTime? date)
        {
            return date?.ToString("dd.MM.yyyy HH:mm:ss") ?? string.Empty;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetTime(DateTime? date)
        {
            return date?.ToString("HH:mm:ss") ?? string.Empty;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetFirstDateYear(int? year)
        {
            return year != null && year >= 1 && year <= 9999 ? this.GetDate(new DateTime(year.Value, 1, 1)) : string.Empty;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetFirstDateYear(string year)
        {
            if (string.IsNullOrEmpty(year) || year == "0")
            {
                return string.Empty;
            }

            return year.Length <= 4 ? this.GetDate(new DateTime(year.ToInt(), 1, 1)) : string.Empty;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetDecimal(string decimalValue)
        {
            return this.GetDecimal(decimalValue, ExportFormatConverter.DefaultDecimalPlaces);
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetDecimal(string decimalValue, int decimalPlaces)
        {
            decimal result;
            if (!decimal.TryParse(decimalValue, out result))
            {
                return string.Empty;
            }

            return result == decimal.Zero
                ? "0"
                : result.ToString($"F{decimalPlaces}", System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetDecimal(decimal? decimalValue)
        {
            return this.GetDecimal(decimalValue, ExportFormatConverter.DefaultDecimalPlaces);
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetDecimal(decimal? decimalValue, int decimalPlaces)
        {
            if (!decimalValue.HasValue)
            {
                return string.Empty;
            }

            return decimalValue.Value == decimal.Zero
                ? "0"
                : decimalValue.Value.ToString($"F{decimalPlaces}", System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetDocumentNumber(string number)
        {
            return string.IsNullOrEmpty(number) ? ExportFormatConverter.DefaultDocumentNumber : number;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetDocumentNumber(string number, string defaultValue)
        {
            return string.IsNullOrEmpty(number) ? defaultValue : number;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetDocumentNumber(int? number)
        {
            return number?.ToString() ?? ExportFormatConverter.DefaultDocumentNumber;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetDocumentNumber(int? number, string defaultValue)
        {
            return number?.ToString() ?? defaultValue;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetPaymentFoundation(ManOrgSetPaymentsOwnersFoundation paymentsOwnersFoundation)
        {
            switch (paymentsOwnersFoundation)
            {
                case ManOrgSetPaymentsOwnersFoundation.OwnersMeetingProtocol:
                    return this.Yes;

                case ManOrgSetPaymentsOwnersFoundation.OpenTenderResult:
                    return this.No;

                default:
                    return string.Empty;
            }
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetStrId(IHaveId entity)
        {
            // ReSharper disable once MergeConditionalExpression
            return entity != null ? entity.Id.ToString() : null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetStrId(IHaveExportId entity)
        {
            // ReSharper disable once MergeConditionalExpression
            return entity != null ? entity.ExportId.ToString() : null;
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetValueOrDefault(string value, string defaultValue = null)
        {
            return value.IsNotEmpty() ? value : defaultValue;
        }

        /// <inheritdoc />
        public string FindDate(string stringWithDate)
        {
            if (!string.IsNullOrWhiteSpace(stringWithDate))
            {
                var match = this.dateRegEx.Match(stringWithDate);

                if (match.Success)
                {
                    for (var i = 1; i < match.Groups.Count; i++)
                    {
                        if (match.Groups[i].Success)
                        {
                            return this.GetDate(match.Groups[i].Value);
                        }
                    }
                }
            }

            return string.Empty;
        }

        /// <inheritdoc />
        public string GetNotZeroValue(int? value)
        {
            return value > 0 ? value.ToStr() : null;
        }

        /// <inheritdoc />
        public string GetNotZeroValue(decimal? value)
        {
            return value > 0 ? this.GetDecimal(value) : null;
        }

        /// <inheritdoc />
        public string GetNotZeroValue(decimal? value, int decimalPlaces)
        {
            return value > 0 ? this.GetDecimal(value, decimalPlaces) : null;
        }

        public Tuple<string, string, string> ParseFullName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Tuple.Create<string, string, string>(null, null, null);
            }

            var matches = this.fullNameMatcher.Matches(name);
            var res = new string[3];
            var count = 0;
            foreach (Match match in matches)
            {
                if (!match.Success || count == 3)
                {
                    break;
                }
                res[count++] = match.Value;
            }
            return Tuple.Create(res[0], res[1], res[2]);
        }
    }
}