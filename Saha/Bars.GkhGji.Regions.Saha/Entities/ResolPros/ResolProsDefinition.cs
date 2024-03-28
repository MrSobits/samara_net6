using Bars.B4.DataAccess;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Regions.Saha.Enums;

namespace Bars.GkhGji.Regions.Saha.Entities
{
    using System;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Определение Постановления прокуратуры
    /// </summary>
    public class ResolProsDefinition : BaseEntity
    {
        /// <summary>
        /// Постановления прокуратуры
        /// </summary>
        public virtual ResolPros ResolPros { get; set; }

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
        public virtual TypeDefinitionResolPros TypeDefinition { get; set; }
    }
}