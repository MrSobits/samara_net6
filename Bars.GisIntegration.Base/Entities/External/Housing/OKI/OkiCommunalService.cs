namespace Bars.GisIntegration.Base.Entities.External.Housing.OKI
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Entities.External.Administration.System;
    using Bars.GisIntegration.Base.Entities.External.Dict.Oki;

    /// <summary>
    /// ОКИ коммунальные услуги
    /// </summary>
    public class OkiCommunalService : BaseEntity
    {
        /// <summary>
        /// ОКИ
        /// </summary>
        public virtual OkiObject OkiObject { get; set; }
        /// <summary>
        /// Поставщик информации
        /// </summary>
        public virtual DataSupplier DataSupplier { get; set; }
        /// <summary>
        /// Код коммунальной услуги.
        /// </summary>
        public virtual ExtCommunalService CommunalService { get; set; }
        /// <summary>
        /// Пользователь 
        /// </summary>
        public virtual int ChangedBy { get; set; }
        /// <summary>
        /// Дата изменения 
        /// </summary>
        public virtual DateTime ChangedOn { get; set; }
    }
}
