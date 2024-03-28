namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    public class OrganMvdMap : BaseEntityMap<OrganMvd>
    {
        public OrganMvdMap()
            : base("Справочник Органы МВД", "GJI_ORGAN_MVD")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование").Column("NAME");
            this.Property(x => x.Code, "Код").Column("COD");
          //  this.Reference(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY_ID");
        }
    }
}