namespace Bars.B4.Modules.Analytics.Reports;

using Bars.B4.Modules.Analytics.Data;
using Bars.B4.Modules.Analytics.Filters;

public class CustomSingleDataProvider<T> : BaseSingleDataProvider<T>
    where T : class, new()
{
    /// <inheritdoc />
    public override T GetSingleData(SystemFilter systemFilter, DataFilter dataFilter, BaseParams baseParams) => this.data;

    /// <inheritdoc />
    public override string Name { get; }

    private readonly T data;

    public CustomSingleDataProvider(string name, T data)
    {
        this.Name = name;
        this.data = data;
    }
}