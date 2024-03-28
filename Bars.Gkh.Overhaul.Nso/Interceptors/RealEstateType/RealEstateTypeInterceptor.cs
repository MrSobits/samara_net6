namespace Bars.Gkh.Overhaul.Nso.Interceptors
{
    using System;
    using B4;
    using B4.DataAccess;

    using System.Linq;

    using Bars.Gkh.Overhaul.Nso.ConfigSections;

    using Gkh.Entities.Dicts;
    using Gkh.Entities.RealEstateType;
    using Gkh.Utils;

    public class RealEstateTypeInterceptor : EmptyDomainInterceptor<RealEstateType>
    {
        public IDomainService<RealEstateTypePriorityParam> PriorParamService { get; set; }

        public IDomainService<RealEstateTypeCommonParam> CommonParamService { get; set; }

        public IDomainService<RealEstateTypeStructElement> StructElemService { get; set; }

        public IDomainService<RealEstateTypeRate> RateService { get; set; }

        public IDomainService<RealEstateTypeRealityObject> realEstRealObjService { get; set; }

        public override IDataResult BeforeDeleteAction(IDomainService<RealEstateType> service, RealEstateType entity)
        {
            using (var transaction = BeginTransaction())
            {
                try
                {
                    var commonParams = CommonParamService.GetAll().Where(x => x.RealEstateType.Id == entity.Id).Select(x => x.Id);
                    var structEls = StructElemService.GetAll().Where(x => x.RealEstateType.Id == entity.Id).Select(x => x.Id);
                    var priorParams = PriorParamService.GetAll().Where(x => x.RealEstateType.Id == entity.Id).Select(x => x.Id);
                    var rates = RateService.GetAll().Where(x => x.RealEstateType.Id == entity.Id).Select(x => x.Id);
                    var realEstRealObjs = realEstRealObjService.GetAll().Where(x => x.RealEstateType.Id == entity.Id).Select(x => x.Id);

                    foreach(var id in commonParams)
                    {
                        CommonParamService.Delete(id);
                    }
                    foreach(var id in structEls)
                    {
                        StructElemService.Delete(id);
                    }
                    foreach (var id in priorParams)
                    {
                        PriorParamService.Delete(id);
                    }
                    foreach (var id in rates)
                    {
                        RateService.Delete(id);
                    }
                    foreach (var id in realEstRealObjs)
                    {
                        realEstRealObjService.Delete(id);
                    }
                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(
                            string.Format(
                                "Произошла не известная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                e.Message,
                                e.StackTrace),
                            exc);
                    }

                    throw;
                }
                return base.BeforeDeleteAction(service, entity);
            }
        }

        public override IDataResult AfterCreateAction(IDomainService<RealEstateType> service, RealEstateType entity)
        {
            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var periodStart = config.ProgrammPeriodStart;
            var periodShort = config.ShortTermProgPeriod;

            if (periodShort > 0 && periodShort <= 5)
            {
                for (int i = periodStart; i < periodStart + periodShort; i++)
                {
                    RateService.Save(new RealEstateTypeRate {RealEstateType = entity, Year = i});
                }
            }

            return base.AfterCreateAction(service, entity);
        }


        private IDataTransaction BeginTransaction()
        {
            return Container.Resolve<IDataTransaction>();
        }
    }
}
