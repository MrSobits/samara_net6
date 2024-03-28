namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;

    /// <summary>
    /// Документация искового зявления
    /// </summary>
    public class LawSuitDebtWorkSSPDocument : BaseEntity
    {
        /// <summary>
        /// Тип документа
        /// </summary>
        public virtual LawSuitDebtWorkSSP LawSuitDebtWorkSSP { get; set; }

        /// <summary>
        /// Наименование документа
        /// </summary>
        public virtual TypeLawsuitDocument TypeLawsuitDocument { get; set; }

        /// <summary>
        /// ИП в отношении
        /// </summary>
        public virtual CollectDebtFrom CollectDebtFrom { get; set; }
        
        /// <summary>
        /// Наименование РОСП
        /// </summary>
        public virtual JurInstitution Rosp { get; set; }
        
        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual int Number { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Note { get; set; }
    }
}