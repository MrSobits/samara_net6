using Bars.B4.Modules.Mapping.Mappers;
using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmCharacters;

namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict.KnmCharacters
{
    public class KnmCharacterKindCheckMap : BaseEntityMap<KnmCharacterKindCheck>
    {
        public KnmCharacterKindCheckMap()
            : base("Связь между характером КНМ и видом проверки", "KNM_CHARACTER_KIND_CHECK")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.KnmCharacter, "Характер КНМ").Column("KNM_CHARACTER_ID").Fetch();
            Reference(x => x.KindCheckGji, "Вид проверки").Column("KIND_CHECK_ID").Fetch();
        }
    }
}
