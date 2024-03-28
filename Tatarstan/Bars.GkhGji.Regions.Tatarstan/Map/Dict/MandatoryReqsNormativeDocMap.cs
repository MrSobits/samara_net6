namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    /// <summary>
    /// Маппинг полей сущности <see cref="MandatoryReqsNormativeDoc"/>
    /// </summary>
    public class MandatoryReqsTypeNormativeDocMap : BaseEntityMap<MandatoryReqsNormativeDoc>
    {
        public MandatoryReqsTypeNormativeDocMap()
            : base("Bars.GkhGji.Regions.Tatarstan.Entities.Dict.MandatoryReqsNormativeDoc", "GJI_DICT_MANDATORY_REQS_NORMATIVE_DOC")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.MandatoryReqs, "MandatoryReqs").Column("MANDATORY_REQS_ID").NotNull();
            this.Reference(x => x.Npa, "Npa").Column("NPA_ID").NotNull();
        }
    }
}