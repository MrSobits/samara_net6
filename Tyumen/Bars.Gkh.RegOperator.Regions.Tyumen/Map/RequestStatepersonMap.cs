using Bars.B4.Modules.Mapping.Mappers;
using Bars.Gkh.RegOperator.Regions.Tyumen.Entities;

namespace Bars.Gkh.RegOperator.Regions.Tyumen.Map
{
    public class RequestStatePersonMap : BaseEntityMap<RequestStatePerson>
    {
        public RequestStatePersonMap() : base("Запрос доступа для рекдактирования дома", "REGOP_REQUESTSTATE_PERSON")
        {
        }

        protected override void Map()
        {
            Property(x => x.Email, "Электронная почта").Column("EMAIL").Length(100).NotNull();
            Property(x => x.Name, "Имя пользователя, разрешающего доступ").Column("NAME").Length(200).NotNull();
            Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(300);
            Property(x => x.Position, "Должность").Column("POSITION").Length(300).NotNull();
            Property(x => x.Status, "Статус").Column("STATUS").NotNull();
        }
    }
}
