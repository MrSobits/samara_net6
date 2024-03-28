namespace Bars.GisIntegration.Base.Entities.HouseManagement
{
    using B4.DataAccess;
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Подтверждение класса экономической эффективности
    /// </summary>
    public class ConfirmDoc : BaseEntity
    {
        /// <summary>
        /// Ссылка на дом
        /// </summary>
        public virtual RisHouse House { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Ссылка на объект файла в РИС
        /// </summary>
        public virtual Attachment Attachment { get; set; }
    }
}