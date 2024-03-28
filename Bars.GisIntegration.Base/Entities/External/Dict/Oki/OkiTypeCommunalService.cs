namespace Bars.GisIntegration.Base.Entities.External.Dict.Oki
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Entities.External.Administration.System;

    /// <summary>
    /// Справочник базовых услуг для видов ОКИ
    /// </summary>
    public class OkiTypeCommunalService : BaseEntity
    {
        /// <summary>
        /// Поставщик информации
        /// </summary>
        public virtual DataSupplier DataSupplier { get; set; }
        /// <summary>
        /// Вид ОКИ (8)
        /// </summary>
        public virtual OkiType OkiType { get; set; }
        /// <summary>
        /// Вид коммунальной услуги.
        /// </summary>
        public virtual ExtCommunalService CommunalService { get; set; }
        /// <summary>
        /// Кем изменено
        /// </summary>
        public virtual int ChangedBy { get; set; }
        /// <summary>
        /// Когда изменено
        /// </summary>
        public virtual DateTime ChangedOn { get; set; }

    }
}
