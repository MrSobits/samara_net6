namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.AppealCits
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;

    public class AppealOrderViewModel : BaseViewModel<AppealOrder>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<OperatorContragent> OperatorContragentDomain { get; set; }

        public override IDataResult List(IDomainService<AppealOrder> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var dateStart2 = baseParams.Params.GetAs("dateStart", new DateTime());
            var dateEnd2 = baseParams.Params.GetAs("dateEnd", new DateTime());
            var showCloseAppeals = baseParams.Params.GetAs("showCloseAppeals", true);
            Operator thisOperator = UserManager.GetActiveOperator();
            if (thisOperator?.Inspector == null)
            {
                var contragent = thisOperator.Contragent;

                var contragentList = OperatorContragentDomain.GetAll()
                    .Where(x => x.Contragent != null)
                    .Where(x => x.Operator == thisOperator)
                    .Select(x => x.Contragent.Id).Distinct().ToList();
                if (contragent != null)
                {
                    if (!contragentList.Contains(contragent.Id))
                    {
                        contragentList.Add(contragent.Id);
                    }
                }
                var data = domainService.GetAll()
                    .Where(x=> x.OrderDate >= dateStart2 && x.OrderDate<=dateEnd2)
                    .WhereIf(!showCloseAppeals, x=> x.YesNoNotSet == Gkh.Enums.YesNoNotSet.No)
                                        .Where(x => contragentList.Contains(x.Executant.Id))
                                        .Select(x => new
                                        {
                                            x.Id,
                                            Executant = x.Executant.Name,
                                            ContragentINN = x.Executant.Inn,
                                            x.Person,
                                            x.AppealCits.State,
                                            x.AppealCits.DocumentNumber,
                                            x.AppealCits.DateFrom,
                                            x.OrderDate,
                                            x.PerformanceDate,
                                            x.YesNoNotSet,
                                            x.Confirmed,
                                            x.AppealCits.Correspondent,
                                            x.AppealCits.CorrespondentAddress,
                                            x.AppealCits.File,
                                            x.Description,
                                            x.ConfirmedGJI
                                        })
                                        .Filter(loadParams, this.Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            else
            {
                var data = domainService.GetAll()
                      .Where(x => x.OrderDate >= dateStart2 && x.OrderDate <= dateEnd2)
                       .WhereIf(!showCloseAppeals, x => x.YesNoNotSet == Gkh.Enums.YesNoNotSet.No)
                                       .Select(x => new
                                       {
                                           x.Id,
                                           Executant = x.Executant.Name,
                                           ContragentINN = x.Executant.Inn,
                                           x.Person,
                                           x.AppealCits.DocumentNumber,
                                           x.AppealCits.DateFrom,
                                           x.OrderDate,
                                           x.AppealCits.State,
                                           x.PerformanceDate,
                                           x.YesNoNotSet,
                                           x.Confirmed,
                                           x.AppealCits.Correspondent,
                                           x.AppealCits.CorrespondentAddress,
                                           x.AppealCits.File,
                                           x.Description,
                                           x.ConfirmedGJI
                                       })
                                       .Filter(loadParams, this.Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
        }

        public override IDataResult Get(IDomainService<AppealOrder> domain, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var obj = domain.Get(id);

            if (obj != null)
            {
                return new BaseDataResult(
                    new
                    {
                        obj.Id,
                        obj.AppealCits,
                        obj.Executant,
                        obj.Person,
                        obj.AppealCits.DocumentNumber,
                        obj.AppealCits.DateFrom,
                        obj.OrderDate,
                        AppealDate = obj.AppealCits.DateFrom,
                        obj.PersonPhone,
                        obj.PerformanceDate,
                        obj.YesNoNotSet,
                        obj.AppealCits.Correspondent,
                        obj.AppealCits.Email,
                        CorrespondentAddress = obj.AppealCits.CorrespondentAddress + ", кв. " + obj.AppealCits.FlatNum,
                        obj.AppealCits.File,
                        obj.Description,
                        obj.AppealText,
                        obj.Confirmed,
                        obj.AppealCits.Phone,
                        obj.ObjectCreateDate,
                        obj.ObjectEditDate,
                        obj.ObjectVersion,
                        ContragentName = obj.Executant.Name
                    });
            }

            return new BaseDataResult();
        }
    }
}