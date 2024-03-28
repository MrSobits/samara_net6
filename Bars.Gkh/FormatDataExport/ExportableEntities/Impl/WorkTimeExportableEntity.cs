namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Режим работы, в т.ч. часы личного приема граждан
    /// </summary>
    public class WorkTimeExportableEntity : BaseExportableEntity<ManagingOrgWorkMode>
    {
        /// <inheritdoc />
        public override string Code => "WORKTIME";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Rso |
            FormatDataExportProviderFlags.Ogv |
            FormatDataExportProviderFlags.Oms;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.GetFiltred(x => x.ManagingOrganization.Contragent)
                .Select(x => new
                {
                    x.Id,
                    ContragentId = x.ManagingOrganization.Contragent.ExportId,
                    x.StartDate,
                    x.TypeMode,
                    x.EndDate,
                    x.Pause,
                    x.TypeDayOfWeek
                })
                .AsEnumerable()
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(), // 1. Уникальный код
                        x.ContragentId.ToStr(), // 2. Контрагент
                        x.StartDate.HasValue ? this.No : this.Yes, // 3. Выходной
                        this.GetWorkMode(x.TypeMode), // 4. Режим работы
                        this.GetTime(x.StartDate), // 5. «С»
                        this.GetTime(x.EndDate), // 6. «По»
                        x.Pause.Cut(50), // 7. «Перерыв»
                        this.Yes, // 8. Еженедельно
                        string.Empty, // 9. Комментарий относительно часов приема граждан
                        this.GetDayOfWeek(x.TypeDayOfWeek), // 10. День недели
                        string.Empty, // 11. Телефон
                        string.Empty, // 12. Адрес приема граждан
                    }))
                .ToList();
        }

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Контрагент",
                "Выходной",
                "Режим работы",
                "«С»",
                "«По»",
                "«Перерыв»",
                "Еженедельно",
                "Комментарий относительно часов приема граждан",
                "День недели",
                "Телефон ",
                "Адрес приема граждан"
            };
        }

        private string GetWorkMode(TypeMode typeMode)
        {
            switch (typeMode)
            {
                case TypeMode.WorkMode:
                    return "10";
                case TypeMode.ReceptionCitizens:
                    return "20";
                case TypeMode.DispatcherWork:
                    return "30";
                case TypeMode.ReceptionJurPerson:
                    return "40";
                default:
                    return string.Empty;
            }
        }

        private string GetDayOfWeek(TypeDayOfWeek typeDayOfWeek)
        {
            switch (typeDayOfWeek)
            {
                case TypeDayOfWeek.Monday:
                    return "1";
                case TypeDayOfWeek.Tuesday:
                    return "2";
                case TypeDayOfWeek.Wednesday:
                    return "3";
                case TypeDayOfWeek.Thursday:
                    return "4";
                case TypeDayOfWeek.Friday:
                    return "5";
                case TypeDayOfWeek.Saturday:
                    return "6";
                case TypeDayOfWeek.Sunday:
                    return "7";
                default:
                    return string.Empty;
            }
        }
    }
}