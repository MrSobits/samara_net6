using Bars.B4.Modules.Mapping.Mappers;
using Bars.GkhGji.Entities.Dict;

namespace Bars.GkhGji.Map.Dict
{
    public class CitizenshipMap : BaseEntityMap<Citizenship>
    {
        public CitizenshipMap()
            : base("Справочник гражданств согласно ОКСМ", "GJI_DICT_CITIZENSHIP")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование").Column("NAME");
            this.Property(x => x.FullName, "Полное наименование").Column("FULL_NAME");
            this.Property(x => x.Oksm2, "Код ОКСМ (2-букв.)").Column("OKSM_2").Length(2);
            this.Property(x => x.Oksm3, "Код ОКСМ (3-букв.)").Column("OKSM_3").Length(3);
            this.Property(x => x.OksmCode, "Код ОКСМ (цифр.)").Column("OKSM_CODE");
        }
    }
}
