namespace Bars.Gkh.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.Regions.Voronezh.Entities;

    /// <summary>Маппинг для "Исковое зявление"</summary>
    public class LawsuitOwnerRepresentativeMap : BaseEntityMap<LawsuitOwnerRepresentative>
    {

        public LawsuitOwnerRepresentativeMap() :
                base("Законные представители несовершеннолетнего", "CLW_LAWSUIT_OWNER_REPRESENTATIVE")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.Rloi, "Rloi").Column("Rloi").Fetch().NotNull();
            this.Property(x => x.RepresentativeType, "RepresentativeType").Column("RepresentativeType");
            this.Property(x => x.Surname, "Surname").Column("Surname");
            this.Property(x => x.FirstName, "FirstName").Column("FirstName");
            this.Property(x => x.Patronymic, "Patronymic").Column("Patronymic");
            this.Property(x => x.BirthDate, "BirthDate").Column("BirthDate");
            this.Property(x => x.BirthPlace, "BirthPlace").Column("BirthPlace");
            this.Property(x => x.LivePlace, "LivePlace").Column("LivePlace");
            this.Property(x => x.Note, "Note").Column("Note");
        }
    }
}
