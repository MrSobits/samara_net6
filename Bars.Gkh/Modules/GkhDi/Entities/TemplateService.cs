namespace Bars.GkhDi.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.GkhDi.Enums;

    /// <summary>
    /// Шаблонная услуга
    /// </summary>
    public class TemplateService : BaseGkhEntity, IHaveExportId
    {
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
        /// Характеристика
        /// </summary>
        public virtual string Characteristic { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public virtual UnitMeasure UnitMeasure { get; set; }

        /// <summary>
        /// Изменяемая
        /// </summary>
        public virtual bool Changeable { get; set; }

        /// <summary>
        /// Группа услуги
        /// </summary>
        public virtual TypeGroupServiceDi TypeGroupServiceDi { get; set; }

        /// <summary>
        /// Вид услуги
        /// </summary>
        public virtual KindServiceDi KindServiceDi { get; set; }

        /// <summary>
        /// Яв-ся обязательной
        /// </summary>
        public virtual bool IsMandatory { get; set; }

        /// <summary>
        /// Вид коммунального ресурса
        /// </summary>
        public virtual TypeCommunalResource? CommunalResourceType { get; set; }

        /// <summary>
        /// Вид жилищной услуги
        /// </summary>
        public virtual TypeHousingResource? HousingResourceType { get; set; }

        /// <summary>
        /// Год, с которого услуга обязательна
        /// </summary>
        public virtual int? ActualYearStart { get; set; }

        /// <summary>
        /// Год, до которого услуга обязательна
        /// </summary>
        public virtual int? ActualYearEnd { get; set; }

        /// <summary>
        /// Учитывать при расчете процента
        /// </summary>
        public virtual bool IsConsiderInCalc { get; set; }
    }
}
