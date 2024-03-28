namespace Bars.B4.Modules.Analytics.Reports.Entities;

using Bars.B4.DataAccess;
using Bars.B4.Utils;

/// <summary>
/// Категория формы печати
/// </summary>
/// <remarks> Взято из пакета Bars.B4.Modules.ReportPanel версии 3.2.0.0</remarks>
[Display("Категория формы печати")]
public class PrintFormCategory : BaseEntity
{
    [Display("Наименование")]
    public virtual string Name { get; set; }
}