namespace Bars.GkhGji.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Определение протокола ГЖИ
    /// </summary>
    public class ResolProsDefinition : BaseGkhEntity
    {
        /// <summary>
        /// Протокол
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
        /// документ выдан
        /// </summary>
        public virtual Inspector SignedBy { get; set; }

        /// <summary>
        /// Тип определения
        /// </summary>
        public virtual TypeDefinitionProtocol TypeDefinition { get; set; }

        /// <summary>
        /// Время начала (Данное поле используется в какомто регионе)
        /// </summary>
        public virtual DateTime? TimeDefinition { get; set; }

        /// <summary>
        /// Дата рассмотрения дела (Даное поле используется в каком то регионе)
        /// </summary>
        public virtual DateTime? DateOfProceedings { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }
    }
}