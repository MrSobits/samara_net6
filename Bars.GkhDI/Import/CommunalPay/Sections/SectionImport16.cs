namespace Bars.GkhDi.Import.Sections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.PassportProvider;
    using Bars.GkhDi.Entities;

    using Castle.Windsor;

    public class SectionImport16 : ISectionImport
    {
        public string Name => "Импорт из комплат секция #16";

        public IWindsorContainer Container { get; set; }

        public void ImportSection(ImportParams importParams)
        {
            var resultFinActivity = new List<FinActivityManagRealityObj>();
            var resultRealObj = new List<RealityObject>();
            var resultInfoRealObj = new List<DisclosureInfoRealityObj>();

            var inn = importParams.Inn;
            var sectionsData = importParams.SectionData;

            if (sectionsData.Section16.Count == 0)
            {
                return;
            }

            var logImport = importParams.LogImport;
            var realityObjects = importParams.RealityObjectIds;
            var realityObjectDict = importParams.RealObjsImportInfo;

            var disinfoService = this.Container.Resolve<IDomainService<DisclosureInfo>>();
            var manOrgService = this.Container.Resolve<IDomainService<ManagingOrganization>>();
            var disclosureInfoRealityObjService = this.Container.Resolve<IDomainService<DisclosureInfoRealityObj>>();
            var realityObjService = this.Container.Resolve<IDomainService<RealityObject>>();
            var finActivityRealityObjService = this.Container.Resolve<IDomainService<FinActivityManagRealityObj>>();
            var passportProvider = this.Container.Resolve<IPassportProvider>();

            using (this.Container.Using(disclosureInfoRealityObjService,
                disinfoService,
                manOrgService,
                realityObjService,
                finActivityRealityObjService,
                passportProvider))
            {
                var disclosureInfo = disinfoService
                    .GetAll()
                    .FirstOrDefault(
                        x => x.ManagingOrganization.Contragent.Inn == inn
                            && x.PeriodDi.Id == importParams.PeriodDiId);

                var finActManagRealObjs = finActivityRealityObjService.GetAll().Where(x => x.DisclosureInfo.Id == disclosureInfo.Id).ToList();

                var disclosureInfoRealityObjs = disclosureInfoRealityObjService.GetAll()
                    .Where(x => x.PeriodDi.Id == importParams.PeriodDiId && realityObjects.Contains(x.RealityObject.Id))
                    .ToList();

                var realObjs = realityObjService.GetAll().Where(x => realityObjects.Contains(x.Id)).ToList();

                var managingOrganization = manOrgService.GetAll().FirstOrDefault(x => x.Contragent.Inn == inn);
                if (managingOrganization == null)
                {
                    logImport.Warn(this.Name, string.Format("Не удалось управляющую организацию с ИНН {0}", inn));
                    return;
                }

                foreach (var section16Record in sectionsData.Section16)
                {
                    var realityObject = realityObjectDict.ContainsKey(section16Record.CodeErc) ? realityObjectDict[section16Record.CodeErc] : null;
                    if (realityObject == null)
                    {
                        logImport.Warn(this.Name, string.Format("Не удалось получить дом с кодом ЕРЦ {0}", section16Record.CodeErc));
                        continue;
                    }

                    if (!realityObjects.Contains(realityObject.Id))
                    {
                        logImport.Warn(
                            this.Name,
                            string.Format(
                                "Для дома с кодом ЕРЦ {0} и упр. организацией с инн {1} нет договора управления в данном периоде",
                                section16Record.CodeErc,
                                inn));
                        continue;
                    }

                    var finActManagRealObj = finActManagRealObjs.FirstOrDefault(x => x.RealityObject.Id == realityObject.Id);

                    var realityObj = realObjs.FirstOrDefault(x => x.Id == realityObject.Id);

                    if (finActManagRealObj == null)
                    {
                        finActManagRealObj = new FinActivityManagRealityObj
                        {
                            DisclosureInfo = disclosureInfo,
                            RealityObject = realityObj
                        };
                    }

                    finActManagRealObj.SumIncomeManage = section16Record.ChargeSumManProfit;

                    if (finActManagRealObj.Id > 0)
                    {
                        logImport.CountChangedRows++;
                    }
                    else
                    {
                        logImport.CountAddedRows++;
                    }

                    resultFinActivity.Add(finActManagRealObj);

                    realityObj.BuildYear = section16Record.ConstrYear;
                    realityObj.MaximumFloors = section16Record.FloorMax;
                    realityObj.Floors = section16Record.FloorMin;
                    realityObj.NumberEntrances = section16Record.EntranceNum;
                    realityObj.NumberLifts = section16Record.ElevNum;
                    realityObj.NumberApartments = section16Record.FlatNum;
                    realityObj.NumberNonResidentialPremises = section16Record.NonResidNum;
                    realityObj.AreaLiving = section16Record.ResidSq;
                    realityObj.AreaNotLivingPremises = section16Record.NonResidSq;
                    realityObj.AreaNotLivingFunctional = section16Record.CommunSq;

                    if (realityObj.Id > 0)
                    {
                        logImport.CountChangedRows++;
                    }
                    else
                    {
                        logImport.CountAddedRows++;
                    }

                    if (realityObj.AreaLivingNotLivingMkd.HasValue && realityObj.AreaLivingNotLivingMkd.ToDecimal() < realityObj.AreaLiving)
                    {
                        logImport.Warn(this.Name,
                            string.Format("Общая площадь жилых и нежилых помещений в МКД должна быть не меньше, чем жилых всего {0}",
                                section16Record.CodeErc));
                    }
                    else
                    {
                        resultRealObj.Add(realityObj);
                    }


                    if (section16Record.WallMaterial.IsNotEmpty())
                    {
                        List<SerializePassportValue> values = new List<SerializePassportValue>
                        {
                            new SerializePassportValue
                            {
                                ComponentCode = "Form_5_2",
                                CellCode = "1:3",
                                Value = section16Record.WallMaterial
                            }
                        };

                        passportProvider.UpdateForm(realityObject.Id, "Form_5_2", values);
                    }
                    else
                    {
                        logImport.Warn(this.Name,
                            string.Format("Не удалось получить Материал несущих стен с кодом {0}", section16Record.WallMaterialCode));
                    }

                    if (section16Record.ChuteNum != null)
                    {
                        List<SerializePassportValue> values = new List<SerializePassportValue>
                        {
                            new SerializePassportValue
                            {
                                ComponentCode = "Form_3_7_3",
                                CellCode = "5:1",
                                Value = section16Record.ChuteNum.ToString()
                            }
                        };

                        passportProvider.UpdateForm(realityObject.Id, "Form_3_7", values);
                    }

                    var disclosureInfoRealityObj = disclosureInfoRealityObjs.FirstOrDefault(x => x.RealityObject.Id == realityObject.Id);

                    disclosureInfoRealityObj.AdvancePayments = section16Record.UserPrepay;
                    disclosureInfoRealityObj.Debt = section16Record.UserDebt;
                    disclosureInfoRealityObj.ChargeForMaintenanceAndRepairsAll = section16Record.RoutineMaintenCharge;
                    disclosureInfoRealityObj.ChargeForMaintenanceAndRepairsMaintanance = section16Record.MaintenCharge;
                    disclosureInfoRealityObj.ChargeForMaintenanceAndRepairsRepairs = section16Record.RoutineCharge;
                    disclosureInfoRealityObj.ChargeForMaintenanceAndRepairsManagement = section16Record.ManagCharge;
                    disclosureInfoRealityObj.ReceivedCashAll = section16Record.GetChargeAll;
                    disclosureInfoRealityObj.ReceivedCashFromOwners = section16Record.GetChargeRent;
                    disclosureInfoRealityObj.CashBalanceAll = section16Record.Budget;
                    disclosureInfoRealityObj.CashBalanceAdvancePayments = section16Record.UserPrepayEnd;
                    disclosureInfoRealityObj.CashBalanceDebt = section16Record.UserDebtEnd;
                    disclosureInfoRealityObj.ComServReceivedPretensionCount = section16Record.ClaimNum;
                    disclosureInfoRealityObj.ComServApprovedPretensionCount = section16Record.StatisfClaimNum;
                    disclosureInfoRealityObj.PretensionRecalcSum = section16Record.ClaimRecalcSum;
                    disclosureInfoRealityObj.ComServStartAdvancePay = section16Record.UserCommPrepayBegin;
                    disclosureInfoRealityObj.ComServStartDebt = section16Record.UserCommDebtBegin;
                    disclosureInfoRealityObj.ComServEndAdvancePay = section16Record.UserCommPrepayEnd;
                    disclosureInfoRealityObj.ComServEndDebt = section16Record.UserCommDebtEnd;
                    disclosureInfoRealityObj.ComServPretensionRecalcSum = section16Record.ComRecalcSum;

                    if (disclosureInfoRealityObj.Id > 0)
                    {
                        logImport.CountChangedRows++;
                    }
                    else
                    {
                        logImport.CountAddedRows++;
                    }

                    resultInfoRealObj.Add(disclosureInfoRealityObj);
                }

                this.InTransaction(resultFinActivity, finActivityRealityObjService);
                this.InTransaction(resultRealObj, realityObjService);
                this.InTransaction(resultInfoRealObj, disclosureInfoRealityObjService);
            }
        }

        /// <summary>
        /// Транзакция
        /// </summary>
        /// <param name="list"></param>
        /// <param name="repos"></param>
        private void InTransaction(IEnumerable<PersistentObject> list, IDomainService repos)
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var entity in list)
                    {
                        if (entity.Id > 0)
                        {
                            repos.Update(entity);
                        }
                        else
                        {
                            repos.Save(entity);
                        }
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
            }
        }
    }
}