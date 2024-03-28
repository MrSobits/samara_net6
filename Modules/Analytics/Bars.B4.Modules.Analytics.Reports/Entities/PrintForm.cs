namespace Bars.B4.Modules.Analytics.Reports.Entities;

using Bars.B4.DataAccess;
using Bars.B4.Utils;

/// <summary>
/// Печатная форма
/// </summary>
/// <remarks> Взято из пакета Bars.B4.Modules.ReportPanel версии 3.2.0.0</remarks>
[Display("Печатная форма")]
public class PrintForm : BaseEntity
{
    [Display("Наименование")]
    public virtual string Name { get; set; }

    [Display("Категория формы печати")]
    public virtual PrintFormCategory Category { get; set; }

    [Display("Описание")]
    public virtual string Description { get; set; }

    [Display("Имя класса отчета")]
    public virtual string ClassName { get; set; }
}