namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;

    /// <summary>Маппинг для "Исполнитель заявки"</summary>
    public class MKDLicRequestExecutantMap : BaseEntityMap<MKDLicRequestExecutant>
    {

        public MKDLicRequestExecutantMap() :
                base("Исполнитель заявки", "GJI_MKD_LIC_REQUEST_EXECUTANT")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.OrderDate, "OrderDate").Column("ORDER_DATE");
            this.Property(x => x.PerformanceDate, "PerformanceDate").Column("PERFOM_DATE");
            this.Property(x => x.IsResponsible, "IsResponsible").Column("RESPONSIBLE");
            this.Property(x => x.OnApproval, "OnApproval").Column("ONAPPROVAL");
            this.Property(x => x.Description, "Description").Column("DESCRIPTION").Length(255);
            this.Reference(x => x.MKDLicRequest, "MKDLicRequest").Column("MKD_LIC_REQUEST_ID").NotNull().Fetch();
            this.Reference(x => x.Executant, "Executant").Column("EXECUTANT_ID").Fetch();
            this.Reference(x => x.Author, "Author").Column("AUTHOR_ID").Fetch();
            this.Reference(x => x.Controller, "Controller").Column("CONTROLLER_ID").Fetch();
            this.Reference(x => x.State, "State").Column("STATE_ID").Fetch();
            this.Reference(x => x.Resolution, "Resolution").Column("RESOLUTION_ID").Fetch();
            this.Reference(x => x.ZonalInspection, "ZonalInspection").Column("ZONAINSP_ID").Fetch();
        }
    }
}