namespace Bars.GkhDi.Entities
{
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Gkh.Enums;

    /// <summary>
    /// Документы сведений об УО объекта недвижимости
    /// </summary>
    public class DocumentsRealityObj : BaseGkhEntity
    {
        /// <summary>
        /// Сведения об УО объекта недвижимости
        /// </summary>
        public virtual DisclosureInfoRealityObj DisclosureInfoRealityObj { get; set; }

        /// <summary>
        /// Акт состояния общего имущества собственников в многоквартирном доме. Файл
        /// </summary>
        public virtual FileInfo FileActState { get; set; }

        /// <summary>
        /// Акт состояния общего имущества собственников в многоквартирном доме. Описание
        /// </summary>
        public virtual string DescriptionActState { get; set; }

        /// <summary>
        /// Перечень работ по содержанию и ремонту. Файл
        /// </summary>
        public virtual FileInfo FileCatalogRepair { get; set; }

        /// <summary>
        /// Отчет о выполнение годового плана мероприятий по содержанию и ремонту. Файл
        /// </summary>
        public virtual FileInfo FileReportPlanRepair { get; set; }

        /// <summary>
        /// Проводились ли общие собрания собственников помещений в МКД с участием УО
        /// </summary>
        public virtual YesNoNotSet HasGeneralMeetingOfOwners { get; set; }
    }
}
