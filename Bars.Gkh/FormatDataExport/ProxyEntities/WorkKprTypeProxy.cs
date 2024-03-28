namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using Bars.B4.DataModels;

    /// <summary>
    /// 2.22.6. Справочник «Вид работ капитального ремонта» (workkprtype.csv)
    /// </summary>
    public class WorkKprTypeProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код работы/услуги
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Группа работ
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// 3. Наименование вида работ
        /// </summary>
        public string Name { get; set; }
    }
}