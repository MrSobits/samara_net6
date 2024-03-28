namespace Bars.GkhGji.Regions.Smolensk.Entities
{
    /// <summary>
    /// Рапоряжение ГЖИ для Смоленск (расширяется дополнительными полями)
    /// </summary>
    public class DisposalSmol : Bars.GkhGji.Entities.Disposal
    {
        /// <summary>
        /// Цель проверки
        /// </summary>
        public virtual string VerificationPurpose { get; set; }
    }
}