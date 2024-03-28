namespace Bars.GkhDi.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.GkhDi.Enums;

    /// <summary>
    /// Документы фин деятельности (сметы доходов и Заключение рев коммиссии) в разрезе по годам
    /// </summary>
    public class FinActivityDocByYear : BaseGkhEntity
    {
        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }

        /// <summary>
        /// Тип документа по годам
        /// </summary>
        public virtual TypeDocByYearDi TypeDocByYearDi { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public virtual int Year { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        ///  Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }
    }
}
