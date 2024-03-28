namespace Bars.GkhGji.Regions.Nso.Entities
{
	using Bars.GkhGji.Entities;
    using System;

    /// <summary>
    /// Акт проверки предписания ГЖИ для НСО (расширяется дополнительными полями)
    /// </summary>
	public class NsoActRemoval : ActRemoval
    {
        /// <summary>
        /// С копией приказа ознакомлен
        /// </summary>
        public virtual string AcquaintedWithDisposalCopy { get; set; }

        /// <summary>
        /// Место составления
        /// </summary>
        public virtual string DocumentPlace { get; set; }

        /// <summary>
        /// Время составления акта
        /// </summary>
        public virtual DateTime? DocumentTime { get; set; }
    }
}