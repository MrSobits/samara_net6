namespace Bars.B4.Modules.FIAS.Map
{
    using Bars.B4.DataAccess.ByCode;

    public class FiasHouseMap : PersistentObjectMap<FiasHouse>
    {
        public FiasHouseMap(): base("B4_FIAS_HOUSE")
        {
            this.Map(x => x.HouseId, "HOUSE_ID", false);
            this.Map(x => x.HouseGuid, "HOUSE_GUID", false);
            this.Map(x => x.AoGuid, "AO_GUID", true);
            this.Map(x => x.PostalCode, "POSTAL_CODE", false, 6);
            this.Map(x => x.Okato, "OKATO", false, 11);
            this.Map(x => x.Oktmo, "OKTMO", false, 11);
            this.Map(x => x.HouseNum, "HOUSE_NUM", false, 20);
            this.Map(x => x.BuildNum, "BUILD_NUM", false, 10);
            this.Map(x => x.StrucNum, "STRUC_NUM", false, 10);
            this.Map(x => x.ActualStatus, "STAT_STATUS", false);
            this.Map(x => x.UpdateDate, "UPDATE_DATE");
            this.Map(x => x.TypeRecord, "TYPE_RECORD", true);
            this.Map(x => x.StartDate, "START_DATE");
            this.Map(x => x.EndDate, "END_DATE");
            this.Map(x => x.StructureType, "STRUCTURE_TYPE", true);
            this.Map(x => x.EstimateStatus, "EST_STATUS", true, Enums.FiasEstimateStatusEnum.House);
        }
    }
}