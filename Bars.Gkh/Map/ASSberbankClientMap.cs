using Bars.B4.Modules.Mapping.Mappers;
using Bars.Gkh.Entities;

namespace Bars.Gkh.Map
{
    public class ASSberbankClientMap : BaseEntityMap<ASSberbankClient>
    {

        public ASSberbankClientMap() :
                base("Настройки для выгрузки в Клиент-СБербанк", "GKH_RO_ASSBERBANKCLIENT")
        {
        }

        protected override void Map()
        {
            Property(x => x.ClientCode, "Код клиента в АС Клиент - Сбербанк").Column("CLIENT_CODE").NotNull();
        }
    }
}
