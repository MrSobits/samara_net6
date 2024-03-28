namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.Gkh.Enums;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.FileStorage;
    using Gkh.Entities;

    /// <summary>
    /// Владелец спецсчета
    /// </summary>
    public class SpecialAccountOwner : BaseGkhEntity
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual OrgStateRole OrgStateRole { get; set; }

      
        //Деятельность
        /// <summary>
        /// Дата окончания деятельности
        /// </summary>
        public virtual DateTime? ActivityDateEnd { get; set; }

        /// <summary>
        /// Основание прекращения деятельности
        /// </summary>
        public virtual GroundsTermination ActivityGroundsTermination { get; set; }

    }
}
