namespace Bars.GkhGji.Regions.Tomsk.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Regions.Tomsk.Enums;

    /// <summary>
    /// Документ административного дела
    /// </summary>
    public class AdministrativeCaseDoc : BaseEntity
    {
        /// <summary>
        /// Административное дело
        /// </summary>
        public virtual AdministrativeCase AdministrativeCase { get; set; }

        /// <summary>
        /// Тип документа дела
        /// </summary>
        public virtual TypeAdminCaseDoc TypeAdminCaseDoc { get; set; }

        /// <summary>
        /// Полный номер относительно Адм. Дела
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Номер определения
        /// </summary>
        public virtual int? DocumentNum { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// ДЛ, рассмотревший хадатайство (уполномоченный)
        /// </summary>
        public virtual Inspector EntitiedInspector { get; set; }

        /// <summary>
        /// Необходимый срок
        /// </summary>
        public virtual DateTime? NeedTerm { get; set; }

        /// <summary>
        /// Продлен до
        /// </summary>
        public virtual DateTime? RenewalTerm { get; set; }

        /// <summary>
        /// Установил
        /// </summary>
        public virtual string DescriptionSet { get; set; }
        
    }
}