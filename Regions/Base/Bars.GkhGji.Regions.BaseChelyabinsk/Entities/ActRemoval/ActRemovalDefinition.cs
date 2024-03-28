namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Определение акта проверки ГЖИ
    /// </summary>
    public class ActRemovalDefinition : BaseGkhEntity
    {
        /// <summary>
        /// Акт проверки предписания
        /// </summary>
		public virtual ActRemoval ActRemoval { get; set; }

        /// <summary>
        /// Дата исполнения
        /// </summary>
        public virtual DateTime? ExecutionDate { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Номер документа (целая часть)
        /// </summary>
        public virtual int? DocumentNumber { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// ДЛ, вынесшее определение
        /// </summary>
        public virtual Inspector IssuedDefinition { get; set; }

        /// <summary>
        /// Тип определения
        /// </summary>
        public virtual TypeDefinitionAct TypeDefinition { get; set; }
    }
}