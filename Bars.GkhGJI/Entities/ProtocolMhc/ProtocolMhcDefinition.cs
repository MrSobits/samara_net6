namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Определение протокола МЖК
    /// </summary>
    public class ProtocolMhcDefinition : BaseGkhEntity
    {
        /// <summary>
        /// Протокол
        /// </summary>
        public virtual ProtocolMhc ProtocolMhc { get; set; }

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
        /// Номер документа
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
        public virtual TypeDefinitionProtocolMhc TypeDefinition { get; set; }
    }
}