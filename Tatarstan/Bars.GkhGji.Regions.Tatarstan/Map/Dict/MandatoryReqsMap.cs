using Bars.B4.Modules.Mapping.Mappers;
using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict
{
    public class MandatoryReqsMap : BaseEntityMap<MandatoryReqs>
    {
        public MandatoryReqsMap() :
            base("Bars.GkhGji.Regions.Tatarstan.Entities.Dict.MandatoryReqs", "GJI_DICT_MANDATORY_REQS")
        {

        }

        protected override void Map()
        {
            this.Property(x => x.MandratoryReqName, "MandratoryReqName").Column("mandratory_req_name").Length(500).NotNull();
            this.Property(x => x.MandratoryReqContent, "MandratoryReqContent").Column("mandratory_req_content").Length(600).NotNull();
            this.Property(x => x.StartDateMandatory, "StartDateMandatory").Column("start_date_mandator").NotNull();
            this.Property(x => x.EndDateMandatory, "EndDateMandatory").Column("end_date_mandatory").NotNull();
            this.Property(x => x.TorId, "TorId").Column("tor_id");
        }
    }
}
