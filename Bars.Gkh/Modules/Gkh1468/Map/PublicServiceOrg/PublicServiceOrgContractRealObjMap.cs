namespace Bars.Gkh.Modules.Gkh1468.Map
{
    using Bars.Gkh.Map;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    /// <summary>
    /// Маппинг
    /// </summary>
    public class PublicServiceOrgContractRealObjMap : BaseImportableEntityMap<PublicServiceOrgContractRealObj>
    {
        /// <summary>
        /// ctor
        /// </summary>
        public PublicServiceOrgContractRealObjMap()
            : base("Bars.Gkh.Modules.Gkh1468.Entities.PublicServiceOrgContractRealObj", "GKH_PUB_SERV_ORG_CONTR_REAL_OBJ")
        {
        }

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.RsoContract, "Договор РСО с Домами").Column("RSO_CONTR_ID").NotNull().Fetch();
            this.Reference(x => x.RealityObject, "Жилой дом").Column("RO_ID").NotNull().Fetch();
        }
    }
}