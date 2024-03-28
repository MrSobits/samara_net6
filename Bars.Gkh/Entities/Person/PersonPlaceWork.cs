namespace Bars.Gkh.Entities
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// место работы физ лица 
    /// </summary>
    public class PersonPlaceWork : BaseImportableEntity
    {

        public virtual Person Person { get; set; }

        /// <summary>
        /// УО, ставлю ссылку на контрагента на всякий случай если вдруг захотят нетолько по УО но и по подрядчикам заполнять места работы
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime? StartDate { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime? EndDate { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public virtual Position Position { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }

    }
}
