using Bars.B4.Modules.Mapping.Mappers;
using Bars.Gkh.RegOperator.Regions.Tyumen.Entities;

namespace Bars.Gkh.RegOperator.Regions.Tyumen.Map
{
    public class RequestStateMap : BaseEntityMap<RequestState>
    {
        public RequestStateMap() : base("Запрос доступа для рекдактирования дома", "REGOP_REQUESTSTATE")
        {
        }

        protected override void Map()
        {
            Property(x => x.UserId, "Id пользователя, запрашивающего доступ").Column("USERID").NotNull();
            Property(x => x.UserName, "Имя пользователя, запрашивающего доступ").Column("USERNAME").Length(200).NotNull();
            Property(x => x.Description, "Description").Column("DESCRIPTION").Length(300);
            Property(x => x.NotifiedUser, "NotifiedUser").Column("NOTIFIED_USER");
            Property(x => x.NotifiedPerson, "NotifiedPerson").Column("NOTIFIED_RUSTAMCHIK");
            Reference(x => x.RealityObject, "Дом, для которого запрашивается доступ").Column("RO_ID").NotNull().Fetch();
            Reference(x => x.File, "Основание").Column("FILE_ID").Fetch();
        }
    }
}
