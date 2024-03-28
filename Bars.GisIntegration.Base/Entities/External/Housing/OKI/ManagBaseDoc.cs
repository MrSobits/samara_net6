namespace Bars.GisIntegration.Base.Entities.External.Housing.OKI
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Entities;

    public class ManagBaseDoc : BaseEntity
    {
        /// <summary>
        /// ОКИ
        /// </summary>
        public virtual OkiObject OkiObject { get; set; }
        /// <summary>
        /// Приложение
        /// </summary>
        public virtual Attachment Attachment { get; set; }
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
