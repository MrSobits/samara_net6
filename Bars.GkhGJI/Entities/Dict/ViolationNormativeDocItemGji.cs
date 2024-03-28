namespace Bars.GkhGji.Entities.Dict
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Сущность связи нарушения и пункта нормативного документа
    /// </summary>
    public class ViolationNormativeDocItemGji : BaseEntity
    {
        /// <summary>
        /// Нарушение
        /// </summary>
        public virtual ViolationGji ViolationGji { get; set; }

        /// <summary>
        /// Пункт нормативного документа
        /// </summary>
        public virtual NormativeDocItem NormativeDocItem { get; set; }

        /// <summary>
        /// Состав правонарушения
        /// </summary>
        public virtual string ViolationStructure { get; set; }
    }
}