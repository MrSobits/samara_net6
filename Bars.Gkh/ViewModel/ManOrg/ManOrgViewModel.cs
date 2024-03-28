namespace Bars.Gkh.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.Utils;

    using Bars.Gkh.Authentification;
    using Domain;
    using Entities;
    using Enums;

    public class ManOrgViewModel : BaseViewModel<ManagingOrganization>
    {
        public IDomainService<ManOrgContractRealityObject> ManOrgContractRealityObjectDomain { get; set; }

        public override IDataResult List(IDomainService<ManagingOrganization> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var userManager = Container.Resolve<IGkhUserManager>();
            var contragentList = userManager.GetContragentIds();
            var activeOperator = userManager.GetActiveOperator();
            var contragent1468Id = activeOperator != null && activeOperator.Contragent != null ? activeOperator.Contragent.Id : 0;

            /*
             * Галочка над гридом показывать не действующие организации
             */
            var showNotValid = !baseParams.Params.ContainsKey("showNotValid") || baseParams.Params["showNotValid"].ToBool();
            /*
             если передан параметр Id значит мы хотим получить только те управляющие организации, которые переданы в данном параметре
             */
            var id = baseParams.Params.Contains("Id") ? baseParams.Params["Id"].ToStr() : string.Empty;
            var jskTsjOnly = baseParams.Params.ContainsKey("jskTsjOnly") && baseParams.Params["jskTsjOnly"].ToBool();
            var jskOnly = baseParams.Params.ContainsKey("jskOnly") && baseParams.Params["jskOnly"].ToBool();
            var tsjOnly = baseParams.Params.ContainsKey("tsjOnly") && baseParams.Params["tsjOnly"].ToBool();
            var manorgOnly = baseParams.Params.ContainsKey("manorgOnly") && baseParams.Params["manorgOnly"].ToBool();
            var periodDiDateStart = baseParams.Params.ContainsKey("periodDiDateStart") ? baseParams.Params["periodDiDateStart"].ToDateTime() : DateTime.MinValue;
            var periodDiDateEnd = baseParams.Params.ContainsKey("periodDiDateEnd") ? baseParams.Params["periodDiDateEnd"].ToDateTime() : DateTime.MinValue;
            var fromDisinfo = baseParams.Params.ContainsKey("fromDisinfo") && baseParams.Params["fromDisinfo"].ToBool();
            var showAll = baseParams.Params.ContainsKey("showAll") && baseParams.Params["showAll"].ToBool();
            var operatorHasContragent = baseParams.Params.ContainsKey("operatorHasContragent") && baseParams.Params["operatorHasContragent"].ToBool();
            var fromDecision = baseParams.Params.GetAs("fromDecision", false);
            
            var activeContractsQuery = ManOrgContractRealityObjectDomain.GetAll()
                .Where(x => x.ManOrgContract.StartDate <= DateTime.Now)
                .Where(x => x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= DateTime.Now);

            var listIds = new List<long>();
            if (!string.IsNullOrEmpty(id))
            {
                if (id.Contains(","))
                {
                    listIds.AddRange(id.Split(",").Select(x => x.ToLong()));
                }
                else
                {
                    listIds.Add(id.ToInt());
                }
            }

            var data = showAll ? new BaseDomainService<ManagingOrganization> { Container = Container }.GetAll() : domain.GetAll();

            if (operatorHasContragent)
            {
                data = data.WhereIf(contragentList.Count > 0 || contragent1468Id > 0, x => contragentList.Contains(x.Contragent.Id) || contragent1468Id == x.Contragent.Id);
            }

            var result = data
                .CustomFilter(Container, baseParams)
                .WhereIf(!showNotValid, x => x.ActivityGroundsTermination == GroundsTermination.NotSet)
                .WhereIf(fromDisinfo && periodDiDateEnd != DateTime.MinValue, x => x.Contragent.DateRegistration <= periodDiDateEnd)
                .WhereIf(fromDisinfo,
                    x =>
                        x.TypeManagement != TypeManagementManOrg.Other
                        && ((x.ActivityGroundsTermination != GroundsTermination.NotSet && x.ActivityDateEnd.HasValue
                             && periodDiDateStart <= x.ActivityDateEnd)
                            || x.ActivityGroundsTermination == GroundsTermination.NotSet))
                .WhereIf(jskTsjOnly,
                    x => x.TypeManagement == TypeManagementManOrg.TSJ || x.TypeManagement == TypeManagementManOrg.JSK)
                .WhereIf(jskOnly, x => x.TypeManagement == TypeManagementManOrg.JSK)
                .WhereIf(tsjOnly, x => x.TypeManagement == TypeManagementManOrg.TSJ)
                .WhereIf(
                    fromDecision && tsjOnly,
                        //// подзапрос для: ТСЖ (1 МКД) или ТСЖ (менее 30 квартир)
                        //// список действующих ТСЖ, у которых в управлении 1 действующий договор на дом 
                        //// + список действующих ТСЖ, у которых несколько действующих договоров с МКД, но количество квартир в этих МКД в сумме не более 30.
                        x => activeContractsQuery.Count(y => y.ManOrgContract.ManagingOrganization.Id == x.Id) == 1
                        || activeContractsQuery.Where(y => y.ManOrgContract.ManagingOrganization.Id == x.Id).Sum(y => y.RealityObject.NumberApartments) < 30)
                .WhereIf(manorgOnly, x => x.TypeManagement == TypeManagementManOrg.UK)
                .WhereIf(listIds.Any(), x => listIds.Contains(x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Contragent,
                    ContragentId = x.Contragent.Id,
                    ContragentName = x.Contragent.Name,
                    ContragentShortName = (x.Contragent.ShortName == null || x.Contragent.ShortName.Trim() == "") ? x.Contragent.Name : x.Contragent.ShortName,
                    ContragentInn = x.Contragent.Inn, // Для раздела регфонд->заявки на перечисление ден. средств
                    ContragentKpp = x.Contragent.Kpp, // Для раздела регфонд->заявки на перечисление ден. средств
                    ContragentOgrn = x.Contragent.Ogrn,
                    ContragentPhone = x.Contragent.Phone, // Для раздела регфонд->заявки на перечисление ден. средств
                    ContragentMailingAddress = x.Contragent.MailingAddress, // Для раздела гжи->деятельность тсж
                    ContragentJuridicalAddress = x.Contragent.JuridicalAddress, // Для раздела гжи->деятельность тсж
                    Municipality = x.Contragent.Municipality.ParentMo == null ? x.Contragent.Municipality.Name : x.Contragent.Municipality.ParentMo.Name,
                    Settlement = x.Contragent.MoSettlement != null ? x.Contragent.MoSettlement.Name : (x.Contragent.Municipality.ParentMo != null ? x.Contragent.Municipality.Name : ""),
                    MunicipalityId = x.Contragent.Municipality != null ? x.Contragent.Municipality.Id : 0,
                    x.NumberEmployees,
                    x.OfficialSite,
                    x.ActivityGroundsTermination,
                    x.TypeManagement
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.ContragentJuridicalAddress)
                .Filter(loadParams, this.Container);

            int totalCount = result.Count();

            return new ListDataResult(result.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }        
    }
}