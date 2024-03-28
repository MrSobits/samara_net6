namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Контрагенты, проводящие проверку юр. лица
    /// </summary>
    public class BaseJurPersonContragent : BaseEntity
    {
        /// <summary>
        /// Проверка Юр. лица
        /// </summary>
        public virtual BaseJurPerson BaseJurPerson { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }
    }
}