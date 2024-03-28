namespace Bars.GisIntegration.Base.Entities.HouseManagement
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Договор ресурсоснабжения и приложения к договору
    /// </summary>
    public class SupResContractAttachment : BaseRisEntity
    {
        /// <summary>
        /// Договор с поставщиком ресурсов
        /// </summary>
        public virtual SupplyResourceContract Contract { get; set; }

        /// <summary>
        /// Вложение
        /// </summary>
        public virtual Attachment Attachment { get; set; }
    }
}
