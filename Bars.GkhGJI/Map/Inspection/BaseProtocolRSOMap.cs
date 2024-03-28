namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;


    /// <summary>Маппинг для "Основание протокола МЖК"</summary>
    public class BaseProtocolRSOMap : JoinedSubClassMap<BaseProtocolRSO>
    {

        public BaseProtocolRSOMap() :
                base("Основание протокола РСО", "GJI_INSPECTION_PROTRSO")
        {
        }

        protected override void Map()
        {
        }
    }
}
