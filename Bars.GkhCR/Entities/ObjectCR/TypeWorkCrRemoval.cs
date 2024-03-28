namespace Bars.GkhCr.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.GkhCr.Enums;

    using Gkh.Entities;

    /// <summary>
    /// Причина изменения вида работы объекта КР
    /// </summary>
    public class TypeWorkCrRemoval : BaseGkhEntity
    {
        /// <summary>
        /// Вид работы объекта КР
        /// </summary>
        public virtual TypeWorkCr TypeWorkCr { get; set; }

        /// <summary>
        ///  Причина удаления
        /// </summary>
        public virtual TypeWorkCrReason TypeReason { get; set; }

        /// <summary>
        ///  Год выполнения по долгосрочной программе
        /// </summary>
        public virtual int? YearRepair { get; set; }

        /// <summary>
        ///  Новый год выполнения 
        /// </summary>
        public virtual int? NewYearRepair { get; set; }

        /// <summary>
        /// Документ (основание)
        /// </summary>
        public virtual FileInfo FileDoc { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string NumDoc { get; set; }

        /// <summary>
        /// Конструктивный элемент
        /// </summary>
        public virtual string StructElement { get; set; }

        /// <summary>
        /// Дата документа От
        /// </summary>
        public virtual DateTime? DateDoc { get; set; }

        /// <summary>
        /// Документ Основание
        /// </summary>
        public virtual string Description { get; set; }
    }
}
