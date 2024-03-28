namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Справочник работ/услуг
    /// </summary>
    public class WorkUslugaProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код работы/услуги
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Наименование работы/услуги
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 3. Базовая работа/услуга организации
        /// </summary>
        public int? BaseService { get; set; }

        /// <summary>
        /// 4. Вид работ
        /// </summary>
        public int? Type { get; set; }

        /// <summary>
        /// 5. Код ОКЕИ
        /// </summary>
        public string OkeiCode { get; set; }

        /// <summary>
        /// 6. Другая единица измерения
        /// </summary>
        public string AnotherUnit { get; set; }

        /// <summary>
        /// 7. Родительская работа/услуга <see cref="DictUslugaProxy"/>
        /// </summary>
        [ProxyId(typeof(DictUslugaProxy))]
        public long? ParentServiceId { get; set; }
    }
}