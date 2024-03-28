namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Castle.Windsor;
    using Entities;
    using Enums;
    using Gkh.Domain;
    using Gkh.Entities;

    public class CalcAccountOverdraftService : ICalcAccountOverdraftService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<CalcAccountOverdraft> Domain { get; set; }

        public IEnumerable<CalcAccountOverdraft> GetRobjectOverdrafts(RealityObject ro)
        {
            var accroDomain = Container.ResolveDomain<CalcAccountRealityObject>();

            var filterQuery = accroDomain.GetAll()
                .Where(x => x.Account.TypeAccount != TypeCalcAccount.Special || ((SpecialCalcAccount) x.Account).IsActive)
                .Where(x => x.RealityObject.Id == ro.Id);

            return Domain.GetAll()
                .Where(y => filterQuery.Any(x => x.Account == y.Account))
                .ToList();
        }

        public decimal GetRobjectOverdraft(BaseParams baseParams)
        {
            var roId = baseParams.Params.GetAsId("roId");
            return GetLastOverdraft(new RealityObject { Id = roId }).Return(x => x.AvailableSum);
        }

        public CalcAccountOverdraft GetLastOverdraft(RealityObject robject)
        {
            return GetRobjectOverdrafts(robject).OrderByDescending(x => x.DateStart).FirstOrDefault();
        }

        public CalcAccountOverdraft GetLastOverdraft(CalcAccount account)
        {
            return Domain.GetAll()
                .Where(x => x.Account.Id == account.Id)
                .OrderByDescending(x => x.DateStart)
                .FirstOrDefault();
        }

        public void UpdateAccountOverdraft(CalcAccount account, decimal availableSum)
        {
            var overdraft = GetLastOverdraft(account);

            UpdateAccountOverdraft(overdraft, availableSum);
        }

        public void UpdateAccountOverdraft(CalcAccountOverdraft overdraft, decimal availableSum)
        {
            if (overdraft != null)
            {
                overdraft.AvailableSum = availableSum;
                Domain.Update(overdraft);
            }
        }
    }
}