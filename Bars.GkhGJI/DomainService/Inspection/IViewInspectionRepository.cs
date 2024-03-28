namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Linq;

    using Bars.B4.DataModels;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.FormatProvider;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// ReadOnly репозиторий проверок ГЖИ для экспорта <see cref="IExportFormatProvider"/>
    /// </summary>
    public interface IViewInspectionRepository
    {
        /// <summary>
        /// Метод формирования запроса получения Dto
        /// </summary>
        IQueryable<ViewFormatDataExportInspection> GetAll();

        /// <summary>
        /// Метод формирования запроса получения Dto
        /// </summary>
        IQueryable<ViewFormatDataExportInspectionDto> GetAllDto();
    }

    /// <summary>
    /// Dto представления проверки ГЖИ для экспорта <see cref="IExportFormatProvider"/>
    /// </summary>
    public class ViewFormatDataExportInspectionDto: IHaveId
    {
        /// <summary>
        /// Идентификатор проверки <see cref="InspectionGji"/>
        /// </summary>
        public virtual long Id { get; set; }

        /// <summary>
        /// Идентификатор распоряжения <see cref="Disposal"/>
        /// </summary>
        public virtual long DisposalId { get; set; }

        /// <summary>
        /// Идентификатор акта проверки <see cref="ActCheck"/>
        /// </summary>
        public virtual long ActCheckId { get; set; }

        /// <summary>
        /// Проверка плановая
        /// </summary>
        public virtual bool IsPlanned { get; set; }

        /// <summary>
        /// Тип основания проверки
        /// </summary>
        public virtual TypeBase TypeBase { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Дата проверки
        /// </summary>
        public virtual DateTime? CheckDate { get; set; }

        /// <summary>
        /// Наименование субъекта проверки
        /// </summary>
        public virtual string ContragentName { get; set; }

        /// <summary>
        /// Наименование муниципального образования
        /// </summary>
        public virtual string MunicipalityName { get; set; }
    }
}