namespace Bars.GisIntegration.Base.Map.Bills
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GisIntegration.Base.Entities.Bills;

    /// <summary>
    /// Маппинг для "Bars.GisIntegration.Base.Entities.InsuranceProduct"
    /// </summary>
    public class InsuranceProductMap : BaseEntityMap<InsuranceProduct>
    {
        public InsuranceProductMap() :
            base("Bars.GisIntegration.Base.Entities.InsuranceProduct", "INSURANCE_PRODUCT")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Name, "Name").Column("NAME");
            this.Reference(x => x.Attachment, "Attachment").Column("ATTACHMENT").NotNull().Fetch();
            this.Property(x => x.Description, "Description").Column("DECRIPTION");
            this.Property(x => x.AttachmentHash, "AttachmentHash").Column("ATTACHMENT_HASH");
            this.Property(x => x.InsuranceOrganization, "InsuranceOrganization").Column("INSURANCE_ORGANIZATION");
            this.Property(x => x.CloseDate, "CloseDate").Column("CLOSE_DATE").NotNull();
        }
    }
}