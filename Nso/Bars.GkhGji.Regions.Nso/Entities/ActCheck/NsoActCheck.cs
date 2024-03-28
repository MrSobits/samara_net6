namespace Bars.GkhGji.Regions.Nso.Entities
{
    /// <summary>
    /// Акт проверки ГЖИ для Нсо (расширяется дополнительными полями)
    /// </summary>
    public class NsoActCheck : GkhGji.Entities.ActCheck
    {
        /// <summary>
        /// С копией приказа ознакомлен
        /// </summary>
        public virtual string AcquaintedWithDisposalCopy { get; set; }
    }
}