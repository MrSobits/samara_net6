using Bars.Gkh.Interceptors.Dict;
using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.Dict
{
    /// <inheritdoc />
    public class TatRiskCategoryInterceptor : BaseGkhDictInterceptor<TatRiskCategory>
    {
        protected override string EntityName => "Категория риска";
    }
}
