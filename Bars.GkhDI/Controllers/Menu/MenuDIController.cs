namespace Bars.GkhDi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Entities;
    using Gkh.Controllers;
    using Gkh.Entities;
    using Navigation;

    public class MenuDiController : MenuController
    {
        public ActionResult GetActionMenu(StoreLoadParams storeParams)
        {
            return new JsonNetResult(this.GetActionMenuItems());
        }

        public ActionResult GetDisclosureInfoRealityObjMenu(StoreLoadParams storeParams)
        {
            var disclosureInfoId = storeParams.Params.GetAs<long>("disclosureInfoId");

            var disclosureInfoRealityObjId = storeParams.Params.GetAs<long>("disclosureInfoRealityObjId");

            ManagingOrganization managingOrg = null;
            IEnumerable<MenuItem> collection = null;
            DateTime? startDate = null;

            if (disclosureInfoId != 0)
            {
                var info = this.Resolve<IDomainService<DisclosureInfo>>().Get(disclosureInfoId);
                managingOrg = info.ManagingOrganization;
                startDate = info.PeriodDi.DateStart;
            }

            if (managingOrg != null)
            {
                var service = this.Container.ResolveAll<IAuthorizationService>().FirstOrDefault();
                var userIdentity = this.Container.Resolve<IUserIdentity>();

                collection = this.FilterInacessibleItems(this.GetMenuItemsManOrg(disclosureInfoRealityObjId, startDate), service, userIdentity);
            }

            return new JsonNetResult(collection);
        }

        public ActionResult List(StoreLoadParams storeParams)
        {
            var typeManagement = storeParams.Params.GetAs<string>("typeManagement");

            var disclosureInfoId = storeParams.Params.GetAs<long>("disclosureInfoId");

            // Если ТСЖ или ЖСК убираем пункты договоров и фондов.
            IEnumerable<ManagingOrgDataMenuItem> collection;

            if (typeManagement == "20" || typeManagement == "40")
            {
                collection = this.GetManagingOrgDataMenuItems(disclosureInfoId, true);
            }
            else
            {
                collection = this.GetManagingOrgDataMenuItems(disclosureInfoId);
            }

            return new JsonNetResult(collection);
        }

        public IEnumerable<ActionMenuItem> GetActionMenuItems()
        {
            var managingOrgItem = new ActionMenuItem("Сведения об УО", "btnDataManOrg", string.Empty);
            var managingOrgRealityObjItem = new ActionMenuItem("Объекты в управлении", "btnObjects", string.Empty);

            var menuList = new List<ActionMenuItem>
                {
                    managingOrgItem,
                    managingOrgRealityObjItem
                };

            return menuList;
        }

        public IEnumerable<ManagingOrgDataMenuItem> GetManagingOrgDataMenuItems(long id, bool allowTsjJsk = false)
        {
            var dinfo = this.Container.ResolveDomain<DisclosureInfo>().Get(id);
            int periodStartYear = dinfo.PeriodDi.DateStart.GetValueOrDefault().Year;
            var diPercentsItems = this.Container.Resolve<IDomainService<DisclosureInfoPercent>>().GetAll().Where(x => x.DisclosureInfo.Id == id);

            var generalDataItem = new ManagingOrgDataMenuItem("Общие сведения", "B4.controller.GeneralData", this.GetDiPercent(diPercentsItems, "GeneralDataPercent"));
            var terminateContractItem = new ManagingOrgDataMenuItem("Сведения о расторгнутых договорах", "B4.controller.TerminateContract", this.GetDiPercent(diPercentsItems, "TerminateContractPercent"));
            var membershipInAssociationItem = new ManagingOrgDataMenuItem("Членство в объединениях", "B4.controller.MembershipUnions", this.GetDiPercent(diPercentsItems, "MembershipUnionsPercent"));
            var financeActivityItem = new ManagingOrgDataMenuItem("Финансовая деятельность", "B4.controller.FinActivity", this.GetDiPercent(diPercentsItems, "FinActivityPercent"));
            var fundsDataItem = new ManagingOrgDataMenuItem("Сведения о фондах", "B4.controller.FundsInfo", this.GetDiPercent(diPercentsItems, "FundsInfoPercent"));
            var adminResponsibilityItem = new ManagingOrgDataMenuItem("Административная ответственность", "B4.controller.AdminResp", this.GetDiPercent(diPercentsItems, "AdminResponsibilityPercent"));
            var licenseItem = new ManagingOrgDataMenuItem("Лицензии", "B4.controller.DisclosureInfoLicense", this.GetDiPercent(diPercentsItems, "LicensePercent"));
            var documents = new ManagingOrgDataMenuItem("Документы", "B4.controller.Documents", this.GetDiPercent(diPercentsItems, "DocumentsPercent"));
            var informationOnContractsItem = new ManagingOrgDataMenuItem("Сведения о договорах", "B4.controller.InformationOnContracts", this.GetDiPercent(diPercentsItems, "InfoOnContrPercent"));
            
            var list = new List<ManagingOrgDataMenuItem>();

            list.Add(generalDataItem);
            list.Add(terminateContractItem);
            list.Add(membershipInAssociationItem);
            list.Add(financeActivityItem);

            if (allowTsjJsk)
            {
                list.Add(fundsDataItem);
                list.Add(informationOnContractsItem);
            }
            list.Add(adminResponsibilityItem);

            if (periodStartYear >= 2015)
            {
                list.Add(licenseItem);
            }
            list.Add(documents);

            return list;
        }

        public IList<MenuItem> GetMenuItemsManOrg(long id, DateTime? startDateTime)
        {
            var DiRealObjPercentsItems = this.Container.Resolve<IDomainService<DiRealObjPercent>>().GetAll().Where(x => x.DiRealityObject.Id == id);

            var serviceItem = new MenuItem("Сведения об услугах" + this.GetDiRoPercent(DiRealObjPercentsItems, "ServicesPercent"), "B4.controller.Service");

            var otherServiceItem = new MenuItem("Прочие услуги", "B4.controller.OtherService").AddRequiredPermission("GkhDi.OtherService.View");

            var planWorkServiceRepairItem = new MenuItem("План работ по содержанию и ремонту" + this.GetDiRoPercent(DiRealObjPercentsItems, "PlanWorkServiceRepairPercent"), "B4.controller.PlanWorkServiceRepair");

            var planReductionExpenseItem = new MenuItem("План мер по снижению расходов" + this.GetDiRoPercent(DiRealObjPercentsItems, "PlanReductionExpensePercent"), "B4.controller.PlanReductionExpense").AddRequiredPermission("GkhDi.DisinfoRealObj.PlanReductionExpense.View"); ;

            var infoAboutReductionPaymentItem = new MenuItem("Сведения о случаях снижения платы" + this.GetDiRoPercent(DiRealObjPercentsItems, "InfoAboutReductionPaymentPercent"), "B4.controller.InfoAboutReductionPayment");

            var infoAboutPaymentCommunalItem = new MenuItem("Сведения об оплатах коммунальных услуг" + this.GetDiRoPercent(DiRealObjPercentsItems, "CommunalServicesPaymentPercent"), "B4.controller.InfoAboutPaymentCommunal").AddRequiredPermission("GkhDi.DisinfoRealObj.InfoAboutPaymentCommunal.View");

            var infoAboutPaymentHousingItem = new MenuItem("Сведения об оплатах жилищных услуг", "B4.controller.InfoAboutPaymentHousing").AddRequiredPermission("GkhDi.DisinfoRealObj.InfoAboutPaymentHousing.View");

            var nonResidentialPlacementItem = new MenuItem("Сведения об использовании нежилых помещений" + this.GetDiRoPercent(DiRealObjPercentsItems, "NonResidentialPlacementPercent"), "B4.controller.NonResidentialPlacement");

            var generalInfoRealityObjItem = new MenuItem("Общие сведения о доме (для рейтинга)", "B4.controller.GeneralInfoRealityObj");

            var infoAboutUseCommonFacilities = new MenuItem("Сведения об использовании мест общего пользования" + this.GetDiRoPercent(DiRealObjPercentsItems, "PlaceGeneralUsePercent"), "B4.controller.InfoAboutUseCommonFacilities");

            var finActRealityObjItem = new MenuItem("Финансовые показатели (для рейтинга)", "B4.controller.FinActivityRealityObj");

            var finItem = new MenuItem("Финансовые показатели" + this.GetDiRoPercent(DiRealObjPercentsItems, "FinActivityHousePercent"), "")
                .AddRequiredPermission("GkhDi.DisinfoRealObj.FinancialPerformance.FinancialPanel_View")
                .AddRequiredPermission("GkhDi.DisinfoRealObj.FinancialPerformance.PretensionQualityWork_View")
                .AddRequiredPermission("GkhDi.DisinfoRealObj.FinancialPerformance.DiRealObjClaimWork_View");

            finItem.Add("Общие сведения", "B4.controller.FinancialPerformance").AddRequiredPermission("GkhDi.DisinfoRealObj.FinancialPerformance.FinancialPanel_View");

            finItem.Add("Претензии по качеству работ", "B4.controller.PretensionQualityWork").AddRequiredPermission("GkhDi.DisinfoRealObj.FinancialPerformance.PretensionQualityWork_View");
            finItem.Add("Претензионно-исковая работа", "B4.controller.DiRealObjClaimWork").AddRequiredPermission("GkhDi.DisinfoRealObj.FinancialPerformance.DiRealObjClaimWork_View");
            finItem.Add("Коммунальные услуги", "B4.controller.FinPerfomanceCommunalService").AddRequiredPermission("GkhDi.DisinfoRealObj.FinancialPerformance.CommunalService_View");

            var docsRealityObjItem = new MenuItem("Документы" + this.GetDiRoPercent(DiRealObjPercentsItems, "DocumentsDiRealObjPercent"), "B4.controller.DocumentsRealityObj");

            var passportItem = new MenuItem("Паспорт дома", "B4.controller.RealityObjectPassport").
                AddRequiredPermission("GkhDi.DisinfoRealObj.RealtyObjectPassport.PassportPanelMainInformation_View");

            passportItem.Items.Add(new MenuItem("Общие сведения" + this.GetDiRoPercent(DiRealObjPercentsItems, "RealityObjectGeneralDataPercent"), "B4.controller.RealityObjectPassport").
                AddRequiredPermission("GkhDi.DisinfoRealObj.RealtyObjectPassport.PassportPanelMainInformation_View"));

            passportItem.Items.Add(new MenuItem("Лифты" + this.GetDiRoPercent(DiRealObjPercentsItems, "LiftsPercent"), "B4.controller.Lifts").
                AddRequiredPermission("GkhDi.DisinfoRealObj.RealtyObjectPassport.LiftsInfoMainInformation_View"));

            passportItem.Items.Add(new MenuItem("Приборы учёта" + this.GetDiRoPercent(DiRealObjPercentsItems, "RealtyObjectDevicesPercent"), "B4.controller.Devices").
                AddRequiredPermission("GkhDi.DisinfoRealObj.RealtyObjectPassport.DevicesInfoMainInformation_View"));

            passportItem.Items.Add(new MenuItem("Конструктивные элементы" + this.GetDiRoPercent(DiRealObjPercentsItems, "StructElementsPercent"), "B4.controller.StructElements").
                AddRequiredPermission("GkhDi.DisinfoRealObj.RealtyObjectPassport.StructElementsInformation_View"));

            passportItem.Items.Add(new MenuItem("Инженерные системы" + this.GetDiRoPercent(DiRealObjPercentsItems, "EngineerSystemsPercent"), "B4.controller.EngineerSystems").
                AddRequiredPermission("GkhDi.DisinfoRealObj.RealtyObjectPassport.EngineerSystemsInformation_View"));

            passportItem.Items.Add(new MenuItem("Управление домом" + this.GetDiRoPercent(DiRealObjPercentsItems, "HouseManagingPercent"), "B4.controller.HouseManaging").
                AddRequiredPermission("GkhDi.DisinfoRealObj.RealtyObjectPassport.HouseManaging_View"));

            var menuList = new List<MenuItem>();
            if (startDateTime != null && startDateTime.Value.Year >= 2015)
            {
                menuList.Add(passportItem);
            }
            menuList.AddRange(new[]
                {
                    serviceItem,
                    otherServiceItem,
                    planWorkServiceRepairItem,
                    planReductionExpenseItem,
                    infoAboutReductionPaymentItem,
                    infoAboutPaymentCommunalItem,
                    infoAboutPaymentHousingItem,
                    nonResidentialPlacementItem,
                    infoAboutUseCommonFacilities,
                    docsRealityObjItem,
                    generalInfoRealityObjItem,
                    finActRealityObjItem
                }
            );

            if (startDateTime != null && startDateTime.Value.Year >= 2015)
            {
                menuList.Add(finItem);
            }

            return menuList;
        }

        private string GetDiPercent(IEnumerable<DisclosureInfoPercent> percents, string code)
        {
            var disclosureInfoPercent = percents.FirstOrDefault(x => x.Code == code);
            if (disclosureInfoPercent != null)
            {
                return disclosureInfoPercent.Percent == null ? "-" : decimal.Round(disclosureInfoPercent.Percent.ToDecimal(), 4, MidpointRounding.AwayFromZero).ToStr();
            }

            return "-";
        }

        private string GetDiRoPercent(IEnumerable<DiRealObjPercent> percents, string code)
        {
            var disclosureInfoPercent = percents.FirstOrDefault(x => x.Code == code);
            if (disclosureInfoPercent != null)
            {
                return disclosureInfoPercent.Percent == null ? " (-)" : string.Format(" ( {0}% )", decimal.Round(disclosureInfoPercent.Percent.ToDecimal(), 2, MidpointRounding.AwayFromZero));
            }

            return " (-)";
        }
    }
}
