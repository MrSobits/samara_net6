namespace Bars.Gkh.Entities.CommonEstateObject
{
    using Bars.Gkh.FormatDataExport.ExportableEntities;

    using Entities;
    using Dicts;
    using Enums;

    /// <summary>
    /// Конструктивный элемент
    /// </summary>
    public class StructuralElement : BaseImportableEntity, IHaveExportId
    {
        /// <summary>
        /// Группа
        /// </summary>
        public virtual StructuralElementGroup Group { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Ед. измерения
        /// </summary>
        public virtual UnitMeasure UnitMeasure { get; set; }

        /// <summary>
        /// Срок эксплуатации
        /// </summary>
        public virtual int LifeTime { get; set; }

        /// <summary>
        /// Срок эксплуатации после ремонта
        /// </summary>
        public virtual int LifeTimeAfterRepair { get; set; }

        /// <summary>
        /// Нормативный документ
        /// </summary>
        public virtual NormativeDoc NormativeDoc { get; set; }

        /// <summary>
        /// Группа взаимоисключаемости
        /// </summary>
        public virtual string MutuallyExclusiveGroup { get; set; }

        /// <summary>
        /// Значение при вычислении стоимости ремонта
        /// </summary>
        public virtual PriceCalculateBy CalculateBy { get; set; }

        /// <summary>
        /// Код реформы ЖКХ
        /// </summary>
        public virtual string ReformCode { get; set; }

        /// <inheritdoc />
        /// <see cref="FormatDataExportSequences.WorkKprTypeExportId"/>
        public virtual long ExportId { get; set; }
    }
}