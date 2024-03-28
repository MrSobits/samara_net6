namespace Bars.GkhGji.Regions.Tatarstan.Map.TatarstanControlList
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanControlList;

    public class TatarstanControlListMap : BaseEntityMap<TatarstanControlList>
    {
        /// <inheritdoc />
        public TatarstanControlListMap()
            : base("Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanControlList", "GJI_CONTROL_LIST")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.TorId, "TorId").Column("TOR_ID");
            this.Property(x => x.Name, "Name").Column("NAME").Length(512);
            this.Property(x => x.ApprovalDetails, "ApprovalDetails").Column("APPROVAL_DETAILS").Length(2048);
            this.Property(x => x.StartDate, "StartDate").Column("START_DATE").NotNull();
            this.Property(x => x.EndDate, "EndDate").Column("END_DATE");
            this.Reference(x => x.File, "File").Column("FILE_ID").Fetch();
            this.Reference(x => x.Disposal, "Disposal").Column("DISPOSAL_ID").Fetch();
            this.Property(x => x.ErpGuid, "ErpGuid").Column("ERP_GUID");
        }
    }
}
