namespace Bars.GkhGji.Regions.Tomsk.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Enums;

    /// <summary>
    /// Административное дело
    /// </summary>
    public class AdministrativeCase : DocumentGji
    {
        /// <summary>
        /// Тип Основания
        /// </summary>
        public virtual TypeAdminCaseBase TypeAdminCaseBase { get; set; }

        /// <summary>
        /// Дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Инспектор
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Вопрос
        /// </summary>
        public virtual string DescriptionQuestion { get; set; }

        /// <summary>
        /// Установил
        /// </summary>
        public virtual string DescriptionSet { get; set; }

        /// <summary>
        /// Определил
        /// </summary>
        public virtual string DescriptionDefined { get; set; }

        /// <summary>
        /// Не хранимое поле InspectionId потому что поле Inspection JSONIgnore и чтобы работать на клиенте нужен id инспекции
        /// </summary>
        public virtual long InspectionId { get; set; }

    }
}
