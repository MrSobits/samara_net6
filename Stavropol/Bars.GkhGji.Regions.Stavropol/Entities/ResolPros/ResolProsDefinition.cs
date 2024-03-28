using System;
using Bars.Gkh.Entities;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Bars.GkhGji.Regions.Stavropol.Enums;

namespace Bars.GkhGji.Regions.Stavropol.Entities
{
	/// <summary>
    /// Определение постановления прокуратуры ГЖИ
    /// </summary>
	public class ResolProsDefinition : BaseGkhEntity
    {
        /// <summary>
        /// Постановление прокуратуры
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
        public virtual TypeDefinitionResolPros TypeDefinition { get; set; }

        /// <summary>
        /// Время начала (Данное поле используется в какомто регионе)
        /// </summary>
        public virtual DateTime? TimeDefinition { get; set; }

        /// <summary>
        /// Дата рассмотрения дела (Даное поле используется в каком то регионе)
        /// </summary>
        public virtual DateTime? DateOfProceedings { get; set; }
    }
}