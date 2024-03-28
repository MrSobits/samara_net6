namespace Bars.B4.Modules.Analytics.Reports.Map;

using Bars.B4.DataAccess.ByCode;
using Bars.B4.Modules.Analytics.Reports.Entities;

/// <summary>Маппинг для <see cref="PrintFormCategory"/></summary>
/// <remarks> Взято из пакета Bars.B4.Modules.ReportPanel версии 3.2.0.0</remarks>
public class PrintFormCategoryMap : BaseEntityMap<PrintFormCategory>
{
    public PrintFormCategoryMap()
        : base("B4_PRINT_FORM_CATEGORY")
    {
        this.Map<string>(x => x.Name, "NAME", true, 100);
    }
}