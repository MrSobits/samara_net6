namespace Bars.GkhGji.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    public class ProtocolTotalTable : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        #region параметры
        private DateTime dateStart = DateTime.MinValue;
        private DateTime dateEnd = DateTime.MaxValue;
        private List<long> municipalityIds = new List<long>();
        private bool canceled;
        private bool returned;
        private bool remarked;
        #endregion

        public ProtocolTotalTable() : base(new ReportTemplateBinary(Properties.Resources.ProtocolTotalTable)){ }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GJI.ProtocolTotalTable";
            }
        }

        public override string Name
        {
            get { return "Итоговая таблица к отчету по протоколам"; }
        }

        public override string Desciption
        {
            get { return "Итоговая таблица к отчету по протоколам"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.ProtocolTotalTable"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.dateStart = baseParams.Params["dateStart"].ToDateTime();
            this.dateEnd = baseParams.Params["dateEnd"].ToDateTime();
            this.returned = baseParams.Params["returned"].ToBool();
            this.canceled = baseParams.Params["canceled"].ToBool();
            this.remarked = baseParams.Params["remarked"].ToBool();

            var municipalityStr = baseParams.Params["municipalityIds"].ToString();
            this.municipalityIds.AddRange(!string.IsNullOrEmpty(municipalityStr) ? municipalityStr.Split(',').Select(x => x.ToLong()) : new long[0]);
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            // код "тип исполнителя" для столбца "юр"
            var jurCode = new List<string> { "0", "9", "11", "8", "15", "4" };
            
            // код "тип исполнителя" для столбца "дл"
            var dolzCode = new List<string> { "1", "10", "12", "13", "16", "19", "5", "3" };

            // код "тип исполнителя" для столбца "фл"
            var fizCode = new List<string> { "6", "7", "14" };

            reportParams.SimpleReportParams["StartDate"] = this.dateStart.ToLongDateString();
            reportParams.SimpleReportParams["EndDate"] = this.dateEnd.ToLongDateString();
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Section");

            IQueryable<long> resolutionIdsQuery1;
            IQueryable<long> resolutionIdsQuery2;
            IQueryable<long> resolutionIdsQuery3;

            var resolutionMunicipalityIdsList = this.GetResolutionMunicipalityIds(out resolutionIdsQuery1, out resolutionIdsQuery2, out resolutionIdsQuery3);
            var resolutionList = this.GetResolutions(resolutionIdsQuery1, resolutionIdsQuery2, resolutionIdsQuery3);
            var exeptResolutionsIds = this.GetExeptResolutionIdsList(resolutionIdsQuery1, resolutionIdsQuery2, resolutionIdsQuery3);
            var resolutionsPayFine = this.GetResolutionPayFine(resolutionIdsQuery1, resolutionIdsQuery2, resolutionIdsQuery3);
            var zjiMuDict = this.GetZjiMunicipalityDict();

            var resolutions = resolutionList.Where(x => !exeptResolutionsIds.Contains(x.ResolutionId)).ToList();

            var dataList = new List<ResolutionDataByGji>();
            
            foreach (var zji in zjiMuDict)
            {
                var zjiName = zji.Key.Trim().ToLower() == "государственная жилищная инспекция республики татарстан"
                                  ? "Центральный инспекторский отдел"
                                  : zji.Key;
                
                var curentGjiData = new ResolutionDataByGji();

                // список идентификаторов постановлений по текущей ГЖИ
                var currentResolutionsIds =
                    resolutionMunicipalityIdsList
                    .Where(x => zji.Value.Contains(x.MunicipalityId))
                    .Select(x => x.ResolutionId)
                    .ToList();

                curentGjiData.ZjiName = zjiName;

                curentGjiData.courtResolutionsPenaltyAmount =
                    resolutions.Where(x => currentResolutionsIds.Contains(x.ResolutionId))
                               .Where(x => x.SanctionCode == "1")
                               .Where(x => x.TypeInitiativeOrg == TypeInitiativeOrgGji.Court)
                               .Sum(x => x.PenaltyAmount);

                curentGjiData.jilResolutionsPenaltyAmount =
                    resolutions.Where(x => currentResolutionsIds.Contains(x.ResolutionId))
                               .Where(x => x.SanctionCode == "1")
                               .Where(x => x.TypeInitiativeOrg == TypeInitiativeOrgGji.HousingInspection)
                               .Sum(x => x.PenaltyAmount);
                
                curentGjiData.resolutionCountJur = resolutions
                    .Where(x => currentResolutionsIds.Contains(x.ResolutionId))
                    .Count(x => jurCode.Contains(x.ExecutantCode));
                
                curentGjiData.resolutionCountDl = resolutions
                    .Where(x => currentResolutionsIds.Contains(x.ResolutionId))
                    .Count(x => dolzCode.Contains(x.ExecutantCode));
                
                curentGjiData.resolutionCountFl = resolutions
                    .Where(x => currentResolutionsIds.Contains(x.ResolutionId))
                    .Count(x => fizCode.Contains(x.ExecutantCode));

                // идентификаторы постановлений по текущему ГЖИ (Кем вынесено == Суд)
                var courtResolutionsPaidedIds =
                    resolutions.Where(x => currentResolutionsIds.Contains(x.ResolutionId))
                               .Where(x => x.TypeInitiativeOrg == TypeInitiativeOrgGji.Court)
                               .Select(x => x.ResolutionId)
                               .ToList();

                // идентификаторы постановлений по текущему ГЖИ (Кем вынесено == Жил.инспекция)
                var jilResolutionsPaidedIds =
                    resolutions.Where(x => currentResolutionsIds.Contains(x.ResolutionId))
                               .Where(x => x.TypeInitiativeOrg == TypeInitiativeOrgGji.HousingInspection)
                               .Select(x => x.ResolutionId)
                               .ToList();

                curentGjiData.courtResolutionsPayFineAmount =
                    resolutionsPayFine
                    .Where(x => courtResolutionsPaidedIds.Contains(x.ResolutionId))
                    .Sum(x => x.Amount);

                curentGjiData.jilResolutionsPayFineAmount =
                    resolutionsPayFine
                    .Where(x => jilResolutionsPaidedIds.Contains(x.ResolutionId))
                    .Sum(x => x.Amount);

                curentGjiData.courtResolutionsAmountPercent = curentGjiData.courtResolutionsPenaltyAmount != 0
                                                                ? curentGjiData.courtResolutionsPayFineAmount / curentGjiData.courtResolutionsPenaltyAmount
                                                                : 0;

                curentGjiData.jilResolutionsAmountPercent = curentGjiData.jilResolutionsPenaltyAmount != 0
                                                                ? curentGjiData.jilResolutionsPayFineAmount / curentGjiData.jilResolutionsPenaltyAmount
                                                                : 0;

                dataList.Add(curentGjiData);
            }

            this.FillSection(reportParams, section, dataList);
        }

        private void FillSection(ReportParams reportParams, Section section, List<ResolutionDataByGji> data)
        {
            foreach (var resolutionDataByGji in data.OrderBy(x => x.ZjiName))
            {
                section.ДобавитьСтроку();
                section["ZJI"] = resolutionDataByGji.ZjiName;
                section["AllPenaltyImposedTribunal"] = resolutionDataByGji.courtResolutionsPenaltyAmount;
                section["AllPenaltyImposedGJI"] = resolutionDataByGji.jilResolutionsPenaltyAmount;
                section["AllPenaltyImposedAll"] = resolutionDataByGji.courtResolutionsPenaltyAmount + resolutionDataByGji.jilResolutionsPenaltyAmount;
                section["AllPenaltyImposedStatusJur"] = resolutionDataByGji.resolutionCountJur;
                section["AllPenaltyImposedStatusDL"] = resolutionDataByGji.resolutionCountDl;
                section["AllPenaltyImposedStatusFL"] = resolutionDataByGji.resolutionCountFl;
                section["AllPenaltyPaidTribunal"] = resolutionDataByGji.courtResolutionsPayFineAmount;
                section["AllPenaltyPaidGJI"] = resolutionDataByGji.jilResolutionsPayFineAmount;
                section["PercentTribunal"] = resolutionDataByGji.courtResolutionsAmountPercent;
                section["PercentGJI"] = resolutionDataByGji.jilResolutionsAmountPercent;
            }

            reportParams.SimpleReportParams["TotalPenaltyImposedTribunal"] = data.Sum(x => x.courtResolutionsPenaltyAmount);
            reportParams.SimpleReportParams["TotalPenaltyImposedGJI"] = data.Sum(x => x.jilResolutionsPenaltyAmount);
            var sumImposed = data.Sum(x => x.courtResolutionsPenaltyAmount) + data.Sum(x => x.jilResolutionsPenaltyAmount);
            reportParams.SimpleReportParams["SumImposed"] = sumImposed;
            reportParams.SimpleReportParams["AllPercentImposedTribunal"] = sumImposed != 0 ? data.Sum(x => x.courtResolutionsPenaltyAmount) / sumImposed : 0;
            reportParams.SimpleReportParams["AllPercentImposedGJI"] = sumImposed != 0 ? data.Sum(x => x.jilResolutionsPenaltyAmount) / sumImposed : 0;
            reportParams.SimpleReportParams["TotalPenaltyImposedStatusJur"] = data.Sum(x => x.resolutionCountJur);
            reportParams.SimpleReportParams["TotalPenaltyImposedStatusDL"] = data.Sum(x => x.resolutionCountDl);
            reportParams.SimpleReportParams["TotalPenaltyImposedStatusFL"] = data.Sum(x => x.resolutionCountFl);
            reportParams.SimpleReportParams["SumStatus"] = data.Sum(x => x.resolutionCountJur) + data.Sum(x => x.resolutionCountDl) + data.Sum(x => x.resolutionCountFl);
            reportParams.SimpleReportParams["TotalPenaltyPaidTribunal"] = data.Sum(x => x.courtResolutionsPayFineAmount);
            reportParams.SimpleReportParams["TotalPenaltyPaidGJI"] = data.Sum(x => x.jilResolutionsPayFineAmount);
            var sumTotalPercent = data.Sum(x => x.courtResolutionsPayFineAmount) + data.Sum(x => x.jilResolutionsPayFineAmount);
            reportParams.SimpleReportParams["SumPaid"] = sumTotalPercent;
            reportParams.SimpleReportParams["AllPercentPaidTribunal"] = sumTotalPercent != 0 ? data.Sum(x => x.courtResolutionsPayFineAmount) / sumTotalPercent : 0;
            reportParams.SimpleReportParams["AllPercentPaidGJI"] = sumTotalPercent != 0 ? data.Sum(x => x.jilResolutionsPayFineAmount) / sumTotalPercent : 0;
        }
        
        // возвращает идентификаторы постановлений с идентификаторами мун.образований (3 out-параметра - запросы идентификаторов постановлений)
        // идентификаторы мун.образований берутся из нарушений протоколов или домов постановлений прокуратуры или из домов проверок
        private List<ResolutionMunicipalityProxy> GetResolutionMunicipalityIds(out IQueryable<long> resolutionIdsQuery1, out IQueryable<long> resolutionIdsQuery2, out IQueryable<long> resolutionIdsQuery3)
        {
            var resolutionsFromProtocolsQuery = 
                this.Container.Resolve<IDomainService<ProtocolViolation>>().GetAll()
                .Join(
                    this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll(),
                    x => x.Document.Id,
                    y => y.Parent.Id,
                    (a, b) => new { ProtocolViolation = a, DocumentGjiChildren = b })
                 .WhereIf(this.dateStart != DateTime.MinValue, x => x.DocumentGjiChildren.Children.DocumentDate >= this.dateStart)
                 .WhereIf(this.dateEnd != DateTime.MinValue, x => x.DocumentGjiChildren.Children.DocumentDate <= this.dateEnd)
                 .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.ProtocolViolation.InspectionViolation.RealityObject.Municipality.Id));

            var resolutionsFromResolProsQuery =
                this.Container.Resolve<IDomainService<ResolProsRealityObject>>().GetAll()
                .Join(
                    this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll(),
                    x => x.ResolPros.Id,
                    y => y.Parent.Id,
                    (c, d) => new { ResolProsRealityObject = c, DocumentGjiChildren = d })
                 .WhereIf(this.dateStart != DateTime.MinValue, x => x.DocumentGjiChildren.Children.DocumentDate >= this.dateStart)
                 .WhereIf(this.dateEnd != DateTime.MinValue, x => x.DocumentGjiChildren.Children.DocumentDate <= this.dateEnd)
                 .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.ResolProsRealityObject.RealityObject.Municipality.Id));

            var resolutionsFromInspectionsQuery =
                this.Container.Resolve<IDomainService<InspectionGjiRealityObject>>().GetAll()
                .Join(
                    this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll(),
                        x => x.Inspection.Id,
                        y => y.Parent.Inspection.Id,
                        (e, f) => new { InspectionGjiRealityObject = e, DocumentGjiChildren = f })
                .WhereIf(this.dateStart != DateTime.MinValue, x => x.DocumentGjiChildren.Children.DocumentDate >= this.dateStart)
                .WhereIf(this.dateEnd != DateTime.MinValue, x => x.DocumentGjiChildren.Children.DocumentDate <= this.dateEnd)
                .Where(x => x.DocumentGjiChildren.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
                .Where(x => x.DocumentGjiChildren.Parent.TypeDocumentGji == TypeDocumentGji.Protocol || x.DocumentGjiChildren.Parent.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor)
                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.InspectionGjiRealityObject.RealityObject.Municipality.Id));
                

            resolutionIdsQuery1 = resolutionsFromProtocolsQuery.Select(x => x.DocumentGjiChildren.Children.Id);
            resolutionIdsQuery2 = resolutionsFromResolProsQuery.Select(x => x.DocumentGjiChildren.Children.Id);
            resolutionIdsQuery3 = resolutionsFromInspectionsQuery.Select(x => x.DocumentGjiChildren.Children.Id);

            return resolutionsFromProtocolsQuery
                .Where(x => x.ProtocolViolation.InspectionViolation.RealityObject != null)
                .Select(x => new ResolutionMunicipalityProxy
                {
                    ResolutionId = x.DocumentGjiChildren.Children.Id,
                    MunicipalityId = x.ProtocolViolation.InspectionViolation.RealityObject.Municipality.Id
                })
                .ToList()
                .Union(
                    resolutionsFromResolProsQuery
                    .Where(x => x.ResolProsRealityObject.RealityObject != null)
                    .Select(x => new ResolutionMunicipalityProxy
                    {
                        ResolutionId = x.DocumentGjiChildren.Children.Id,
                        MunicipalityId = x.ResolProsRealityObject.RealityObject.Municipality.Id
                    }).ToList())
                .Union(
                    resolutionsFromInspectionsQuery
                    .Where(x => x.InspectionGjiRealityObject.RealityObject != null)
                    .Select(x => new ResolutionMunicipalityProxy
                        {
                            ResolutionId = x.DocumentGjiChildren.Children.Id,
                            MunicipalityId = x.InspectionGjiRealityObject.RealityObject.Municipality.Id
                        }).ToList())
                .Distinct()
                .ToList();
        }

        // возвращает список постановлений
        private List<ResolutionInfoProxy> GetResolutions(IQueryable<long> resolutionIdsQuery1, IQueryable<long> resolutionIdsQuery2, IQueryable<long> resolutionIdsQuery3)
        {
            return this.Container.Resolve<IDomainService<Resolution>>().GetAll()
                    .Where(x => resolutionIdsQuery1.Contains(x.Id) 
                        || resolutionIdsQuery2.Contains(x.Id) 
                        || resolutionIdsQuery3.Contains(x.Id))
                    .Select(x => new ResolutionInfoProxy
                    {
                        ResolutionId = x.Id,
                        SanctionCode = x.Sanction.Code,
                        TypeInitiativeOrg = x.TypeInitiativeOrg,
                        PenaltyAmount = x.PenaltyAmount,
                        ExecutantCode = x.Executant.Code,
                        Paided = x.Paided
                    })
                    .ToList();
        }

        // возвращает список идентификаторов постановлений, не попадающих в отчет в зависимости от пользовательских параметров
        private List<long> GetExeptResolutionIdsList(IQueryable<long> resolutionIdsQuery1, IQueryable<long> resolutionIdsQuery2, IQueryable<long> resolutionIdsQuery3)
        {
            var exeptList = new List<long>();
            
            var disputeResolutions = 
                this.Container.Resolve<IDomainService<ResolutionDispute>>().GetAll()
                    .Where(x => resolutionIdsQuery1.Contains(x.Resolution.Id) 
                        || resolutionIdsQuery2.Contains(x.Resolution.Id) 
                        || resolutionIdsQuery3.Contains(x.Resolution.Id))
                    .Select(x => new { x.Resolution.Id, x.CourtVerdict.Code })
                    .ToList();

            if (this.canceled || this.remarked || this.returned)
            {
                if (this.canceled)
                {
                    exeptList.AddRange(disputeResolutions.Where(x => x.Code == "2").Select(x => x.Id).ToList());
                }

                if (this.remarked)
                {
                    exeptList.AddRange(disputeResolutions.Where(x => x.Code == "3").Select(x => x.Id).ToList());
                }

                if (this.returned)
                {
                    exeptList.AddRange(disputeResolutions.Where(x => x.Code != "2" && x.Code != "3").Select(x => x.Id).ToList());
                }
            }

            return exeptList;
        }

        // возвращает оплаты штрафов
        private List<ResolutionPayFineProxy> GetResolutionPayFine(IQueryable<long> resolutionIdsQuery1, IQueryable<long> resolutionIdsQuery2, IQueryable<long> resolutionIdsQuery3)
        {
            return this.Container.Resolve<IDomainService<ResolutionPayFine>>().GetAll()
                    .Where(x => resolutionIdsQuery1.Contains(x.Resolution.Id)
                        || resolutionIdsQuery2.Contains(x.Resolution.Id)
                        || resolutionIdsQuery3.Contains(x.Resolution.Id))
                    .Select(x => new ResolutionPayFineProxy { ResolutionId = x.Resolution.Id, Amount = x.Amount })
                    .ToList();
        }

        private Dictionary<string, List<long>> GetZjiMunicipalityDict()
        {
            return this.Container.Resolve<IDomainService<ZonalInspectionMunicipality>>().GetAll()
                    .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.Municipality.Id))
                    .Select(x => new
                    {
                        muId = x.Municipality.Id,
                        zjiName = string.IsNullOrEmpty(x.ZonalInspection.ZoneName) ? x.ZonalInspection.Name : x.ZonalInspection.ZoneName
                    })
                    .AsEnumerable()
                    .OrderBy(x => x.zjiName)
                    .GroupBy(x => x.zjiName)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Select(y => y.muId).ToList());
        }
    }

    internal class ResolutionDataByGji
    {
        /// <summary>
        /// Наименование ЗЖИ
        /// </summary>
        public string ZjiName;

        /// <summary>
        /// сумма административных штрафов постановлений, вынесенных судом
        /// </summary>
        public decimal? courtResolutionsPenaltyAmount;

        /// <summary>
        /// сумма административных штрафов постановлений, вынесенных жил.инспекцией
        /// </summary>
        public decimal? jilResolutionsPenaltyAmount;

        /// <summary>
        /// количество постановлений с типом исполнителя - Юр.лицо.
        /// </summary>
        public int resolutionCountJur;

        /// <summary>
        /// количество постановлений с типом исполнителя - Должностное лицо
        /// </summary>
        public int resolutionCountDl;

        /// <summary>
        /// количество постановлений с типом исполнителя - Физ.лицо
        /// </summary>
        public int resolutionCountFl;

        /// <summary>
        /// оплаты штрафов по постановлениям, вынесненных судом
        /// </summary>
        public decimal? courtResolutionsPayFineAmount;

        /// <summary>
        /// оплаты штрафов по постановлениям, вынесненных жил.инспекцией
        /// </summary>
        public decimal? jilResolutionsPayFineAmount;

        /// <summary>
        /// процент оплатченых штрафов по постановлениям, вынесненных судом
        /// </summary>
        public decimal? courtResolutionsAmountPercent;

        /// <summary>
        /// процент оплатченых штрафов по постановлениям, вынесненных жил.инспекцией
        /// </summary>
        public decimal? jilResolutionsAmountPercent;
    }

    internal class ResolutionPayFineProxy
    {
        public long ResolutionId;

        public decimal? Amount;
    }

    internal class ResolutionInfoProxy
    {
        public long ResolutionId;

        public string SanctionCode;

        public TypeInitiativeOrgGji TypeInitiativeOrg;

        public decimal? PenaltyAmount;

        public string ExecutantCode;

        public YesNoNotSet Paided;
    }

    internal class ResolutionMunicipalityProxy : IEquatable<ResolutionMunicipalityProxy>
    {
        public long ResolutionId;

        public long MunicipalityId;

        public bool Equals(ResolutionMunicipalityProxy other)
        {
            if (Object.ReferenceEquals(other, null)) { return false; }
            if (Object.ReferenceEquals(this, other)) { return true; }

            var res = this.ResolutionId == other.ResolutionId && this.MunicipalityId == other.MunicipalityId;
            return res;
        }

        public override int GetHashCode()
        {
            int hashRoId = this.ResolutionId.GetHashCode();
            int hashMuId = this.ResolutionId.GetHashCode();

            return hashRoId ^ hashMuId;
        }
    }
}