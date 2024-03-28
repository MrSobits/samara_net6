namespace Bars.GkhDi.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    using Castle.Windsor;

    public class FinActivityRealityObjCommunalServService : IFinActivityRealityObjCommunalServService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddWorkMode(BaseParams baseParams)
        {
            var service = this.Container.ResolveDomain<FinActivityRealityObjCommunalService>();
            var disclosureInfoRealObjDomain = this.Container.ResolveDomain<DisclosureInfoRealityObj>();

            try
            {
                var disclosureInfoRealityObjId = baseParams.Params.GetAs<long>("disclosureInfoRealityObjId");
                var records = baseParams.Params["records"]
                    .As<List<object>>()
                    .Select(x => x.As<DynamicDictionary>().ReadClass<FinActivityRealityObjCommunalService>())
                    .ToList();

                var existingFinActivityRealityObjCommunalService = service.GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObjId)
                    .AsEnumerable()
                    .GroupBy(x => x.TypeServiceDi)
                    .ToDictionary(x => x.Key, y => y.FirstOrDefault());

                var disclosureInfoRealObj = disclosureInfoRealObjDomain.GetAll().FirstOrDefault(x => x.Id == disclosureInfoRealityObjId);

                foreach (var rec in records)
                {
                    FinActivityRealityObjCommunalService existingCommunalService = null;

                    if (existingFinActivityRealityObjCommunalService.ContainsKey(rec.TypeServiceDi))
                    {
                        existingCommunalService = existingFinActivityRealityObjCommunalService[rec.TypeServiceDi];
                    }

                    if (existingCommunalService != null)
                    {
                        existingCommunalService.PaidOwner = rec.PaidOwner;
                        existingCommunalService.DebtOwner = rec.DebtOwner;
                        existingCommunalService.PaidByIndicator = rec.PaidByIndicator;
                        existingCommunalService.PaidByAccount = rec.PaidByAccount;

                        service.Update(existingCommunalService);
                    }
                    else
                    {
                        var newFinActivityCommunalService = new FinActivityRealityObjCommunalService
                        {
                            DisclosureInfoRealityObj = disclosureInfoRealObj,
                            TypeServiceDi = rec.TypeServiceDi,
                            PaidOwner = rec.PaidOwner,
                            DebtOwner = rec.DebtOwner,
                            PaidByIndicator = rec.PaidByIndicator,
                            PaidByAccount = rec.PaidByAccount
                        };

                        service.Save(newFinActivityCommunalService);
                    }
                }

                return new BaseDataResult {Success = true};
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult {Success = false, Message = exc.Message};
            }
            finally
            {
                this.Container.Release(service);
                this.Container.Release(disclosureInfoRealObjDomain);
            }
        }

        public IDataResult AddDataByRealityObj(BaseParams baseParams)
        {
            var disclosureInfoDomain = this.Container.ResolveDomain<DisclosureInfo>();
            var disclosureInfoRelationDomain = this.Container.ResolveDomain<DisclosureInfoRelation>();
            var finActivityRealityObjCommunalServiceDomain = this.Container.ResolveDomain<FinActivityRealityObjCommunalService>();
            var finActivityCommunalServiceDomain = this.Container.ResolveDomain<FinActivityCommunalService>();

            try
            {
                var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");

                var disclosureInfo = disclosureInfoDomain.Load(disclosureInfoId);

                var disclosureInfoRealityObjIdList = disclosureInfoRelationDomain
                    .GetAll()
                    .Where(x => x.DisclosureInfo.Id == disclosureInfoId)
                    .Select(x => x.DisclosureInfoRealityObj.Id)
                    .ToList();

                var finActivityRealityObjCommunalServiceList = finActivityRealityObjCommunalServiceDomain
                    .GetAll()
                    .Where(x => disclosureInfoRealityObjIdList.Contains(x.DisclosureInfoRealityObj.Id))
                    .ToList();

                var finActivityByType = finActivityCommunalServiceDomain.GetAll()
                    .Where(x => x.DisclosureInfo.Id == disclosureInfoId)
                    .GroupBy(x => x.TypeServiceDi)
                    .ToDictionary(x => x.Key, x => new FinActivityCommunalServiceContext
                    {
                        FinActivityCommunalService = x.First()
                    });
                
                foreach (var finActivityRealityObjCommunalService in finActivityRealityObjCommunalServiceList)
                {
                    var finActivityContext = finActivityByType.Get(finActivityRealityObjCommunalService.TypeServiceDi);
                    if (finActivityContext == null)
                    {
                        finActivityContext = new FinActivityCommunalServiceContext
                        {
                            FinActivityCommunalService = new FinActivityCommunalService
                            {
                                Id = 0,
                                TypeServiceDi = finActivityRealityObjCommunalService.TypeServiceDi,
                                DisclosureInfo = disclosureInfo
                            }
                        };

                        finActivityByType[finActivityRealityObjCommunalService.TypeServiceDi] = finActivityContext;
                    }

                    finActivityContext.Process(finActivityRealityObjCommunalService);
                }

                var finActivityCommunalServiceForSave = finActivityByType.Select(x => x.Value)
                    .Where(x => x.IsEdited.Any())
                    .Select(x => x.FinActivityCommunalService)
                    .ToList();

                foreach (var finActivityCommunalService in finActivityCommunalServiceForSave)
                {
                    if (finActivityCommunalService.Exact.HasValue || finActivityCommunalService.DebtPopulationEnd.HasValue
                        || finActivityCommunalService.PaidByMeteringDevice.HasValue || finActivityCommunalService.PaidByGeneralNeeds.HasValue)
                    {
                        if (finActivityCommunalService.Id == 0)
                        {
                            finActivityCommunalServiceDomain.Save(finActivityCommunalService);
                        }
                        else
                        {
                            finActivityCommunalServiceDomain.Update(finActivityCommunalService);
                        }
                    }
                }

                return new BaseDataResult {Success = true};
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult {Success = false, Message = exc.Message};
            }
            finally
            {
                this.Container.Release(disclosureInfoDomain);
                this.Container.Release(disclosureInfoRelationDomain);
                this.Container.Release(finActivityRealityObjCommunalServiceDomain);
                this.Container.Release(finActivityCommunalServiceDomain);
            }
        }
        
        private class FinActivityCommunalServiceContext
        {
            public FinActivityCommunalServiceContext()
            {
                this.IsEdited = new bool[] { false, false, false, false };
            }

            public FinActivityCommunalService FinActivityCommunalService { get; set; }

            public bool[] IsEdited { get; set; }

            public void Process(FinActivityRealityObjCommunalService finActivityRealityObjCommunalService)
            {
                this.ProcessInner(
                    finActivityRealityObjCommunalService,
                    source => source.PaidOwner,
                    target => target.Exact = 0m,
                    (target, amount) => target.Exact += amount,
                    0);
                this.ProcessInner(
                    finActivityRealityObjCommunalService,
                    source => source.DebtOwner,
                    target => target.DebtPopulationEnd = 0m,
                    (target, amount) => target.DebtPopulationEnd += amount,
                    1);
                this.ProcessInner(
                    finActivityRealityObjCommunalService,
                    source => source.PaidByIndicator,
                    target => target.PaidByMeteringDevice = 0m,
                    (target, amount) => target.PaidByMeteringDevice += amount,
                    2);
                this.ProcessInner(
                    finActivityRealityObjCommunalService,
                    source => source.PaidByAccount,
                    target => target.PaidByGeneralNeeds = 0m,
                    (target, amount) => target.PaidByGeneralNeeds += amount,
                    3);
            }

            private void ProcessInner(
                FinActivityRealityObjCommunalService finActivityRealityObjCommunalService,
                Func<FinActivityRealityObjCommunalService, decimal?> sourceGetter,
                Action<FinActivityCommunalService> targetZeroSetter,
                Action<FinActivityCommunalService, decimal?> targerAppender,
                int index)
            {
                var source = sourceGetter(finActivityRealityObjCommunalService);
                if (source.HasValue)
                {
                    if (!this.IsEdited[index])
                    {
                        targetZeroSetter(this.FinActivityCommunalService);
                        this.IsEdited[index] = true;
                    }

                    targerAppender(this.FinActivityCommunalService, source.Value);
                }
            }
        }
    }
}