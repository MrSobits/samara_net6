namespace Bars.Gkh.RegOperator.Interceptors
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Entities;
    using Enums;

    public class SubsidyIncomeDetailInterceptor : EmptyDomainInterceptor<SubsidyIncomeDetail>
    {
        public override IDataResult AfterUpdateAction(IDomainService<SubsidyIncomeDetail> service, SubsidyIncomeDetail entity)
        {
            var subsidyIncomeDomain = Container.ResolveDomain<SubsidyIncome>();

            try
            {
                var definedInfoRealObj = service.GetAll()
                    .Where(x => x.SubsidyIncome.Id == entity.SubsidyIncome.Id)
                    .Select(x => x.RealityObject != null)
                    .ToArray();

                entity.SubsidyIncome.SubsidyIncomeDefineType = definedInfoRealObj.Any(x => x)
                    ? definedInfoRealObj.All(x => x)
                        ? SubsidyIncomeDefineType.Defined
                        : SubsidyIncomeDefineType.PartiallyDefined
                    : SubsidyIncomeDefineType.NotDefined;

                subsidyIncomeDomain.Save(entity.SubsidyIncome);

                return Success();

            }
            finally
            {
                Container.Release(subsidyIncomeDomain);
            }
        }
    }
}