using Bars.B4.Modules.Mapping.Mappers;
using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmCharacters;

namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict.KnmCharacters
{
    public class KnmCharacterMap : BaseEntityMap<KnmCharacter>
    {
        public KnmCharacterMap()
            : base("Характер КНМ", "GJI_DICT_KNM_CHARACTER")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ErknmCode, "Код в ЕРКНМ").Column("ERKNM_CODE");
        }
    }
}
