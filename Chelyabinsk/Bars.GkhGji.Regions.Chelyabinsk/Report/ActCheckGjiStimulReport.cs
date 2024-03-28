namespace Bars.GkhGji.Regions.Chelyabinsk.Report
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Properties;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActCheck;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.DocumentGji;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Properties;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Report.ActCheck;
    using Bars.GkhGji.Report;


    /// <summary>
	/// Отчет акта проверки
	/// </summary>
	public class ChelyabinskActCheckGjiStimulReport : ActCheckGjiStimulReport
    {

        #region Public Properties
        /// <summary>
        /// Формат экспорта
        /// </summary>
        public override StiExportFormat ExportFormat
        {
            get
            {
                return StiExportFormat.Odt;
            }
        }

        /// <summary>
        /// Настройки экспорта
        /// </summary>
        public override Dictionary<string, string> ExportSettings
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { "RemoveEmptySpaceAtBottom", "false" }
                };
            }
        }

        #endregion
    }
}