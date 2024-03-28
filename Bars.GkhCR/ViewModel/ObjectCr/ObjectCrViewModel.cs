using Bars.Gkh.Domain;

namespace Bars.GkhCr.DomainService
{
    using System.Linq;

    using B4;
    using B4.Utils;

    using Enums;
    using Entities;
    using System.Collections.Generic;
    using Bars.Gkh.Domain.CollectionExtensions;

    public class ObjectCrViewModel : BaseViewModel<Entities.ObjectCr>
    {
        public override IDataResult List(IDomainService<Entities.ObjectCr> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            //workId
            var programId = baseParams.Params.GetAs("programId", string.Empty);
            var workId = baseParams.Params.GetAs("workId", string.Empty);
            var municipalityId = baseParams.Params.GetAs("municipalityId", string.Empty);
            var deleted = baseParams.Params.GetAs("deleted", false);
            var isbuildContr = baseParams.Params.GetAs("isbuildContr", false);
            var ids = baseParams.Params.GetAs("Id", string.Empty).ToLongArray();
            var programIds = !string.IsNullOrEmpty(programId) ? programId.Split(',').Select(x => x.ToLong()).ToArray() : new long[0];
            var workIds = !string.IsNullOrEmpty(workId) ? workId.Split(',').Select(x => x.ToLong()).ToArray() : new long[0];
            var municipalityIds = !string.IsNullOrEmpty(municipalityId) ? municipalityId.Split(',').Select(x => x.ToLong()).ToArray() : new long[0];
            var realityObjectId = baseParams.Params.GetAs("realityObjectId", 0L);
            List<long> objwithcontract = new List<long>();
            List<long> objwithselectedWork = new List<long>();

            if (isbuildContr)
            {
                var serviceCrObjBK = this.Container.Resolve<IDomainService<BuildContract>>();
                objwithcontract.AddRange(serviceCrObjBK.GetAll()
                    .WhereIf(programIds.Length > 0, x => programIds.Contains(x.ObjectCr.ProgramCr.Id))
                    .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                    .Select(x=> x.ObjectCr.Id).Distinct().ToList()
                    );
            }
            if (workIds.Count() > 0)
            {
                var serviceTypeWorkCr = this.Container.Resolve<IDomainService<TypeWorkCr>>();
                objwithselectedWork.AddRange(serviceTypeWorkCr.GetAll()
                     .WhereIf(programIds.Length > 0, x => programIds.Contains(x.ObjectCr.ProgramCr.Id))
                    .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                    .Where(x=> workIds.Contains(x.Work.Id))
                      .Select(x => x.ObjectCr.Id).Distinct().ToList()
                    );
            }

            var stateId = baseParams.Params.GetAs<long>("stateId", 0);

            var serviceProgramCr = this.Container.Resolve<IDomainService<ProgramCr>>();

            var data = this.Container.Resolve<IObjectCrService>().GetFilteredByOperator()
                .WhereIf(stateId > 0, x => x.State.Id == stateId)
                .WhereIf(objwithcontract.Count > 0, x => objwithcontract.Contains(x.Id))
                .WhereIf(objwithselectedWork.Count > 0, x => objwithselectedWork.Contains(x.Id))
                .WhereIf(realityObjectId > 0, x => x.RealityObjectId == realityObjectId)
                .WhereIf(deleted, x => !x.ProgramCrId.HasValue && x.BeforeDeleteProgramCrId.HasValue)
                .WhereIf(!deleted, x => x.ProgramCrId.HasValue && !x.BeforeDeleteProgramCrId.HasValue)
                .WhereIf(programIds.Length > 0 && deleted, x => programIds.Contains(x.BeforeDeleteProgramCrId.Value))
                .WhereIf(programIds.Length > 0 && !deleted, x => programIds.Contains(x.ProgramCrId.Value))
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.MunicipalityId) || municipalityIds.Contains(x.SettlementId))
                .WhereIf(ids.Length > 0, x => ids.Contains(x.Id))
                .WhereIf(deleted, y => serviceProgramCr.GetAll().Any(x =>
                        x.Id == y.BeforeDeleteProgramCrId
                        && x.TypeVisibilityProgramCr == TypeVisibilityProgramCr.Full))
                .WhereIf(!deleted, y => serviceProgramCr.GetAll().Any(x =>
                        x.Id == y.ProgramCrId
                        && x.TypeVisibilityProgramCr == TypeVisibilityProgramCr.Full))
                .Select(x => new
                    {
                        x.Id,
                        x.ProgramNum,
                        x.DateAcceptCrGji,
                        RealityObjName = x.Address,
                        x.Municipality,
                        x.Settlement,
                        x.DeadlineMissed,
                        x.Iscluttered,
                        x.Address,
                        x.State,
                        ProgramCrName = deleted ? x.BeforeDeleteProgramCrName : x.ProgramCrName,
                        AllowReneg = x.DateAcceptCrGji,
                        MonitoringSmrState = x.SmrState,
                        x.MonitoringSmrId,
                        RealityObject = x.RealityObjectId,
                        x.PeriodName,
                        x.MethodFormFund
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.RealityObjName)
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = loadParams.Order.Length == 0 ? data.Paging(loadParams) : data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<Entities.ObjectCr> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            var contractDomain = this.Container.Resolve<IDomainService<BuildContract>>();
            var actPaymentDomain = this.Container.Resolve<IDomainService<PerformedWorkActPayment>>();
            var actDomain = this.Container.Resolve<IDomainService<PerformedWorkAct>>();

            var workSum = contractDomain.GetAll().Where(y => y.ObjectCr.Id == id).Sum(y => y.Sum);
            var actSum = actDomain.GetAll().Where(y => y.ObjectCr.Id == id).Where(y => y.State.FinalState).SafeSum(y => y.SumTransfer.HasValue?y.SumTransfer.Value:0);

            var obj =
                domainService.GetAll()
                    .Where(x => x.Id == id)
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        x.ProgramCr,
                        x.ProgramNum,
                        x.DateEndBuilder,
                        x.DateStartWork,
                        x.AllowReneg,
                        x.BeforeDeleteProgramCr,
                        x.DateAcceptCrGji,
                        x.DateAcceptReg,
                        x.DateCancelReg,
                        x.DateEndWork,
                        x.DateStopWorkGji,
                        x.Description,
                        x.ExternalId,
                        FactAmountSpent = actSum,
                        x.FactEndDate,
                        x.RealityObject.Iscluttered,
                        x.FactStartDate,
                        x.FederalNumber,
                        x.GjiNum,
                            
                        //x.DeadlineMissed,
                        x.ObjectCreateDate,
                        x.ObjectEditDate,
                        x.ObjectVersion,
                        x.RealityObject,
                        x.SumDevolopmentPsd,
                        x.SumSmr,
                        x.SumSmrApproved,
                        x.SumTehInspection,
                        x.WarrantyEndDate,
                        MaxKpkrAmount = workSum

                    })
                    .FirstOrDefault();

            return new BaseDataResult(obj);
        }

    }
}