namespace Bars.GisIntegration.Base.Entities.External.Housing.OKI
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Entities.External.Administration.System;

    /// <summary>
    /// Источник/Приемник
    /// </summary>
    public class SourceReceiver : BaseEntity
    {
        /// <summary>
        /// Идентификатор ОКИ
        /// </summary>
        public virtual long OkiObjectId { get; set; }
        /// <summary>
        /// Поставщик информации
        /// </summary>
        public virtual DataSupplier DataSupplier { get; set; }
        /// <summary>
        /// ОКИ
        /// </summary>
        public virtual OkiObject OkiObject { get; set; }
        /// <summary>
        /// Идентификатор ОКИ-дочернего
        /// </summary>
        public virtual long ChildOkiObjectId { get; set; }
        /// <summary>
        /// ОКИ-дочерний
        /// </summary>
        public virtual OkiObject ChildOkiObject { get; set; }
        /// <summary>
        ///  Источник/Приемник
        /// </summary>
        public virtual bool IsSource { get; set; }
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
