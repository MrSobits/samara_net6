namespace Bars.B4.Modules.Analytics.Reports
{
    using System.Collections.Generic;
    using System.IO;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public abstract class ExportFormatHelper : BaseCodedReport
    {
        public IGkhUserManager UserManager { get; set; }
        public Operator thisOperator => UserManager.GetActiveOperator();
        public virtual OperatorExportFormat GetExtension()
        {
            return thisOperator.ExportFormat;
        }
    }
}
