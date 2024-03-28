namespace Bars.Gkh.Entities.Dicts
{
    using System;

    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;

    /// <summary>
    /// Работы
    /// </summary>
    public class Work : BaseGkhEntity, IHaveExportId
    {
        /// <inheritdoc />
        /// <see cref="FormatDataExportSequences.WorkKprTypeExportId"/>
        public virtual long ExportId { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Код реформы
        /// </summary>
        public virtual string ReformCode { get; set; }

        /// <summary>
        /// Код ГИС
        /// </summary>
        public virtual string GisCode { get; set; }

        /// <summary>
        /// Ед. измерения
        /// </summary>
        public virtual UnitMeasure UnitMeasure { get; set; }

        /// <summary>
        /// Норматив
        /// </summary>
        public virtual decimal? Normative { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Соответсвие 185 ФЗ
        /// </summary>
        public virtual bool Consistent185Fz { get; set; }

        /// <summary>
        /// Дополнительная работа, нужно для ДПКР
        /// </summary>
        public virtual bool IsAdditionalWork { get; set; }

        /// <summary>
        /// Работа (услуга) по строительству (Татарстан)
        /// </summary>
        public virtual bool IsConstructionWork { get; set; }

        /// <summary>
        /// Работа (услуга) по ПСД
        /// </summary>
        public virtual bool IsPSD { get; set; }

        /// <summary>
        /// Дата окончания работ
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Тип работ
        /// </summary>
        public virtual TypeWork TypeWork { get; set; }

        /// <summary>
        /// Назначение работ
        /// </summary>
        public virtual WorkAssignment? WorkAssignment { get; set; }

        /// <summary>
        /// Код ГИС ЖКХ
        /// </summary>
        public virtual string GisGkhCode { get; set; }

        /// <summary>
        /// ГИС ЖКХ Guid
        /// </summary>
        public virtual string GisGkhGuid { get; set; }
        
        /// Актуальность
        /// </summary>
        public virtual bool IsActual { get; set; }

        /// <summary>
        /// Проводится в рамках краткосрочной программы
        /// </summary>
        public virtual bool WithinShortProgram { get; set; }
    }
}
