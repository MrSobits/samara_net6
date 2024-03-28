using Bars.B4.Modules.Mapping.Mappers;
using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    /// <summary>
    /// Маппинг полей сущности <see cref="MandatoryReqsControlType"/>
    /// </summary>
    public class MandatoryReqsControlTypeMap : BaseEntityMap<MandatoryReqsControlType>
    {
        public MandatoryReqsControlTypeMap()
            : base("Bars.GkhGji.Regions.Tatarstan.Entities.Dict.MandatoryReqsControlType", "GJI_DICT_MANDATORY_REQS_CONTOROL_TYPE")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.MandatoryReqs, "MandatoryReqs").Column("MANDATORY_REQS_ID").NotNull();
            this.Reference(x => x.ControlType, "ControlType").Column("CONTORL_TYPE_ID").NotNull();
        }
    }
}
