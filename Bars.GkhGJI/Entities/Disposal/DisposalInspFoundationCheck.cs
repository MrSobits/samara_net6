namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// НПА проверки приказа ГЖИ
    /// </summary>
    public class DisposalInspFoundationCheck : BaseGkhEntity, IEntityUsedInErp
    {
        /// <summary>
        /// Распоряжение ГЖИ 
        /// </summary>
        public virtual Disposal Disposal { get; set; }

        /// <summary>
        /// Правовое основание проведения проверки
        /// </summary>
        public virtual NormativeDoc InspFoundationCheck { get; set; }

        /// <summary>
        /// Гуид ЕРП
        /// </summary>
        public virtual string ErpGuid { get; set; }
    }
}