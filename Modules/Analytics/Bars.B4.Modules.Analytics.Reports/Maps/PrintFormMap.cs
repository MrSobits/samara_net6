namespace Bars.B4.Modules.Analytics.Reports.Map;

using Bars.B4.DataAccess.ByCode;
using Bars.B4.Modules.Analytics.Reports.Entities;

/// <summary>
/// Маппинг для <see cref="PrintForm"/>
/// </summary>
/// <remarks> Взято из пакета Bars.B4.Modules.ReportPanel версии 3.2.0.0</remarks>
public class PrintFormMap : BaseEntityMap<PrintForm>
{
    public PrintFormMap()
        : base("B4_PRINT_FORM")
    {
        this.Map(x => x.Name, "NAME", false, 100);
        this.Map(x => x.Description, "DESCRIPTION");
        this.Map(x => x.ClassName, "CLASSNAME", true, 250);
        this.References(x => x.Category, "CATEGORY", ReferenceMapConfig.NotNullAndFetch);
    }
}