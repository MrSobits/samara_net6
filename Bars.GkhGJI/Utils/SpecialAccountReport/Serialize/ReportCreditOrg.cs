namespace Bars.GkhGji.Domain.SpecialAccountReport.Serialize
{
    using Bars.Gkh.Enums;

    public class ReportCreditOrg
    { 
        /// <summary>Наименование</summary>
        public virtual string Name { get; set; }

        /// <summary>ИНН</summary>
        public virtual string Inn { get; set; }

        /// <summary>КПП</summary>
        public virtual string Bik { get; set; }
    }
}