namespace Bars.Gkh.RegOperator.Interceptors
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Entities;
    using Gkh.Entities;

    /// <summary>
    /// 
    /// </summary>
    public class EntranceInterceptor : EmptyDomainInterceptor<Entrance>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<Entrance> service, Entrance entity)
        {
            var chargeDomain = Container.ResolveDomain<PersonalAccountCharge>();

            if (chargeDomain.GetAll()
                .Where(x => x.ObjectCreateDate >= entity.ObjectCreateDate)
                .Any(x => x.BasePersonalAccount.Room.Entrance.Id == entity.Id))
            {
                return Failure("По лицевым счетам этого подъезда уже были проведены начисления. Удаление записи невозможно.");
            }

            return Success();
        }
    }
}