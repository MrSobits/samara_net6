namespace Bars.Gkh.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Properties;

    using Castle.Windsor;

    /// <summary>
    /// Отчёт по Совету МКД
    /// </summary>
    public class AdviceMkdReport : BasePrintForm
    {
        private long[] municipalityIds;

        /// <summary>
        /// Value:
        /// 0 - "Ук"
        /// 1 - "Непосредственное управленеи"
        /// </summary>
        private int controlType = 0;

        /// <summary>
        /// Конструктор
        /// </summary>
        public AdviceMkdReport()
            : base(new ReportTemplateBinary(Resources.AdviceMKDReport))
        {
        }

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Наименование отчёта
        /// </summary>
        public override string Name
        {
            get { return "Отчет по Совету МКД"; }
        }

        /// <summary>
        /// Описание отчёта
        /// </summary>
        public override string Desciption
        {
            get { return "Отчет по Совету МКД"; }
        }

        /// <summary>
        /// Наименование группы
        /// </summary>
        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        /// <summary>
        /// Контроллер параметров
        /// </summary>
        public override string ParamsController
        {
            get { return "B4.controller.report.AdviceMKD"; }
        }

        /// <summary>
        /// Необходимые права доступа
        /// </summary>
        public override string RequiredPermission
        {
            get { return "Reports.GKH.AdviceMKDReport"; }
        }

        /// <summary>
        /// Установить параметры
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса </param>
        public override void SetUserParams(BaseParams baseParams)
        {
            this.municipalityIds = baseParams.Params.GetAs<string>("municipalityIds").ToLongArray();
            this.controlType = baseParams.Params.GetAs<int>("controlType");
        }

        /// <summary>
        /// The prepare report.
        /// </summary>
        /// <param name="reportParams">
        /// The report parameters.
        /// </param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var repAdviceMkdData = this.Container.Resolve<IDomainService<RealityObjectCouncillors>>();
            var repProcolsAdviceMkd = this.Container.Resolve<IDomainService<RealityObjectProtocol>>();

            using (this.Container.Using(repAdviceMkdData, repProcolsAdviceMkd))
            {
                var dataResult = new List<DataAdviceMkdReport>();

                IQueryable<ManOrgContractRealityObject> query = null;

                switch (this.controlType)
                {
                    case 0:
                        query = this.ControlType0();
                        break;
                    case 1:
                        query = this.ControlType1();
                        break;
                }

                if (query == null)
                {
                    return;
                }

                var listRealObjManorg = query
                    .Select(
                        x => new
                        {
                            x.RealityObject.Id,
                            MunicipalityName = x.RealityObject.Municipality.Name,
                            x.RealityObject.Address,
                            ContragentName = x.ManOrgContract.ManagingOrganization.Contragent.Name ?? string.Empty,
                            x.ManOrgContract.ManagingOrganization.Contragent.Phone,
                            x.ManOrgContract.ManagingOrganization.Contragent.Email
                        })
                    .ToList();

                var arrRealObjId = query.Select(x => x.RealityObject.Id).Distinct();

                var dictRealObjManorg =
                    listRealObjManorg
                        .GroupBy(x => x.Id)
                        .ToDictionary(
                            x => x.Key,
                            v => v.OrderBy(x => x.MunicipalityName).ThenBy(x => x.Address).ToList());


                var listRealObjCouncillors =
                    repAdviceMkdData.GetAll()
                        .Where(x => arrRealObjId.Contains(x.RealityObject.Id))
                        .Select(x => new { RealObjId = x.RealityObject.Id, x.Fio, x.Post, x.Phone })
                        .ToList();

                var dictRealObjCouncillors =
                    listRealObjCouncillors
                        .GroupBy(x => x.RealObjId)
                        .ToDictionary(x => x.Key, v => v.Select(x => new { x.Fio, x.Post, x.Phone }).ToList());



                var listProtocolAdviceMkd =
                    repProcolsAdviceMkd.GetAll()
                        .Where(x => arrRealObjId.Contains(x.RealityObject.Id))
                        .Select(x => new { RealObjId = x.RealityObject.Id, x.DateFrom, x.CouncilResult, x.File })
                        .ToList();

                var dictProtocolAdviceMkd =
                    listProtocolAdviceMkd
                        .GroupBy(x => x.RealObjId)
                        .ToDictionary(x => x.Key, v => v.Select(x => new { x.CouncilResult, x.DateFrom, x.File }).Last());

                foreach (var item in dictRealObjManorg)
                {
                    var itemReport = new DataAdviceMkdReport();

                    var itemData = item.Value.Last();

                    var roId = itemData.Id;

                    itemReport.MuName = itemData.MunicipalityName;
                    itemReport.AddressMkd = itemData.Address;
                    itemReport.Phone =
                        listRealObjCouncillors.Where(x => x.RealObjId == roId)
                            .Where(x => x.Post == TypeCouncillors.Сhairman)
                            .Select(x => x.Phone)
                            .FirstOrDefault();
                    itemReport.Email = itemData.Email;

                    itemReport.MoName = itemData.ContragentName;

                    if (dictRealObjCouncillors.ContainsKey(item.Key))
                    {
                        var dataAdviceMkd = dictRealObjCouncillors[item.Key];
                        itemReport.CreatesovetMkd = true;
                        itemReport.CountAdviceMkd = dataAdviceMkd.Count;
                        itemReport.FioDirectorSovetMkd =
                            dataAdviceMkd
                                .Where(x => x.Post == TypeCouncillors.Сhairman)
                                .Select(x => x.Fio)
                                .FirstOrDefault();
                    }
                    else
                    {
                        itemReport.CreatesovetMkd = false;
                    }

                    if (dictProtocolAdviceMkd.ContainsKey(item.Key))
                    {
                        var tmpDict = dictProtocolAdviceMkd[item.Key];
                        itemReport.DateFrom = tmpDict.DateFrom.ToDateTime().ToShortDateString();

                        itemReport.AdviceMkdSelect = tmpDict.CouncilResult == CouncilResult.CouncilNotCreated;

                        itemReport.Quorum = tmpDict.CouncilResult == CouncilResult.NoQuorum;

                        itemReport.AbsenteeMeeting = tmpDict.CouncilResult == CouncilResult.AbsencteeMeetingCouncilNotCreated;

                        itemReport.ExistsMKDSelectionProtocol = tmpDict.File != null;
                    }

                    dataResult.Add(itemReport);
                }

                this.FillSection(reportParams, dataResult);
            }
        }

        /// <summary>
        /// The control type 0.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable" />.
        /// </returns>
        private IQueryable<ManOrgContractRealityObject> ControlType0()
        {
            var manorgContractsDomain = this.Container.Resolve<IDomainService<ManOrgContractRelation>>();
            var contractRoService = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();

            try
            {
                var queryManOrgRelation = manorgContractsDomain.GetAll()
                .Where(x => x.Children.ManagingOrganization.TypeManagement == TypeManagementManOrg.UK)
                .Where(x => x.Children.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgJskTsj)
                .Where(x => x.Parent.TypeContractManOrgRealObj == TypeContractManOrg.JskTsj)
                .Where(x => x.TypeRelation == TypeContractRelation.TransferTsjUk)
                .Where(
                    x => x.Parent.ManagingOrganization.Contragent.ContragentState == ContragentState.Liquidated
                        || x.Parent.ManagingOrganization.Contragent.ContragentState == ContragentState.Bankrupt)
                .Where(x => x.Children.StartDate == null || x.Children.StartDate <= DateTime.Now.Date)
                .Where(x => x.Children.EndDate == null || x.Children.EndDate >= DateTime.Now.Date);
                
                var resultQuery = contractRoService.GetAll()
                    .Where(x => x.RealityObject.TypeHouse == TypeHouse.ManyApartments)
                    .Where(x => x.RealityObject.ConditionHouse != ConditionHouse.Razed)
                    .Where(
                        x => x.RealityObject.ConditionHouse != ConditionHouse.Emergency ||
                            (x.RealityObject.ConditionHouse == ConditionHouse.Emergency && !x.RealityObject.ResidentsEvicted))
                    .Where(x => x.RealityObject.NumberApartments > 4)
                    .Where(x => !x.ManOrgContract.StartDate.HasValue || x.ManOrgContract.StartDate <= DateTime.Now.Date)
                    .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= DateTime.Now.Date)
                    .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                    .Where(
                        x => (x.ManOrgContract.ManagingOrganization.TypeManagement == TypeManagementManOrg.UK
                            && x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgOwners)
                            || queryManOrgRelation.Any(y => y.Children.ManagingOrganization.Id == x.ManOrgContract.ManagingOrganization.Id));
                return resultQuery;
            }
            finally
            {
                this.Container.Release(manorgContractsDomain);
                this.Container.Release(contractRoService);
            }
        }

        /// <summary>
        /// The control type 1.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable" />.
        /// </returns>
        private IQueryable<ManOrgContractRealityObject> ControlType1()
        {
            var contractRoDomain = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();

            using (this.Container.Using(contractRoDomain))
            {
                return contractRoDomain.GetAll()
                    .Where(x => x.RealityObject.NumberApartments > 4)
                    .Where(x => x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= DateTime.Now.Date)
                    .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                    .Where(x => x.RealityObject.ConditionHouse != ConditionHouse.Razed)
                    .Where(
                        x => x.RealityObject.ConditionHouse != ConditionHouse.Emergency ||
                            (x.RealityObject.ConditionHouse == ConditionHouse.Emergency && !x.RealityObject.ResidentsEvicted))
                    .Where(x => x.RealityObject.TypeHouse == TypeHouse.ManyApartments)
                    .Where(x => x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag);
            }
        }

        /// <summary>
        /// The fill section.
        /// </summary>
        /// <param name="reportPar">
        /// The report par.
        /// </param>
        /// <param name="resultData">
        /// The result data.
        /// </param>
        private void FillSection(ReportParams reportPar, IEnumerable<DataAdviceMkdReport> resultData)
        {
            var section = reportPar.ComplexReportParams.ДобавитьСекцию("section");

            var listData = resultData.GroupBy(x => x.MuName);
            foreach (var data in listData.OrderBy(x => x.Key))
            {
                foreach (var item in data.OrderBy(x => x.AddressMkd))
                {
                    section.ДобавитьСтроку();

                    section["MuName"] = item.MuName;
                    section["Address"] = item.AddressMkd;
                    section["ManOrgName"] = item.MoName;
                    section["CreateAdviceMkd"] = item.CreatesovetMkd ? "Да" : "Нет";
                    section["Fio"] = item.FioDirectorSovetMkd;
                    section["Count"] = item.CountAdviceMkd;

                    section["DateFrom"] = item.DateFrom;

                    section["QuorumNot"] = item.Quorum ? "Да" : string.Empty;
                    section["AdviceMkdNotSelect"] = item.AdviceMkdSelect ? "Да" : string.Empty;
                    section["AbsenteeMeeting"] = item.AbsenteeMeeting ? "Да" : string.Empty;
                    section["Phone"] = item.Phone;
                    section["Email"] = item.Email;
                    section["ExistsMKDSelectionProtocol"] = item.ExistsMKDSelectionProtocol ? "1" : "0";
                }
            }
        }

        /// <summary>
        /// The data advice report.
        /// </summary>
        private class DataAdviceMkdReport
        {
            public string MuName;

            public string AddressMkd;

            public string MoName;

            public bool Quorum;

            public bool AdviceMkdSelect;

            public bool AbsenteeMeeting;

            public bool CreatesovetMkd;

            public string FioDirectorSovetMkd;

            public int CountAdviceMkd;

            public string DateFrom;

            public string Phone;

            public string Email;

            public bool ExistsMKDSelectionProtocol;
        }
    }
}