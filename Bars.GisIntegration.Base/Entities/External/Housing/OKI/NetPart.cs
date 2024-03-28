namespace Bars.GisIntegration.Base.Entities.External.Housing.OKI
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Entities.External.Administration.System;
    using Bars.GisIntegration.Base.Entities.External.Dict.Oki;

    /// <summary>
    /// Участки сети
    /// </summary>
    public class NetPart : BaseEntity
    {
        /// <summary>
        /// Идентификатор ОКИ
        /// </summary>
        public virtual long OkiId { get; set; }

        /// <summary>
        /// Поставщик информации
        /// </summary>
        public virtual DataSupplier DataSupplier { get; set; }

        /// <summary>
        /// ОКИ
        /// </summary>
        public virtual OkiObject OkiObject { get; set; }

        /// <summary>
        /// Наименование участка
        /// </summary>
        public virtual string NetPartName { get; set; }

        /// <summary>
        /// Диаметр (мм)
        /// </summary>
        public virtual decimal Diameter { get; set; }

        /// <summary>
        /// Протяженность (км)
        /// </summary>
        public virtual decimal Length { get; set; }

        /// <summary>
        /// Нуждается в замене (км)
        /// </summary>
        public virtual decimal? ChangeKmCnt { get; set; }

        /// <summary>
        /// Износ (%)
        /// </summary>
        public virtual decimal? Wearout { get; set; }

        /// <summary>
        /// Кем изменено
        /// </summary>
        public virtual int ChangedBy { get; set; }

        /// <summary>
        /// Когда изменено
        /// </summary>
        public virtual DateTime ChangedOn { get; set; }

        /// <summary>
        /// Уровень давления газопровода
        /// </summary>
        public virtual GasPressure GasPressure { get; set; }

        /// <summary>
        /// Уровень напряжения
        /// </summary>
        public virtual Voltage Voltage { get; set; }
    }
}
