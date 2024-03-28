namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActCheck
{
    using System;

    /// <summary>
    /// Акт проверки ГЖИ для Челябинска (расширяется дополнительными полями)
    /// </summary>
    public class ChelyabinskActCheck : GkhGji.Entities.ActCheck
    {
        /// <summary>
        /// С копией приказа ознакомлен
        /// </summary>
        public virtual string AcquaintedWithDisposalCopy { get; set; }
    }
}