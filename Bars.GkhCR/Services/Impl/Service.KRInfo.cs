namespace Bars.GkhCr.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Bars.GkhCr.Services.DataContracts;
    using Bars.GkhCr.Services.DataContracts.KRInfo;
    using B4.Modules.FileStorage;

    using Bars.Gkh.ConfigSections.Cr;
    using Bars.Gkh.ConfigSections.Cr.Enums;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.DomainService;

    /// <summary>
    /// Реализация метода KRInfo
    /// </summary>
    public partial class Service
    {
        #region Cache

        // ключ ObjectCrId
        protected Dictionary<long, ContractorOrg[]> DictContractor;

        // ключ ContragentId
        protected Dictionary<long, ContragentContact> DictContact;

        // ключ WorkId#PeriodId
        protected Dictionary<string, Photo[]> DictPhoto;

        // ключ ObjectCrId
        protected Dictionary<long, Work[]> DictWork;

        // ключ TypeWorkId
        protected Dictionary<long, WorkAct[]> DictWorkActs;

        // ключ ObjectCrId, ключ TypeWorkId
        protected Dictionary<long, Dictionary<long, Contractor[]>> DictContractorsName;

        protected Dictionary<long, ContractorWork[]> DictBuildContractTypeWorkCr;

        // ключ ObjectCrId
        protected Dictionary<long, FundingSources[]> DictSourcesoffinancing;
        #endregion

        public IDomainService<BuildContractTypeWork> BuildContractTypeWorkService { get; set; }
        public IDomainService<BuildContract> BuildContractService { get; set; }
        public IDomainService<ContragentContact> ContragentContactService { get; set; }
        public IDomainService<RealityObjectImage> RealityObjectImageService { get; set; }
        public IDomainService<PerformedWorkAct> PerformedWorkActService { get; set; }
        public IDomainService<TypeWorkCr> TypeWorkCrService { get; set; }
        public IDomainService<ObjectCr> ObjectCrService { get; set; }
        public IDomainService<FinanceSourceResource> FinanceSourceResourceService { get; set; }
        public IOverhaulViewModels OverhaulViewModels { get; set; }

        public IFileManager FileManager { get; set; }

        public IDpkrTypeWorkService DpkrTypeWorkService { get; set; }

        /// <summary>
        /// Метод получения данных по программе капитального ремонта
        /// </summary>
        /// <param name="houseId">Id дома</param>
        /// <param name="houseofRegoperator">Выбирать или нет только те дома, у котроых "Способ формирования фонда" = "Счет рег. оператора"</param>
        /// <returns></returns>
        public KRInfoResponse KRInfo(string houseId, string houseofRegoperator = null)
        {
            long id;
            var programsKr = new ProgramKR[] { };

            if (long.TryParse(houseId, out id))
            {
                this.CacheContractorOrg(id);
                this.CacheWork(id);
                this.CacheSourcesoffinancing(id);

                programsKr =this.ObjectCrService.GetAll()
                        .WhereIf(houseofRegoperator.ToBool(), x => x.RealityObject.AccountFormationVariant == CrFundFormationType.RegOpAccount)
                        .Where(x => x.RealityObject.Id == id)
                        .Where(x => x.ProgramCr.UsedInExport)
                        .Where(x => x.ProgramCr.TypeVisibilityProgramCr == TypeVisibilityProgramCr.Full)
                        .Select(x => new ObjectCr(
                                        x.RealityObject, 
                                        new ProgramCr
                                        {
                                            Id = x.ProgramCr.Id,
                                            Name = x.ProgramCr.Name,
                                            Period = new Period { DateEnd = x.ProgramCr.Period.DateEnd }
                                        })
                                    {
                                        Id = x.Id
                                    })
                        .AsEnumerable()
                        .Select(this.GetProgramInfo)
                        .ToArray();

                this.CleanCache();
            }

            var result = programsKr.Length == 0 ? Result.DataNotFound : Result.NoErrors;

            return new KRInfoResponse { ProgramsKR = programsKr, Result = result };
        }

        protected void CleanCache()
        {
            this.DictContractor.Clear();
            this.DictContact.Clear();
            this.DictPhoto.Clear();
            this.DictWork.Clear();
            this.DictWorkActs.Clear();
            this.DictBuildContractTypeWorkCr.Clear();
            this.DictSourcesoffinancing.Clear();
        }

        protected void CacheContractorOrg(long robjectId, bool methodKrContracts = false)
        {
            this.CacheContractorContact(robjectId);
            this.CacheBuildContractTypeWork(robjectId);

            this.DictContractor = this.BuildContractService.GetAll()
                .Where(x => x.ObjectCr.RealityObject.Id == robjectId)
                .Where(x => x.Builder != null)
                .Select(x => new
                {
                    x.Id,
                    ObjectCrId = x.ObjectCr.Id,
                    IdDoc = x.Id,
                    BuilderId =x.Builder.Id,
                    x.Builder.Contragent.Name,
                    LegalAddress = x.Builder.Contragent.JuridicalAddress,
                    ContragentId = (long?)x.Builder.Contragent.Id,
                    x.Sum,
                    x.DocumentName,
                    x.DocumentNum,
                    x.DocumentDateFrom
                })
                .AsEnumerable()
                .GroupBy(x => x.ObjectCrId)
                .ToDictionary(
                    x => x.Key, 
                    y => y.Select(x => new ContractorOrg
                        {
                            IdDoc = x.IdDoc,
                            Id = x.BuilderId,
                            Name = x.Name,
                            LegalAddress = x.LegalAddress,
                            NameBoss = this.GetContact(x.ContragentId).Return(z => z.FullName),
                            ContactBoss = this.GetContact(x.ContragentId).Return(z => z.Name),
                            ContractorWork = methodKrContracts ? this.DictBuildContractTypeWorkCr.Get(x.Id) : null,
                            DocName = x.DocumentName,
                            DocNum = x.DocumentNum,
                            DateDoc = x.DocumentDateFrom?.ToShortDateString()
                    })
                .ToArray());
        }

        protected  void CacheContractorContact(long robjectId)
        {
            this.DictContact = this.ContragentContactService.GetAll()
                .Where(y => this.BuildContractService.GetAll()
                    .Where(x => x.ObjectCr.RealityObject.Id == robjectId)
                    .Any(x => x.Builder.Contragent == y.Contragent))
                .Where(x => x.Position.Code == "1" || x.Position.Code == "4")
                .Where(x => !x.DateEndWork.HasValue || x.DateEndWork >= DateTime.Today)
                .Where(x => !x.DateStartWork.HasValue || x.DateStartWork <= DateTime.Today)
                .GroupBy(x => x.Contragent.Id)
                .ToDictionary(x => x.Key, y => y.FirstOrDefault());
        }

        protected  void CachePhoto(long robjectId)
        {
            this.DictPhoto = this.RealityObjectImageService.GetAll()
                    .Where(x => x.RealityObject.Id == robjectId)
                    .Where(x => x.ImagesGroup != ImagesGroup.PictureHouse)
                    .Where(x => x.WorkCr != null && x.Period != null)
                    .Select(x => new
                        {
                            WorkId = x.WorkCr.Id,
                            PeriodId = x.Period.Id,
                            x.Id
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.WorkId + "#" + x.PeriodId)
                    .ToDictionary(x => x.Key, y => y.Select(x => new Photo { Id = x.Id }).ToArray());
        }

        protected void CacheWork(long robjectId, bool methodKrContracts = false)
        {
            this.CacheWorkAct(robjectId, methodKrContracts);
            this.CachePhoto(robjectId);
            this.CacheContractorsName(robjectId);

            var objectCrIds = this.ObjectCrService.GetAll()
                .Where(x => x.RealityObject.Id == robjectId)
                .Select(x => x.Id)
                .ToArray();

            var dpkrWorkDict = this.DpkrTypeWorkService.GetWorksByObjectCr(objectCrIds);

            this.DictWork = this.TypeWorkCrService.GetAll()
                .WhereContains(x => x.ObjectCr.Id, objectCrIds)
                .Select(x => new
                {
                    ObjectCrId = x.ObjectCr.Id,
                    x.Id,                      
                    Measure = x.Work.UnitMeasure.Name,
                    PlanYear = x.YearRepair,
                    x.Sum,
                    FinishDate = x.DateEndWork.HasValue
                        ? x.DateEndWork.Value.ToShortDateString()
                        : null,
                    IdWork = x.Id,
                    x.Work.Name,
                    Percent = x.PercentOfCompletion.GetValueOrDefault(),
                    StartDate = x.DateStartWork.HasValue
                        ? x.DateStartWork.Value.ToShortDateString()
                        : null,
                    WorkId = x.Work.Id,
                    PeriodId = x.ObjectCr.ProgramCr.Period.Id,
                    x.Volume
                })
                .AsEnumerable()
                .GroupBy(x => x.ObjectCrId)
                .ToDictionary(
                    x => x.Key, 
                    y => y.Select(x => new Work
                    {
                        IdWork = x.Id,
                        Name = x.Name,                           
                        Measure = x.Measure,
                        PlanYear = x.PlanYear.ToString(),
                        PlanVolume = (x.Volume.GetValueOrDefault() == 0
                                ? dpkrWorkDict.Get(x.Id)?.Volume 
                                : x.Volume) 
                            ?? 0m,
                        PlanSum = (x.Sum.GetValueOrDefault() == 0
                                ? dpkrWorkDict.Get(x.Id)?.Sum
                                : x.Sum) 
                            ?? 0m,
                        StartDate = x.StartDate,
                        FinishDate = x.FinishDate,
                        Percent = x.Percent,
                        WorkActs = this.GetWorkActs(x.Id),
                        PhotoArchive = this.GetPhotos(x.WorkId, x.PeriodId),
                        Sum = this.GetWorkActs(x.Id).Return(z => z.SafeSum(a => a.Sum)),
                        FactVolume = this.GetWorkActs(x.Id).Return(z => z.SafeSum(a => a.Volume)),
                        Volume = x.Volume ?? 0m,
                        Contractors = this.GetContractors(x.ObjectCrId, x.Id)
                    })
                    .ToArray());
        }

        protected  void CacheWorkAct(long robjectId, bool methodKRContracts)
        {
            this.DictWorkActs = this.PerformedWorkActService.GetAll()
                .Where(x => x.ObjectCr.RealityObject.Id == robjectId)
                .Select(x => new
                    {
                        TypeWorkId = x.TypeWorkCr.Id,
                        x.Id,
                        Sum = x.Sum ?? 0m,
                        Volume = x.Volume ?? 0m,
                        Date = x.DateFrom,
                        Number = x.DocumentNum,
                        App = methodKRContracts ? x.AdditionFile : null
                })
                .AsEnumerable()
                .GroupBy(x => x.TypeWorkId)
                .ToDictionary(
                    x => x.Key, 
                    y => y.Select(x => new WorkAct
                        {
                            Id = x.Id,
                            Date = x.Date.GetValueOrDefault(),
                            Number = x.Number,
                            Sum = x.Sum,
                            Volume = x.Volume,
                            File = x.App != null ? new File
                            {
                                Id = x.App.Id,
                                Name = x.App.Name,
                                Base64 = this.FileManager.GetBase64String(x.App),
                                Extention = x.App.Extention
                            } : null
                        })
                        .ToArray());
        }

        protected  void CacheContractorsName(long robjectId)
        {
            var buildContractIds = this.BuildContractService.GetAll()
                .Where(x => x.ObjectCr.RealityObject.Id == robjectId)
                .Select(x => x.Id);

            this.DictContractorsName = this.BuildContractTypeWorkService.GetAll()
                .Where(x => buildContractIds.Contains(x.BuildContract.Id))
                .Select(x => new
                                 {
                                     TypeWorkId = x.TypeWork.Id,
                                     BuilderName = x.BuildContract.Builder.Contragent.Name,
                                     ObjectCrId = x.BuildContract.ObjectCr.Id
                                 })
                .AsEnumerable()
                .GroupBy(x => x.ObjectCrId)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.TypeWorkId)
                            .ToDictionary(
                                k => k.Key,
                                v => v.Select(y => new Contractor
                                                    {
                                                        Name = y.BuilderName
                                                    })
                                    .ToArray()));
        }

        protected void CacheSourcesoffinancing(long robjectId)
        {
            var formFinanceSource = this.Container.GetGkhConfig<GkhCrConfig>().General.FormFinanceSource;

            if (this.OverhaulViewModels != null && (formFinanceSource == FormFinanceSource.WithTypeWork))
            {
                var objectCrIds = this.ObjectCrService.GetAll()
                    .Where(x => x.RealityObject.Id == robjectId)
                    .Select(x => x.Id)
                    .ToArray();

                this.DictSourcesoffinancing = new Dictionary<long, FundingSources[]>();

                foreach (var objectCrId in objectCrIds)
                {
                    var baseParams = new BaseParams();
                    baseParams.Params.Add("objectCrId", objectCrId);

                    var finSource = this.OverhaulViewModels.FinanceSourceResList(this.FinanceSourceResourceService, baseParams)
                        .Data.Select(
                            x => new FundingSources
                            {
                                Id = x.Id,
                                FinanceSourceId = x.FinanceSourceId.GetValueOrDefault(),
                                Name = x.FinanceSourceName,
                                IdWork = x.TypeWorkCrId.GetValueOrDefault(),
                                TypeWork = x.TypeWorkCr,
                                Year = x.Year.GetValueOrDefault(),
                                BudgetMu = x.BudgetMu.GetValueOrDefault(),
                                BudgetSubject = x.BudgetSubject.GetValueOrDefault(),
                                OwnerResource = x.OwnerResource.GetValueOrDefault(),
                                FundResource = x.FundResource.GetValueOrDefault(),
                                OtherResource = x.OtherResource.GetValueOrDefault()
                            })
                        .ToArray();

                    this.DictSourcesoffinancing.Add(objectCrId, finSource);
                }
            }
            else
            {
                this.DictSourcesoffinancing = this.FinanceSourceResourceService.GetAll()
                    .Where(x => x.ObjectCr.RealityObject.Id == robjectId)
                    .Select(
                        x => new
                        {
                            ObjectCrId = x.ObjectCr.Id,
                            x.Id,
                            FinanceSourceId = x.FinanceSource.Id,
                            x.FinanceSource.Name,
                            IdWork = x.TypeWorkCr.Id,
                            TypeWork = x.TypeWorkCr.Work.Name,
                            x.Year,
                            BudgetMu = x.BudgetMu.GetValueOrDefault(),
                            BudgetSubject = x.BudgetSubject.GetValueOrDefault(),
                            OwnerResource = x.OwnerResource.GetValueOrDefault(),
                            FundResource = x.FundResource.GetValueOrDefault()

                        })
                    .Distinct()
                    .AsEnumerable()
                    .GroupBy(x => x.ObjectCrId)
                    .ToDictionary(
                        x => x.Key,
                        y => y.Select(
                                x => new FundingSources
                                {
                                    Id = x.Id,
                                    FinanceSourceId = x.FinanceSourceId,
                                    Name = x.Name,
                                    IdWork = x.IdWork,
                                    TypeWork = x.TypeWork,
                                    Year = x.Year.GetValueOrDefault(),
                                    BudgetMu = x.BudgetMu,
                                    BudgetSubject = x.BudgetSubject,
                                    FundResource = x.FundResource,
                                    OwnerResource = x.OwnerResource,
                                    OtherResource = 0
                                })
                            .ToArray());
            }
        }

        protected  ProgramKR GetProgramInfo(ObjectCr objectCr)
        {
            return new ProgramKR
            {
                IdProg = objectCr.ProgramCr.Id,
                NameProg = objectCr.ProgramCr.Name,
                Year = objectCr.ProgramCr.Period.DateEnd.GetValueOrDefault().Year,
                ContractorOrgs = this.DictContractor.Get(objectCr.Id) ?? new ContractorOrg[0],
                Works = this.DictWork.Get(objectCr.Id) ?? new Work[0],
                FundingSources = this.DictSourcesoffinancing.Get(objectCr.Id) ?? new FundingSources[0]
            };
        }

        protected  Photo[] GetPhotos(long workId, long periodId)
        {
            return this.DictPhoto.Get(workId + "#" + periodId) ?? new Photo[0];
        }

        protected  ContragentContact GetContact(long? contragentId)
        {
            return this.DictContact.Get(contragentId ?? 0);
        }

        protected  WorkAct[] GetWorkActs(long typeWorkId)
        {
            return this.DictWorkActs.Get(typeWorkId) ?? new WorkAct[0];
        }

        protected  Contractor[] GetContractors(long objectCrId, long typeWorkId)
        {
            if (this.DictContractorsName.ContainsKey(objectCrId))
            {
                if (this.DictContractorsName[objectCrId].ContainsKey(typeWorkId))
                {
                    return this.DictContractorsName[objectCrId][typeWorkId];
                }
            }

            return new Contractor[0];
        }

        protected void CacheBuildContractTypeWork(long robjectId)
        {
            var query = this.BuildContractService.GetAll()
                .Where(x => x.ObjectCr.RealityObject.Id == robjectId)
                .Where(x => x.Builder != null).Select(x => x.Id);

            this.DictBuildContractTypeWorkCr = this.BuildContractTypeWorkService.GetAll()
                .Where(x => query.Contains(x.BuildContract.Id))
                .AsEnumerable()
                .GroupBy(x => x.BuildContract.Id)
                .ToDictionary(
                    x => x.Key,
                    y => y.Select(
                        x =>
                            new ContractorWork
                            {
                                Id = x.TypeWork.Id,
                                Sum = x.Sum ?? 0
                            })
                        .ToArray());

        }
    }
}