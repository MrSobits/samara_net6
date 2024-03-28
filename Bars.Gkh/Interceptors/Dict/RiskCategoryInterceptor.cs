namespace Bars.Gkh.Interceptors.Dict
{
    using Bars.Gkh.Entities.Dicts;

    /// <inheritdoc />
    public class RiskCategoryInterceptor : BaseGkhDictInterceptor<RiskCategory>
    {
        protected override string EntityName => "Категория риска";
    }
}