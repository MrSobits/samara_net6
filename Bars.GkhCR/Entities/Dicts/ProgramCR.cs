namespace Bars.GkhCr.Entities
{
    using Bars.B4.Modules.FileStorage;
    using System;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.GkhCr.Enums;

    /// <summary>
    /// Программа капитального ремонта
    /// </summary>
    public class ProgramCr : BaseGkhEntity, IHaveExportId
    {
        /// <summary>
        /// Видимость
        /// </summary>
        public virtual TypeVisibilityProgramCr TypeVisibilityProgramCr { get; set; }

        /// <summary>
        /// Период
        /// </summary>
        public virtual Period Period { get; set; }

        /// <summary>
        /// Тип КР
        /// </summary>
        public virtual TypeProgramCr TypeProgramCr { get; set; }

        /// <summary>
        /// Состояние
        /// </summary>
        public virtual TypeProgramStateCr TypeProgramStateCr { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Используется при экспорте
        /// </summary>
        public virtual bool UsedInExport { get; set; }

        /// <summary>
        /// Не доступно добавление домов
        /// </summary>
        public virtual bool NotAddHome { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Наименование программы
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Соответствует ФЗ
        /// </summary>
        public virtual bool MatchFl { get; set; }

        /// <summary>
        /// Для специальных счетов
        /// </summary>
        public virtual bool ForSpecialAccount { get; set; }

        /// <summary>
        /// Использовать для отчетов Реформы и ГИС ЖКХ
        /// </summary>
        public virtual bool UseForReformaAndGisGkhReports { get; set; }

        /// <summary>
        /// Добавление видов работ из ДПКР
        /// </summary>
        public virtual AddWorkFromLongProgram AddWorkFromLongProgram { get; set; }

        /// <summary>
        /// Постановление об утверждении КП
        /// </summary>
        public virtual NormativeDoc NormativeDoc { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
        
        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Орган, принявший документ
        /// </summary>
        public virtual string DocumentDepartment { get; set; }

        /// <summary>
        /// ГИС ЖКХ GUID
        /// </summary>
        public virtual string GisGkhGuid { get; set; }

        /// <summary>
        /// ГИС ЖКХ Transport GUID
        /// </summary>
        public virtual string GisGkhTransportGuid { get; set; }

        /// <summary>
        /// ГИС ЖКХ GUID вложения документа плана
        /// </summary>
        public virtual string GisGkhDocumentAttachmentGuid { get; set; }

        /// <summary>
        /// ГИС ЖКХ GUID документа плана
        /// </summary>
        public virtual string GisGkhDocumentGuid { get; set; }

        /// <summary>
        /// ГИС ЖКХ Transport GUID документа плана
        /// </summary>
        public virtual string GisGkhDocumentTransportGuid { get; set; }

        /// <summary>
        /// Флажок для выгрузки договоров
        /// </summary>
        public virtual bool ImportContract { get; set; }
        
        /// Государственный заказчик
        /// </summary>
        public virtual Contragent GovCustomer { get; set; }

        /// <inheritdoc />
        public virtual long ExportId { get; set; }
    }
}
