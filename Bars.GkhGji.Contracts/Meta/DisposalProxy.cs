namespace Bars.GkhGji.Contracts.Meta
{
    using System;

    using Bars.B4.Utils;

    /// <summary>
    /// Приказ
    /// </summary>
    public class DisposalProxy : DocumentGjiProxy
    {
        [Display("Дата начала обследования")]
        public virtual DateTime? DateStart { get; set; }

        [Display("Дата окончания обследования")]
        public virtual DateTime? DateEnd { get; set; }
    }
}