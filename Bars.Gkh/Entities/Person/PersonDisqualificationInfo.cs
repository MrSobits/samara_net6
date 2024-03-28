namespace Bars.Gkh.Entities
{
    using System;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Сведения о дисквалификации
    /// </summary>
    public class PersonDisqualificationInfo : BaseImportableEntity
    {

        public virtual Person Person { get; set; }

        
        /// <summary>
        /// Основание дисвалификации
        /// </summary>
        public virtual TypePersonDisqualification TypeDisqualification { get; set; }

        /// <summary>
        /// Дата дисквалификации
        /// </summary>
        public virtual DateTime? DisqDate { get; set; }

        /// <summary>
        /// Дата окончания 
        /// </summary>
        public virtual DateTime? EndDisqDate { get; set; }

        /// <summary>
        /// Ходотайство - дата
        /// </summary>
        public virtual DateTime? PetitionDate { get; set; }

        /// <summary>
        /// Ходотайство - номер
        /// </summary>
        public virtual string PetitionNumber { get; set; }

        /// <summary>
        /// Ходотайство - файл
        /// </summary>
        public virtual FileInfo PetitionFile { get; set; }

        /// <summary>
        /// Наименование суда
        /// </summary>
        public virtual string NameOfCourt { get; set; }
    }
}
