namespace Bars.Gkh.Map.Dict
{
    using Bars.Gkh.Entities.Dicts;

    /// <inheritdoc />
    public class RiskCategoryMap : BaseGkhDictMap<RiskCategory>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public RiskCategoryMap()
            : base("Категория риска", "GKH_DICT_RISK_CATEGORY")
        {

        }
    }
}