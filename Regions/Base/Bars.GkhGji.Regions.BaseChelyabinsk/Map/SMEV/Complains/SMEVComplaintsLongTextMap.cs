namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.Email;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Bars.GkhGji.Entities.EmailGjiLongText"</summary>
    public class SMEVComplaintsLongTextMap : BaseEntityMap<SMEVComplaintsLongText>
    {
        public SMEVComplaintsLongTextMap() :
                base("Bars.GkhGji.Entities.EmailGjiLongText", "SMEV_CH_COMPLAINTS_LTEXT")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.SMEVComplaints, "AppealCits").Column("COMPLAINT_ID").NotNull();
            this.Property(x => x.PauseResolutionPetition, "Содержание").Column("PAUSE_PETIT");
            this.Property(x => x.RenewTermPetition, "Содержание").Column("RENEW_PETIT");
        }
    }

    public class SMEVComplaintsLongTextNHibernateMapping : ClassMapping<SMEVComplaintsLongText>
    {
        public SMEVComplaintsLongTextNHibernateMapping()
        {
            this.Property(
                x => x.PauseResolutionPetition,
                mapper =>
                {
                    mapper.Type<BinaryBlobType>();
                });

            this.Property(
               x => x.RenewTermPetition,
               mapper =>
               {
                   mapper.Type<BinaryBlobType>();
               });
        }
    }
}
