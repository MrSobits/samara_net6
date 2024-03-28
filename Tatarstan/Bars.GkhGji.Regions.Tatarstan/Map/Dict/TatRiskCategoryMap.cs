using Bars.B4.Modules.Mapping.Mappers;
using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict
{
    public class TatRiskCategoryMap : JoinedSubClassMap<TatRiskCategory>
    {
        public TatRiskCategoryMap()
            : base("Категория риска", "GJI_DICT_TAT_RISK_CATEGORY")
        {
        }

        protected override void Map()
        {
            Property(x => x.ErvkGuid, "Идентификатор в ЕРВК").Column("ERVK_GUID").Length(36);
        }
    }
}
