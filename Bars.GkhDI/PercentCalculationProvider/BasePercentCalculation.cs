namespace Bars.GkhDi.PercentCalculationProvider
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.Log;
    using Bars.GkhDi.Calculating;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    using Castle.Windsor;

    using Microsoft.Extensions.Logging;

    using NHibernate;

    /// <summary>
    /// Базовый калькулятор процентов
    /// </summary>
    public abstract class BasePercentCalculation : IPercentCalculation
    {
        #region property injections
        public IWindsorContainer Container { get; set; }
        public IDomainService<DisclosureInfo> DisInfoDomain { get; set; }
        public IDomainService<DisclosureInfoRealityObj> DisInfoRoDomain { get; set; }
        public IDomainService<DisclosureInfoPercent> DisInfoPercentDomain { get; set; }
        public IDomainService<RepairService> RepairServiceDomain { get; set; }
        public IDomainService<ProviderService> ProviderServiceDomain { get; set; }
        public IDomainService<WorkRepairList> WorkRepairListDomain { get; set; }
        public IDomainService<TariffForConsumers> TariffForConsumersDomain { get; set; }
        public IDomainService<WorkRepairTechServ> WorkRepairTechServDomain { get; set; }
        public IDomainService<PlanWorkServiceRepairWorks> PlanWorkServiceRepairWorksDomain { get; set; }
        public IDomainService<PlanWorkServiceRepair> PlanWorkServiceRepairDomain { get; set; }
        public IDomainService<PlanReductionExpense> PlanReductionExpenseDomain { get; set; }
        public IDomainService<PlanReductionExpenseWorks> PlanReductionExpenseWorksDomain { get; set; }
        public IDomainService<InfoAboutReductionPayment> InfoAboutReductionPaymentDomain { get; set; }
        public IDomainService<InfoAboutUseCommonFacilities> InfoAboutUseCommonFacilitiesDomain { get; set; }
        public IDomainService<NonResidentialPlacement> NonResidentialPlacementDomain { get; set; }
        public IDomainService<DocumentsRealityObj> DocumentsRealityObjDomain { get; set; }
        public IDomainService<BaseService> BaseServiceDomain { get; set; }
        public IDomainService<TariffForRso> TariffForRsoDomain { get; set; }
        public IDomainService<ManOrgContractRealityObject> ManOrgContractRealityObjectDomain { get; set; }
        public IDomainService<ArchiveDiPercent> ArchiveDiPercentDomain { get; set; }
        public IDomainService<ServicePercent> ServicePercentDomain { get; set; }
        public IDomainService<DiRealObjPercent> DiRealObjPercentDomain { get; set; }
        public IDomainService<ArchiveDiRoPercent> ArchiveDiRoPercentDomain { get; set; }

        public ISessionProvider SessionProvider { get; set; }
        #endregion

        protected PeriodDi period;

        protected ICalcPercentAlgoritm PercentAlgoritm { get; private set; }

        protected IQueryable<ManOrgContractRealityObject> moRoQuery;

        protected List<ServicePercent> ServicePercents = new List<ServicePercent>();

        protected List<DiRealObjPercent> RealObjPercents = new List<DiRealObjPercent>();

        protected List<DisclosureInfoPercent> DisInfoPercents = new List<DisclosureInfoPercent>();
        
        protected List<int> liftServCodes = new List<int> { 27, 28 };

        protected Dictionary<long, long[]> RealObjByDi = new Dictionary<long, long[]>();

        protected Dictionary<long, PercCalcResult> DictRoPerc = new Dictionary<long, PercCalcResult>();

        protected HashSet<int> TempCompleteCodes = new HashSet<int>();

        protected HashSet<int> TempNullCodes = new HashSet<int>();

        private void InitCalcAlgoritm()
        {
            this.PercentAlgoritm = this.InitCalcAlgoritmInternal();
        }

        public abstract bool CheckByPeriod(PeriodDi periodDi);

        public ILogger LogManager { get; set; }

        protected IAsyncLogger<DisclosureInfoEmptyFieldsBase> EmptyFieldsLogger { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        protected BasePercentCalculation()
        {
            this.InitCalcAlgoritm();
        }

        /// <inheritdoc />
        public IDataResult MassCalculate(PeriodDi periodDi, long[] muIds)
        {
            try
            {
                this.EmptyFieldsLogger = this.Container.Resolve<IAsyncLogger<DisclosureInfoEmptyFieldsBase>>();

                if (periodDi == null)
                {
                    return BaseDataResult.Error("Неверный период!");
                }

                if (muIds.Length == 0)
                {
                    return BaseDataResult.Error("Нет выбранных муниципальных образований!");
                }

                var diQuery = this.DisInfoDomain.GetAll()
                    .Where(x => x.ManagingOrganization.Contragent.Municipality != null)
                    .Where(x => x.PeriodDi.Id == periodDi.Id)
                    .Where(x => muIds.Contains(x.ManagingOrganization.Contragent.Municipality.Id));

                var disInfos = diQuery.ToList();

                var disInfoManOrg = disInfos.ToDictionary(x => x.ManagingOrganization.Id, y => y.Id);

                this.period = periodDi;

                this.GetManagRealObj(disInfoManOrg, diQuery);

                var dictMoPercent = this.CalculateManOrgsInfo(disInfos, diQuery);

                this.CalculateRealObj();

                this.CalculateMainInfo(dictMoPercent);

                this.EmptyFieldsLogger.Flush();

                this.SaveServicePercents();
                this.SaveRoPercents();
                this.SaveDiPercents(diQuery);

                return new BaseDataResult();
            }
            catch (Exception e)
            {
                var error = $"Ошибка массового расчета: {e.Message}";

                this.LogManager.LogError(e, error);

                return BaseDataResult.Error(error);
            }
            finally
            {
                this.ServicePercents?.Clear();
                this.RealObjPercents?.Clear();
                this.DisInfoPercents?.Clear();
                this.RealObjByDi?.Clear();
                this.DictRoPerc?.Clear();
                this.TempCompleteCodes?.Clear();
                this.TempNullCodes?.Clear();

                this.Container.Release(this.EmptyFieldsLogger);

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /// <inheritdoc />
        public IDataResult Calculate(DisclosureInfo disInfo)
        {
            try
            {
                this.EmptyFieldsLogger = this.Container.Resolve<IAsyncLogger<DisclosureInfoEmptyFieldsBase>>();

                this.period = disInfo.PeriodDi;
                if (disInfo.InCalculation)
                {
                    return BaseDataResult.Error("По данной управляющей компании в этом периоде уже идет расчет процентов!");
                }

                this.SessionProvider.InStatelessTransaction(stateless =>
                {
                    disInfo.IsCalculation = true;
                    stateless.Update(disInfo);
                });

                var diQuery = this.DisInfoDomain.GetAll().Where(x => x.Id == disInfo.Id);

                var disInfoManOrg = new Dictionary<long, long> { { disInfo.ManagingOrganization.Id, disInfo.Id } };

                this.GetManagRealObj(disInfoManOrg, diQuery);

                var dictMoPercent = this.CalculateManOrgsInfo(new[] { disInfo }, diQuery);

                this.DictRoPerc.Clear();

                this.CalculateRealObj();

                this.CalculateMainInfo(dictMoPercent);

                this.EmptyFieldsLogger.Flush();

                this.SaveServicePercents();
                this.SaveRoPercents();
                this.SaveDiPercents(diQuery);

                var diPerc = this.DisInfoPercents.FirstOrDefault(x => x.Code == "DisclosureInfoPercentProvider");
                var manOrgInfoPerc = this.DisInfoPercents.FirstOrDefault(x => x.Code == "ManOrgInfoPercent");
                var realObjsPerc = this.DisInfoPercents.FirstOrDefault(x => x.Code == "RealObjsPercent");

                return
                    new BaseDataResult(
                        new
                        {
                            diperc = diPerc != null ? diPerc.Percent : 0,
                            manorginfoperc = manOrgInfoPerc != null ? manOrgInfoPerc.Percent : 0,
                            realobjsperc = realObjsPerc != null ? realObjsPerc.Percent : 0
                        });
            }
            catch (Exception e)
            {
                var error = $"Ошибка расчета: {e.Message}";

                this.LogManager.LogError(e, error);

                return BaseDataResult.Error(error);
            }
            finally
            {
                this.SessionProvider.InStatelessTransaction(stateless =>
                {
                    disInfo.IsCalculation = false;
                    stateless.Update(disInfo);
                });

                this.ServicePercents?.Clear();
                this.RealObjPercents?.Clear();
                this.DisInfoPercents?.Clear();
                this.RealObjByDi?.Clear();
                this.DictRoPerc?.Clear();
                this.TempCompleteCodes?.Clear();
                this.TempNullCodes?.Clear();
                this.Container.Release(this.EmptyFieldsLogger);

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        protected abstract Dictionary<long, PercCalcResult> CalculateManOrgsInfo(IEnumerable<DisclosureInfo> disInfos, IQueryable<DisclosureInfo> diQuery);

        protected abstract void CalculateRealObj();

        protected virtual ICalcPercentAlgoritm InitCalcAlgoritmInternal() => new PositionCalcAlogritm();

        private void GetManagRealObj(Dictionary<long, long> disInfoManOrg, IQueryable<DisclosureInfo> diInfo)
        {
            this.moRoQuery = this.ManOrgContractRealityObjectDomain
                .GetAll()
                .Where(x => diInfo.Any(y => y.ManagingOrganization.Id == x.ManOrgContract.ManagingOrganization.Id))
                .Where(x => x.ManOrgContract.StartDate <= this.period.DateEnd && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= this.period.DateStart))
                .Where(x => x.RealityObject.TypeHouse != TypeHouse.BlockedBuilding
                         && x.RealityObject.TypeHouse != TypeHouse.Individual)
                .Where(x => x.RealityObject.ConditionHouse == ConditionHouse.Serviceable
                         || x.RealityObject.ConditionHouse == ConditionHouse.Emergency
                         || x.RealityObject.ConditionHouse == ConditionHouse.Dilapidated
                         && !x.RealityObject.ResidentsEvicted)
                .WhereIf(this.period.DateAccounting.HasValue, x => !x.RealityObject.DateCommissioning.HasValue || x.RealityObject.DateCommissioning.Value <= this.period.DateAccounting.Value);

            var realObjManOrg = this.moRoQuery
                .Select(x => new { x.RealityObject.Id, ManOrgId = x.ManOrgContract.ManagingOrganization.Id })
                .AsEnumerable()
                .GroupBy(x => x.ManOrgId)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Id).Distinct().ToArray());

            this.RealObjByDi = realObjManOrg.Where(roMo => disInfoManOrg.ContainsKey(roMo.Key)).ToDictionary(roMo => disInfoManOrg[roMo.Key], roMo => roMo.Value);
        }

        protected void CalculateMainInfo(Dictionary<long, PercCalcResult> dictMoPercent)
        {
            foreach (var disInfo in dictMoPercent)
            {
                decimal realObjsPercent = 0;
                if (this.RealObjByDi.ContainsKey(disInfo.Key))
                {
                    realObjsPercent += this.RealObjByDi[disInfo.Key].Sum(realObj => this.DictRoPerc.ContainsKey(realObj) ? this.DictRoPerc[realObj].Percent.ToDecimal() : 0);
                }
                realObjsPercent = this.RealObjByDi.ContainsKey(disInfo.Key) ? decimal.Divide(realObjsPercent, this.RealObjByDi[disInfo.Key].Length) : 100;

                var percentCalculationItem = new DisclosureInfoPercent
                {
                    Code = "RealObjsPercent",
                    TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                    Percent = realObjsPercent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = this.RealObjByDi.ContainsKey(disInfo.Key) ? this.RealObjByDi[disInfo.Key].Length : 0,
                    CompletePositionsCount = 0,
                    ActualVersion = 1,
                    DisclosureInfo = new DisclosureInfo { Id = disInfo.Key }
                };

                this.DisInfoPercents.Add(percentCalculationItem);

                var percent = ((realObjsPercent + disInfo.Value.Percent) / 2).ToDecimal();
                percent = this.GetRoundingPercent(percent);

                this.DisInfoPercents.Add(new DisclosureInfoPercent
                {
                    Code = "DisclosureInfoPercentProvider",
                    TypeEntityPercCalc = TypeEntityPercCalc.DisclosureInfo,
                    Percent = percent,
                    CalcDate = DateTime.Now.Date,
                    PositionsCount = 2,
                    CompletePositionsCount = 0,
                    ActualVersion = 1,
                    DisclosureInfo = new DisclosureInfo { Id = disInfo.Key }
                });
            }
        }

        protected decimal GetRoundingPercent(decimal percent)
        {
            var roundingpercent = decimal.Round(percent.ToDecimal(), 2, MidpointRounding.AwayFromZero);
            if (roundingpercent == 100 && percent != 100)
            {
                var procent = percent.ToString(CultureInfo.InvariantCulture);
                percent = procent.Remove(5).ToDecimal();
            }
            else
            {
                percent = decimal.Round(percent.ToDecimal(), 2, MidpointRounding.AwayFromZero);
            }
            return percent;
        }

        protected void SaveDiPercents(IQueryable<DisclosureInfo> diInfoQuery)
        {
            var disInfoPercentItems = this.DisInfoPercentDomain.GetAll()
                .Where(x => diInfoQuery.Any(y => y.Id == x.DisclosureInfo.Id))
                .GroupBy(x => x.DisclosureInfo.Id)
                .ToDictionary(x => x.Key);

            var archDiPercentItems = this.ArchiveDiPercentDomain.GetAll()
                .Where(x => diInfoQuery.Any(y => y.Id == x.DisclosureInfo.Id) && x.CalcDate == DateTime.Now.Date)
                .GroupBy(x => x.DisclosureInfo.Id)
                .ToDictionary(x => x.Key);

            var disInfoPercentToSave = new List<DisclosureInfoPercent>();
            var archiveDiPercentToSave = new List<ArchiveDiPercent>();

            foreach (var tempPercentItem in this.DisInfoPercents)
            {
                var oldPercentItem = disInfoPercentItems.ContainsKey(tempPercentItem.DisclosureInfo.Id) ? disInfoPercentItems[tempPercentItem.DisclosureInfo.Id].FirstOrDefault(x => x.Code == tempPercentItem.Code) : null;

                if (oldPercentItem != null)
                {
                    oldPercentItem.Percent = tempPercentItem.Percent;
                    oldPercentItem.CompletePositionsCount = tempPercentItem.CompletePositionsCount;
                    oldPercentItem.PositionsCount = tempPercentItem.PositionsCount;
                    oldPercentItem.ActualVersion = tempPercentItem.ActualVersion;
                    oldPercentItem.CalcDate = tempPercentItem.CalcDate;
                    disInfoPercentToSave.Add(oldPercentItem);
                }
                else
                {
                    disInfoPercentToSave.Add(tempPercentItem);
                }

                var archivePercItem = new ArchiveDiPercent
                {
                    Code = tempPercentItem.Code,
                    TypeEntityPercCalc = tempPercentItem.TypeEntityPercCalc,
                    Percent = tempPercentItem.Percent,
                    CalcDate = tempPercentItem.CalcDate,
                    PositionsCount = tempPercentItem.PositionsCount,
                    CompletePositionsCount = tempPercentItem.CompletePositionsCount,
                    ActualVersion = tempPercentItem.ActualVersion,
                    DisclosureInfo = tempPercentItem.DisclosureInfo
                };

                var oldArchivePercentItem = archDiPercentItems.ContainsKey(archivePercItem.DisclosureInfo.Id) ? archDiPercentItems[archivePercItem.DisclosureInfo.Id].FirstOrDefault(x => x.Code == tempPercentItem.Code) : null;

                if (oldArchivePercentItem != null)
                {
                    oldArchivePercentItem.Percent = archivePercItem.Percent;
                    oldArchivePercentItem.CompletePositionsCount = archivePercItem.CompletePositionsCount;
                    oldArchivePercentItem.PositionsCount = archivePercItem.PositionsCount;
                    oldArchivePercentItem.ActualVersion = archivePercItem.ActualVersion;
                    archiveDiPercentToSave.Add(oldArchivePercentItem);
                }
                else
                {
                    archiveDiPercentToSave.Add(archivePercItem);
                }
            }

            try
            {
                TransactionHelper.InsertInManyTransactions(this.Container, disInfoPercentToSave, useStatelessSession: true);
                TransactionHelper.InsertInManyTransactions(this.Container, archiveDiPercentToSave, useStatelessSession: true);
            }
            finally
            {
                disInfoPercentToSave.Clear();
                archiveDiPercentToSave.Clear();
            }

        }

        protected void SaveServicePercents()
        {
            var servicePercentItems = this.ServicePercentDomain.GetAll()
                .Where(x => this.moRoQuery.Any(y => y.RealityObject.Id == x.Service.DisclosureInfoRealityObj.RealityObject.Id))
                .Where(x => x.Service.DisclosureInfoRealityObj.PeriodDi == this.period)
                .ToDictionary(x => x.Service.Id);

            var servicePercentToSave = new List<ServicePercent>();

            foreach (var tempPercentItem in this.ServicePercents)
            {
                if (servicePercentItems.ContainsKey(tempPercentItem.Service.Id))
                {
                    var oldPercentItem = servicePercentItems[tempPercentItem.Service.Id];

                    if (oldPercentItem != null)
                    {
                        oldPercentItem.Percent = tempPercentItem.Percent;
                        oldPercentItem.CompletePositionsCount = tempPercentItem.CompletePositionsCount;
                        oldPercentItem.PositionsCount = tempPercentItem.PositionsCount;
                        oldPercentItem.ActualVersion = tempPercentItem.ActualVersion;
                        oldPercentItem.CalcDate = tempPercentItem.CalcDate;
                        servicePercentToSave.Add(oldPercentItem);
                    }
                }
                else
                {
                    servicePercentToSave.Add(tempPercentItem);
                }
            }

            try
            {
                TransactionHelper.InsertInManyTransactions(this.Container, servicePercentToSave, useStatelessSession: true);
            }
            finally
            {
                servicePercentToSave.Clear();
            }

            this.ServicePercents.Clear();
        }

        protected void SaveRoPercents()
        {
            var realObjPercentItems = this.DiRealObjPercentDomain.GetAll()
                .Where(x => x.DiRealityObject.PeriodDi.Id == this.period.Id)
                .Where(x => this.moRoQuery.Any(y => y.RealityObject.Id == x.DiRealityObject.RealityObject.Id))
                .GroupBy(x => x.DiRealityObject.Id)
                .ToDictionary(x => x.Key);

            var archRoPercentItems = this.ArchiveDiRoPercentDomain.GetAll()
                .Where(x => x.CalcDate == DateTime.Now.Date)
                .Where(x => x.DiRealityObject.PeriodDi.Id == this.period.Id)
                .Where(x => this.moRoQuery.Any(y => y.RealityObject.Id == x.DiRealityObject.RealityObject.Id))
                .GroupBy(x => x.DiRealityObject.Id)
                .ToDictionary(x => x.Key, y => y.First());

            var DiRealObjPercentToSave = new List<DiRealObjPercent>();
            var ArchiveDiRoPercentToSave = new List<ArchiveDiRoPercent>();

            foreach (var tempPercentItem in this.RealObjPercents)
            {
                if (realObjPercentItems.ContainsKey(tempPercentItem.DiRealityObject.Id))
                {
                    var oldPercentItem = realObjPercentItems[tempPercentItem.DiRealityObject.Id].FirstOrDefault(x => x.Code == tempPercentItem.Code);

                    if (oldPercentItem != null)
                    {
                        oldPercentItem.Percent = tempPercentItem.Percent;
                        oldPercentItem.CompletePositionsCount = tempPercentItem.CompletePositionsCount;
                        oldPercentItem.PositionsCount = tempPercentItem.PositionsCount;
                        oldPercentItem.ActualVersion = tempPercentItem.ActualVersion;
                        oldPercentItem.CalcDate = tempPercentItem.CalcDate;
                        DiRealObjPercentToSave.Add(oldPercentItem);
                        continue;
                    }
                }

                DiRealObjPercentToSave.Add(tempPercentItem);
            }

            foreach (var tempPercentItem in this.RealObjPercents.Where(x => x != null && x.Code == "DiRealObjPercent"))
            {
                if (archRoPercentItems.ContainsKey(tempPercentItem.DiRealityObject.Id))
                {
                    var oldArchivePercentItem = archRoPercentItems[tempPercentItem.DiRealityObject.Id];

                    if (oldArchivePercentItem != null)
                    {
                        oldArchivePercentItem.Percent = tempPercentItem.Percent;
                        oldArchivePercentItem.CompletePositionsCount = tempPercentItem.CompletePositionsCount;
                        oldArchivePercentItem.PositionsCount = tempPercentItem.PositionsCount;
                        oldArchivePercentItem.ActualVersion = tempPercentItem.ActualVersion;
                        oldArchivePercentItem.CalcDate = tempPercentItem.CalcDate;
                        ArchiveDiRoPercentToSave.Add(oldArchivePercentItem);
                    }
                }
                else
                {
                    var archivePercItem = new ArchiveDiRoPercent
                    {
                        Code = tempPercentItem.Code,
                        TypeEntityPercCalc = tempPercentItem.TypeEntityPercCalc,
                        Percent = tempPercentItem.Percent,
                        CalcDate = tempPercentItem.CalcDate,
                        PositionsCount = tempPercentItem.PositionsCount,
                        CompletePositionsCount = tempPercentItem.CompletePositionsCount,
                        ActualVersion = tempPercentItem.ActualVersion,
                        DiRealityObject = tempPercentItem.DiRealityObject
                    };

                    ArchiveDiRoPercentToSave.Add(archivePercItem);
                }
            }

            try
            {
                TransactionHelper.InsertInManyTransactions(this.Container, DiRealObjPercentToSave, useStatelessSession: true);
                TransactionHelper.InsertInManyTransactions(this.Container, ArchiveDiRoPercentToSave, useStatelessSession: true);
            }
            finally
            {
                DiRealObjPercentToSave.Clear();
                ArchiveDiRoPercentToSave.Clear();
            }

        }

        protected void AddServiceNullPercent(long serviceId)
        {
            this.ServicePercents.Add(new ServicePercent
            {
                Code = "ServicePercent",
                TypeEntityPercCalc = TypeEntityPercCalc.Service,
                Percent = null,
                CalcDate = DateTime.Now.Date,
                PositionsCount = 0,
                CompletePositionsCount = 0,
                ActualVersion = 1,
                Service = new BaseService { Id = serviceId }
            }.AddForcePercent(this.PercentAlgoritm));
        }

        protected class ServiceProxy
        {
            public long Id { get; set; }

            public int Code { get; set; }

            public string Name { get; set; }

            public KindServiceDi KindServiceDi { get; set; }

            public long DiRealObjId { get; set; }

            public long RealObjId { get; set; }

            public bool HasProvider { get; set; }
        }

        protected ISession OpenSession()
        {
            return this.Container.Resolve<ISessionProvider>().GetCurrentSession();
        }
    }
}